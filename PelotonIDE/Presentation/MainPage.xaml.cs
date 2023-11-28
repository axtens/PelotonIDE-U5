using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;

using Newtonsoft.Json;

using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using Windows.Storage;
using Windows.System;

using FactorySettingsStructure = System.Collections.Generic.Dictionary<string, object>;
using InterpreterParametersStructure = System.Collections.Generic.Dictionary<string,
    System.Collections.Generic.Dictionary<string, object>>;
using InterpreterParameterStructure = System.Collections.Generic.Dictionary<string, object>;
using LanguageConfigurationStructure = System.Collections.Generic.Dictionary<string,
    System.Collections.Generic.Dictionary<string,
        System.Collections.Generic.Dictionary<string, string>>>;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        readonly Dictionary<object, CustomRichEditBox> _richEditBoxes = [];
        bool outputPanelShowing = true;
        enum OutputPanelPosition
        {
            Left,
            Bottom,
            Right
        }
        string? InterfaceLanguageName = "English";
        long InterfaceLanguageID = 0;

        string? LastSelectedInterpreterLanguageName;
        long LastSelectedInterpreterLanguageID;

        bool LastSelectedVariableLength;
        //bool LastSelectedSpaced;
        long LastSelectedQuietude = 2;

        OutputPanelPosition outputPanelPosition = OutputPanelPosition.Bottom;
        string? Engine = string.Empty;
        string? Scripts = string.Empty;
        string? InterpreterOld = string.Empty;
        string? InterpreterNew = string.Empty;

        //string engineArguments = string.Empty;

        InterpreterParametersStructure? GlobalInterpreterParameters = [];
        InterpreterParametersStructure? PerTabInterpreterParameters = [];
        /// <summary>
        /// does not change
        /// </summary>
        LanguageConfigurationStructure? LanguageSettings;
        FactorySettingsStructure? FactorySettings = [];
        readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public LanguageConfigurationStructure? LanguageSettings1 { get => LanguageSettings; set => LanguageSettings = value; }

        public MainPage()
        {
            this.InitializeComponent();

            //System.Timers.Timer t = new(1000)
            //{
            //    AutoReset = true,
            //    Enabled = true,

            //    //Elapsed += new ElapsedEventHandler(OnTimedEvent_Tick)
            //};
            //t.Elapsed += TimerTick;
            //t.Start();
            // GetGlobals();
            CustomRichEditBox customREBox = new()
            {
                Tag = tab1.Tag
            };
            customREBox.KeyDown += RichEditBox_KeyDown;
            customREBox.AcceptsReturn = true;

            // richEditBox.Background = 
            tabControl.Content = customREBox;
            _richEditBoxes[customREBox.Tag] = customREBox;
            tab1.TabSettingsDict = null;
            tabControl.SelectedItem = tab1;
            App._window.Closed += MainWindow_Closed;
            // InterpreterLanguageSelectionBuilder(contextualLanguagesFlyout, "mnuLanguage", some_click);
            UpdateTabCommandLine();
            customREBox.Focus(FocusState.Keyboard);
            customREBox.Document.Selection.SetIndex(Microsoft.UI.Text.TextRangeUnit.Character,1, false);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                return;
            }

            var parameters = (NavigationData)e.Parameter;

            //var selectedLanguage = parameters.selectedLangauge;
            //var translatedREB = parameters.translatedREB;
            switch (parameters.Source)
            {
                case "IDEConfig":
                    LocalSettings.Values[LocalSettings.Values["Engine"].ToString()] = parameters.KVPs["Interpreter"].ToString();
                    LocalSettings.Values["Scripts"] = parameters.KVPs["Scripts"].ToString();
                    break;
                case "TranslatePage":
                    CustomRichEditBox richEditBox = new()
                    {
                        IsDirty = true,
                        IsRTF = true,
                    };
                    richEditBox.KeyDown += RichEditBox_KeyDown;
                    richEditBox.AcceptsReturn = true;
                    richEditBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.UnicodeBidi, parameters.KVPs["TargetText"].ToString());

                    CustomTabItem navigationViewItem = new()
                    {
                        Content = "Tab " + (tabControl.MenuItems.Count + 1),
                        Tag = "Tab" + (tabControl.MenuItems.Count + 1),
                        IsNewFile = true,
                        TabSettingsDict = Clone(PerTabInterpreterParameters),
                        Height = 30,
                    };
                    navigationViewItem.TabSettingsDict["Language"]["Defined"] = true;
                    navigationViewItem.TabSettingsDict["Language"]["Value"] = (long)parameters.KVPs["TargetLanguage"];

                    richEditBox.Tag = navigationViewItem.Tag;
                    tabControl.Content = richEditBox;

                    _richEditBoxes[richEditBox.Tag] = richEditBox;
                    tabControl.MenuItems.Add(navigationViewItem);
                    tabControl.SelectedItem = navigationViewItem;
                    richEditBox.Focus(FocusState.Keyboard);
                    UpdateLanguageName(navigationViewItem.TabSettingsDict);
                    UpdateTabCommandLine();
                    break;
            }
        }

        private InterpreterParametersStructure CopyFromGlobalCodeRunCargo()
        {
            InterpreterParametersStructure tsj = [];
            foreach (var key in GlobalInterpreterParameters.Keys)
            {
                var kvp = GlobalInterpreterParameters[key];
                tsj.Add(key, kvp);
            }
            return tsj;
        }

        private Dictionary<string, object> GetTabSettingsFromRegistry()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            Dictionary<string, object> dict = [];
            foreach (var value in localSettings.Values)
            {
                if (value.Key.StartsWith("tab_"))
                    dict.Add(value.Key, value.Value);
            }
            return dict;
        }

        public static async Task<InterpreterParametersStructure?> GetGlobalInterpreterParameters()
        {
            var tabSettingStorage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\GlobalInterpreterParameters.json"));
            string tabSettings = File.ReadAllText(tabSettingStorage.Path);
            return JsonConvert.DeserializeObject<InterpreterParametersStructure>(tabSettings);
        }

        public static async Task<InterpreterParametersStructure?> GetPerTabInterpreterParameters()
        {
            var tabSettingStorage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\PerTabInterpreterParameters.json"));
            string tabSettings = File.ReadAllText(tabSettingStorage.Path);
            return JsonConvert.DeserializeObject<InterpreterParametersStructure>(tabSettings);
        }

        private static async Task<LanguageConfigurationStructure?> GetLanguageConfiguration()
        {
            var languageConfig = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\LanguageConfiguration.json"));
            string languageConfigString = File.ReadAllText(languageConfig.Path);
            var languages = JsonConvert.DeserializeObject<LanguageConfigurationStructure>(languageConfigString);
            languages.Remove("Viet");
            return languages;
        }

        private async void InterfaceLanguageSelectionBuilder(MenuFlyoutSubItem menuBarItem, string menuLabel, RoutedEventHandler routedEventHandler)
        {
            //var tabset = await GetGlobalInterpreterParameters();

            if (InterfaceLanguageName == null || !LanguageSettings.ContainsKey(InterfaceLanguageName))
            {
                return;
            }

            // what is current language?
            var globals = LanguageSettings[InterfaceLanguageName]["GLOBAL"];
            var count = LanguageSettings.Keys.Count;
            for (var i = 0; i < count; i++)
            {
                var names = from lang in LanguageSettings.Keys
                            where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                            let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                            select name;
                if (names.Any())
                {
                    MenuFlyoutItem menuFlyoutItem = new()
                    {
                        Text = globals[$"{100 + i + 1}"],
                        Name = names.First(),
                        Foreground = names.First() == InterfaceLanguageName ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                        Background = names.First() == InterfaceLanguageName ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                    };
                    menuFlyoutItem.Click += routedEventHandler; //  Internationalization_Click;
                    menuBarItem.Items.Add(menuFlyoutItem);
                }
            }
        }

        private async void InterpreterLanguageSelectionBuilder(MenuBarItem menuBarItem, string menuLabel, RoutedEventHandler routedEventHandler)
        {
            var tabset = await GetGlobalInterpreterParameters();

            LanguageSettings = await GetLanguageConfiguration();

            if (InterfaceLanguageName == null || !LanguageSettings.ContainsKey(InterfaceLanguageName))
            {
                return;
            }

            var foo = from item in menuBarItem.Items where item.GetType().Name == "MenuFlyoutSubItem" && item.Name == menuLabel select item;
            if (foo.Any())
            {
                return;
            }

            var sub = new MenuFlyoutSubItem
            {
                // <!--<MenuFlyoutSubItem Text="Choose interface language" BorderBrush="LightGray" BorderThickness="1" names:Name="SettingsBar_InterfaceLanguage" />-->
                Text = LanguageSettings[InterfaceLanguageName]["frmMain"][menuLabel],
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderBrush = new SolidColorBrush() { Color = Colors.LightGray },
                Name = menuLabel
            };

            //var items = new List<MenuFlyoutItem>();

            // what is current language?
            var globals = LanguageSettings[InterfaceLanguageName]["GLOBAL"];
            var count = LanguageSettings.Keys.Count;
            for (var i = 0; i < count; i++)
            {
                var names = from lang in LanguageSettings.Keys
                            where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                            let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                            select name;
                if (names.Any())
                {
                    MenuFlyoutItem menuFlyoutItem = new()
                    {
                        Text = globals[$"{100 + i + 1}"],
                        Name = names.First(),
                        Foreground = names.First() == LastSelectedInterpreterLanguageName ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                        Background = names.First() == LastSelectedInterpreterLanguageName ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                    };
                    menuFlyoutItem.Click += routedEventHandler; //  Internationalization_Click;
                    sub.Items.Add(menuFlyoutItem);
                }
            }
            menuBarItem.Items.Add(sub);
        }
        private static void ControlHighligter(MenuFlyoutItem? menuFlyoutItem, bool onish)
        {
            if (onish)
            {
                menuFlyoutItem.Background = new SolidColorBrush(Colors.Black);
                menuFlyoutItem.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                menuFlyoutItem.Foreground = new SolidColorBrush(Colors.Black);
                menuFlyoutItem.Background = new SolidColorBrush(Colors.White);
            }
        }
        private void UpdateVariableLengthMode(InterpreterParameterStructure variableLength) => ControlHighligter(mnuVariableLength, (bool)variableLength["Defined"]);

        private void UpdateMenuRunningMode(InterpreterParameterStructure quietude)
        {
            if ((bool)quietude["Defined"])
            {
                foreach (var item in from key in new string[] { "mnuQuiet", "mnuVerbose", "mnuVerbosePauseOnExit" }
                                     let items = from item in mnuRunningMode.Items where item.Name == key select item
                                     from item in items
                                     select item)
                {
                    ControlHighligter((MenuFlyoutItem)item!, false);
                }

                switch ((long)quietude["Value"])
                {
                    case 0:
                        ControlHighligter(mnuQuiet, true);
                        break;
                    case 1:
                        ControlHighligter(mnuVerbose, true);
                        break;
                    case 2:
                        ControlHighligter(mnuVerbosePauseOnExit, true);
                        break;
                }
            }
        }

        /// <summary>
        /// Save current editor settings
        /// </summary>
        private void MainWindow_Closed(object sender, object e)
        {
            //CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            if (_richEditBoxes.Count > 0)
            {
                foreach (var _reb in _richEditBoxes)
                {
                    if (_reb.Value.IsDirty)
                    {
                        var key = _reb.Key;
                        var aRichEditBox = _richEditBoxes[key];
                        foreach (var item in tabControl.MenuItems)
                        {
                            var cti = item as CustomTabItem;
                            var content = cti.Content.ToString().Replace(" ", "");
                            if (content == key as string)
                            {
                                Debug.WriteLine(cti.Content);
                                cti.Focus(FocusState.Keyboard); // was Pointer
                            }
                        }
                    }
                }
            }

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["OutputPanelPosition"] = outputPanelPosition.ToString();
            localSettings.Values["OutputHeight"] = outputPanel.Height;
            localSettings.Values["OutputWidth"] = outputPanel.Width;
            localSettings.Values["InterfaceLanguageName"] = InterfaceLanguageName;
            localSettings.Values["InterfaceLanguageID"] = InterfaceLanguageID;
            localSettings.Values["LastSelectedInterpreterLanguageName"] = LastSelectedInterpreterLanguageName;
            localSettings.Values["LastSelectedInterpreterLanguageID"] = LastSelectedInterpreterLanguageID;
            localSettings.Values["LastSelectedVariableLength"] = LastSelectedVariableLength;
            // localSettings.Values["LastSelectedSpaced"] = LastSelectedSpaced;
            localSettings.Values["Engine"] = Engine;
            localSettings.Values["Quietude"] = LastSelectedQuietude;
            localSettings.Values["Scripts"] = Scripts;
            localSettings.Values["Interpreter.P3"] = InterpreterNew;
            localSettings.Values["Interpreter.P2"] = InterpreterOld;
        }

        #region Event Handlers
        private void CreateNewRichEditBox()
        {
            CustomRichEditBox richEditBox = new()
            {
                IsDirty = false,
            };
            richEditBox.KeyDown += RichEditBox_KeyDown;
            richEditBox.AcceptsReturn = true;

            CustomTabItem navigationViewItem = new()
            {
                Content = "Tab " + (tabControl.MenuItems.Count + 1),
                Tag = "Tab" + (tabControl.MenuItems.Count + 1),
                IsNewFile = true,
                TabSettingsDict = Clone(PerTabInterpreterParameters),
                Height = 30
            };
            richEditBox.Tag = navigationViewItem.Tag;
            tabControl.Content = richEditBox;
            _richEditBoxes[richEditBox.Tag] = richEditBox;
            tabControl.MenuItems.Add(navigationViewItem);
            tabControl.SelectedItem = navigationViewItem; // in focus?
            richEditBox.Focus(FocusState.Keyboard);
            UpdateLanguageName(navigationViewItem.TabSettingsDict);
            UpdateTabCommandLine();
        }

        private static InterpreterParametersStructure Clone(InterpreterParametersStructure? perTabInterpreterParameters)
        {
            var clone = new InterpreterParametersStructure();
            foreach (var okey in perTabInterpreterParameters.Keys)
            {
                var inner = new Dictionary<string, object>();
                foreach (var ikey in perTabInterpreterParameters[okey].Keys)
                {
                    inner[ikey] = perTabInterpreterParameters[okey][ikey];
                }
                clone[okey] = inner;
            }
            return clone;
        }

        public string GetTabsLanguageName(InterpreterParametersStructure? tabSettingJson)
        {
            var langValue = InterfaceLanguageID;
            var langName = string.Empty;
            // select from LanguageSettings the record where GLOBAL.ID matches langValue
            var languages = from lang in LanguageSettings.Keys
                            where long.Parse(LanguageSettings[lang]["GLOBAL"]["ID"]) == langValue
                            select LanguageSettings[lang];
            if (languages.Any())
            {
                var first = languages.First();
                var value = (long)tabSettingJson["Language"]["Value"];
                var type = value.GetType().Name;
                long i = 0;
                if (type == "Int32")
                {
                    i = (int)value;
                }

                if (type == "Int64")
                {
                    i = (long)value;
                }

                var nameName = $"{101 + i}";
                langName = first["GLOBAL"][nameName];
            }
            return langName;
        }

        private void UpdateLanguageName(InterpreterParametersStructure? tabSettingJson)
        {
            languageName.Text = GetTabsLanguageName(tabSettingJson);
            LastSelectedInterpreterLanguageID = (long)tabSettingJson["Language"]["Value"];
            LastSelectedInterpreterLanguageName = languageName.Text;
            ApplicationData.Current.LocalSettings.Values["LastSelectedInterpreterLanguageName"] = LastSelectedInterpreterLanguageName;
            ApplicationData.Current.LocalSettings.Values["LastSelectedInterpreterLanguageID"] = LastSelectedInterpreterLanguageID;
        }

        #endregion

        //public static string Reverse(string s)
        //{
        //    char[] charArray = s.ToCharArray();
        //    Array.Reverse(charArray);
        //    return new string(charArray);
        //}

        public void HandleCustomPropertySaving(StorageFile file, CustomRichEditBox customRichEditBox, CustomTabItem navigationViewItem)
        {
            string rtfContent = File.ReadAllText(file.Path);
            StringBuilder rtfBuilder = new(rtfContent);

            var ques = new Regex(Regex.Escape("?"));
            string info = @"\info {\*\ilang ?} {\*\ilength ?} } "; // {\*\ipadout ?}
            info = ques.Replace(info, $"{navigationViewItem.TabSettingsDict["Language"]["Value"]}", 1);
            info = ques.Replace(info, (bool)navigationViewItem.TabSettingsDict["VariableLength"]["Value"] ? "1" : "0", 1);
            // info = ques.Replace(info, (bool)navigationViewItem.TabSettingsDict["Spaced"]["Value"] ? "1" : "0", 1);

            var regex = CustomRTFRegex();

            var matches = regex.Matches(rtfContent);

            var infos = from match in matches where match.Value == @"\info" select match;

            if (infos.Any())
            {
                var fullBlock = rtfContent.Substring(infos.First().Index, infos.First().Length);
                var blockMatches = regex.Matches(fullBlock);
            }
            else
            {
                const string start = @"{\rtf1";
                var i = rtfContent.IndexOf(start);
                var j = i + start.Length;
                rtfBuilder.Insert(j, info);
            }
            File.WriteAllText(file.Path, rtfBuilder.ToString(), Encoding.ASCII);
        }

        public void HandleCustomPropertyLoading(StorageFile file, CustomRichEditBox customRichEditBox, CustomTabItem navigationViewItem)
        {
            string rtfContent = File.ReadAllText(file.Path);
            var regex = CustomRTFRegex();

            var matches = regex.Matches(rtfContent);

            var infos = from match in matches where match.Value.StartsWith(@"\info") select match;
            if (infos.Any())
            {
                var ilang = from match in matches where match.Value.Contains(@"\ilang") select match;
                if (ilang.Any())
                {
                    var items = ilang.First().Value.Split(' ');
                    if (items.Any())
                    {
                        var internalLanguageID = ConvertILangToInternalLanguage(long.Parse(items[1].Replace("}", "")));
                        navigationViewItem.TabSettingsDict["Language"]["Defined"] = true;
                        navigationViewItem.TabSettingsDict["Language"]["Value"] = internalLanguageID;
                    }
                }
                var ilength = from match in matches where match.Value.Contains(@"\ilength") select match;
                if (ilength.Any())
                {
                    var items = ilength.First().Value.Split(' ');
                    if (items.Any())
                    {
                        var flag = items[1].Replace("}", "");
                        if (flag == "0")
                        {
                            navigationViewItem.TabSettingsDict["VariableLength"]["Defined"] = false;
                            navigationViewItem.TabSettingsDict["VariableLength"]["Value"] = flag == "1";
                        }
                        else
                        {
                            navigationViewItem.TabSettingsDict["VariableLength"]["Defined"] = true;
                            navigationViewItem.TabSettingsDict["VariableLength"]["Value"] = flag == "1";
                        }
                    }
                }
            }
            else
            {
                var deflang = from match in matches where match.Value.StartsWith(@"\deflang") select match;
                if (deflang.Any())
                {
                    var deflangId = deflang.First().Value.Replace(@"\deflang", "");
                    var internalLanguageID = ConvertILangToInternalLanguage(long.Parse(deflangId));
                    navigationViewItem.TabSettingsDict["Language"]["Defined"] = true;
                    navigationViewItem.TabSettingsDict["Language"]["Value"] = internalLanguageID;
                }
                else
                {
                    var lang = from match in matches where match.Value.StartsWith(@"\lang") select match;
                    if (lang.Any())
                    {
                        var langId = lang.First().Value.Replace(@"\lang", "");
                        var internalLanguageID = ConvertILangToInternalLanguage(long.Parse(langId));
                        navigationViewItem.TabSettingsDict["Language"]["Defined"] = true;
                        navigationViewItem.TabSettingsDict["Language"]["Value"] = internalLanguageID;
                    }
                    else
                    {
                        navigationViewItem.TabSettingsDict["Language"]["Defined"] = true;
                        navigationViewItem.TabSettingsDict["Language"]["Value"] = (long)0;
                    }
                }
                if (rtfContent.Contains("<# "))
                {
                    navigationViewItem.TabSettingsDict["VariableLength"]["Defined"] = true;
                    navigationViewItem.TabSettingsDict["VariableLength"]["Value"] = rtfContent.Contains("<# ");
                }
            }
        }

        private long ConvertILangToInternalLanguage(long v)
        {
            foreach (string language in LanguageSettings.Keys)
            {
                var global = LanguageSettings[language]["GLOBAL"];
                if (long.Parse(global["ilang"]) == v)
                {
                    return long.Parse(global["ID"]);
                }
                else
                {
                    if (global["ilangAlso"].Split(',').Contains(v.ToString()))
                    {
                        return long.Parse(global["ID"]);
                    }
                }
            }
            return long.Parse(LanguageSettings["English"]["GLOBAL"]["ID"]);
        }

        private static void HandlePossibleAmpersand(string name, MenuFlyoutItemBase mfib)
        {
            if (name.Contains('&'))
            {
                string accel = name.Substring(name.IndexOf("&") + 1, 1);
                mfib.KeyboardAccelerators.Add(new KeyboardAccelerator() { Key = Enum.Parse<VirtualKey>(accel.ToUpperInvariant()), Modifiers = VirtualKeyModifiers.Menu });
                name = name.Replace("&", "");
            }
            switch (mfib.GetType().Name)
            {
                case "MenuFlyoutSubItem":
                    ((MenuFlyoutSubItem)mfib).Text = name;
                    break;
                case "MenuFlyoutItem":
                    ((MenuFlyoutItem)mfib).Text = name;
                    break;
                default:
                    Debugger.Launch();
                    break;
            }
        }

        private static void HandlePossibleAmpersand(string name, MenuBarItem mbi)
        {
            if (name.Contains('&'))
            {
                string accel = name.Substring(name.IndexOf("&") + 1, 1);
                mbi.KeyboardAccelerators.Add(new KeyboardAccelerator() { Key = Enum.Parse<VirtualKey>(accel.ToUpperInvariant()), Modifiers = VirtualKeyModifiers.Menu });
                name = name.Replace("&", "");
            }
            mbi.Title = name;
        }

        private static void HandlePossibleAmpersand(string name, MenuFlyoutItem mfi)
        {
            if (name.Contains('&'))
            {
                string accel = name.Substring(name.IndexOf("&") + 1, 1);
                mfi.KeyboardAccelerators.Add(new KeyboardAccelerator() { Key = Enum.Parse<VirtualKey>(accel.ToUpperInvariant()), Modifiers = VirtualKeyModifiers.Menu });
                name = name.Replace("&", "");
            }
            mfi.Text = name;
        }

        private string BuildTabCommandLine()
        {
            static List<string> BuildWith(InterpreterParametersStructure? interpreterParametersStructure)
            {
                List<string> paras = [];
                if (interpreterParametersStructure != null)
                {
                    foreach (var key in interpreterParametersStructure.Keys)
                    {
                        if ((bool)interpreterParametersStructure[key]["Defined"])
                        {
                            var entry = $"/{interpreterParametersStructure[key]["Key"]}";
                            var value = interpreterParametersStructure[key]["Value"];
                            var type = value.GetType().Name;
                            switch (type)
                            {
                                case "Boolean":
                                    paras.Add(entry);
                                    break;
                                default:
                                    paras.Add($"{entry}:{value}");
                                    break;
                            }
                        }
                    }
                }
                return paras;
            }

            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];

            List<string> paras = [.. BuildWith(GlobalInterpreterParameters), .. BuildWith(navigationViewItem.TabSettingsDict)];

            return string.Join(" ", [.. paras]);
        }

        private void UpdateTabCommandLine()
        {
            tabCommandLine.Text = BuildTabCommandLine();
        }

        [GeneratedRegex("\\{\\*?\\\\[^{}]+}|[{}]|\\\\\\n?[A-Za-z]+\\n?(?:-?\\d+)?[ ]?", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-AU")]
        private static partial Regex CustomRTFRegex();

        private void TabViewItem_Output_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Output Text context menu",
                Content = "right-click menu not enabled",
                CloseButtonText = "OK"
            };
            _ = dialog.ShowAsync();
        }

        private void TabViewItem_Error_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Error Text context menu",
                Content = "right-click menu not enabled",
                CloseButtonText = "OK"
            };
            _ = dialog.ShowAsync();
        }

        private void ContentControl_LanguageName_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //ContentDialog dialog = new()
            //{
            //    XamlRoot = this.XamlRoot,
            //    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            //    Title = "Interpreter language",
            //    Content = "Context menu selection of interpreter language\ncurrently only available from menu",
            //    CloseButtonText = "OK"
            //};
            //_ = dialog.ShowAsync();

            var me = (ContentControl)sender;

            var prevContent = me.Content;

            

            MenuFlyoutSubItem mfsu = new()
            {
                //Text = "»",
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderBrush = new SolidColorBrush() { Color = Colors.LightGray },
                Name = "mnuLanguage"
            };

            var globals = LanguageSettings[LastSelectedInterpreterLanguageName!]["GLOBAL"];
            var count = LanguageSettings.Keys.Count;
            for (var i = 0; i < count; i++)
            {
                var names = from lang in LanguageSettings.Keys
                            where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                            let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                            select name;
                if (names.Any())
                {
                    MenuFlyoutItem menuFlyoutItem = new()
                    {
                        Text = globals[$"{100 + i + 1}"],
                        Name = names.First(),
                        Foreground = names.First() == LastSelectedInterpreterLanguageName ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                        Background = names.First() == LastSelectedInterpreterLanguageName ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                        Tag = new Dictionary<string, object>()
                        {
                            {"MenuFlyoutSubItem",mfsu },
                            {"ContentControlPreviousContent",prevContent },
                            {"ContentControl" ,me}
                        }
                    };
                    menuFlyoutItem.Click += ContentControl_Click; // this has to reset the cell to its original value
                    mfsu.Items.Add(menuFlyoutItem);
                }
            }
            me.Content = mfsu;
        }
    }
}
