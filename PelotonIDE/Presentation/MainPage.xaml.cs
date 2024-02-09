using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;

using Newtonsoft.Json;

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Windows.ApplicationModel.DataTransfer;

using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
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
        [GeneratedRegex("\\{\\*?\\\\[^{}]+}|[{}]|\\\\\\n?[A-Za-z]+\\n?(?:-?\\d+)?[ ]?", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-AU")]
        private static partial Regex CustomRTFRegex();
        private Object rtbLock = new();
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

        string? InterpreterLanguageName;
        long InterpreterLanguageID;

        //bool VariableLength;
        long Quietude = 2;

        OutputPanelPosition outputPanelPosition = OutputPanelPosition.Bottom;
        string? Engine = string.Empty;
        string? Scripts = string.Empty;
        string? InterpreterP2 = string.Empty;
        string? InterpreterP3 = string.Empty;

        int TabControlCounter = 2; // Because the XAML defines the first tab

        InterpreterParametersStructure? PerTabInterpreterParameters = [];

        /// <summary>
        /// does not change
        /// </summary>
        LanguageConfigurationStructure? LanguageSettings;
        FactorySettingsStructure? FactorySettings = [];
        readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        // public LanguageConfigurationStructure? LanguageSettings1 { get => LanguageSettings; set => LanguageSettings = value; }
        List<Plex>? Plexes = GetAllPlexes();

        bool AfterTranslation = false;

        public MainPage()
        {
            this.InitializeComponent();

            CustomRichEditBox customREBox = new()
            {
                Tag = tab1.Tag
            };
            customREBox.KeyDown += RichEditBox_KeyDown;
            customREBox.AcceptsReturn = true;

            tabControl.Content = customREBox;
            _richEditBoxes[customREBox.Tag] = customREBox;
            tab1.TabSettingsDict = null;
            tabControl.SelectedItem = tab1;
            App._window.Closed += MainWindow_Closed;
            UpdateCommandLineInStatusBar();
            customREBox.Document.Selection.SetIndex(TextRangeUnit.Character, 1, false);
        }

        public static async Task<InterpreterParametersStructure?> GetPerTabInterpreterParameters()
        {
            StorageFile tabSettingStorage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\PerTabInterpreterParameters.json"));
            string tabSettings = File.ReadAllText(tabSettingStorage.Path);
            return JsonConvert.DeserializeObject<InterpreterParametersStructure>(tabSettings);
        }

        private static async Task<LanguageConfigurationStructure?> GetLanguageConfiguration()
        {
            StorageFile languageConfig = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\LanguageConfiguration.json"));
            string languageConfigString = File.ReadAllText(languageConfig.Path);
            LanguageConfigurationStructure? languages = JsonConvert.DeserializeObject<LanguageConfigurationStructure>(languageConfigString);
            languages.Remove("Viet");
            return languages;
        }

        private async void InterfaceLanguageSelectionBuilder(MenuFlyoutSubItem menuBarItem, RoutedEventHandler routedEventHandler)
        {
            if (InterfaceLanguageName == null || !LanguageSettings.ContainsKey(InterfaceLanguageName))
            {
                return;
            }

            // what is current language?
            Dictionary<string, string> globals = LanguageSettings[InterfaceLanguageName]["GLOBAL"];
            int count = LanguageSettings.Keys.Count;
            for (int i = 0; i < count; i++)
            {
                IEnumerable<string> names = from lang in LanguageSettings.Keys
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
            LanguageSettings = await GetLanguageConfiguration();

            if (InterfaceLanguageName == null || !LanguageSettings.ContainsKey(InterfaceLanguageName))
            {
                return;
            }

            IEnumerable<MenuFlyoutItemBase> foo = from item in menuBarItem.Items where item.GetType().Name == "MenuFlyoutSubItem" && item.Name == menuLabel select item;
            if (foo.Any())
            {
                return;
            }

            MenuFlyoutSubItem sub = new()
            {
                // <!--<MenuFlyoutSubItem Text="Choose interface language" BorderBrush="LightGray" BorderThickness="1" names:Name="SettingsBar_InterfaceLanguage" />-->
                Text = LanguageSettings[InterfaceLanguageName]["frmMain"][menuLabel],
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderBrush = new SolidColorBrush() { Color = Colors.LightGray },
                Name = menuLabel
            };

            // what is current language?
            Dictionary<string, string> globals = LanguageSettings[InterfaceLanguageName]["GLOBAL"];
            int count = LanguageSettings.Keys.Count;
            for (int i = 0; i < count; i++)
            {
                IEnumerable<string> names = from lang in LanguageSettings.Keys
                                            where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                                            let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                                            select name;
                if (names.Any())
                {
                    MenuFlyoutItem menuFlyoutItem = new()
                    {
                        Text = globals[$"{100 + i + 1}"],
                        Name = names.First(),
                        Foreground = names.First() == InterpreterLanguageName ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                        Background = names.First() == InterpreterLanguageName ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                    };
                    menuFlyoutItem.Click += routedEventHandler;
                    sub.Items.Add(menuFlyoutItem);
                }
            }
            menuBarItem.Items.Add(sub);
        }
        private static void MenuItemHighlightController(MenuFlyoutItem? menuFlyoutItem, bool onish)
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
        // private void ToggleVariableLengthModeInMenu(InterpreterParameterStructure variableLength) => MenuItemHighlightController(mnuVariableLength, (bool)variableLength["Defined"]);
        private void SetVariableLengthModeInMenu(MenuFlyoutItem? menuFlyoutItem, bool showEnabled)
        {
            if (showEnabled)
            {
                menuFlyoutItem.Background = new SolidColorBrush(Colors.Black);
                menuFlyoutItem.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                menuFlyoutItem.Background = new SolidColorBrush(Colors.White);
                menuFlyoutItem.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void ToggleVariableLengthModeInMenu(bool flag) => MenuItemHighlightController(mnuVariableLength, flag);

        private void UpdateMenuRunningModeInMenu(InterpreterParameterStructure quietude)
        {
            if ((bool)quietude["Defined"])
            {
                foreach (MenuFlyoutItemBase? item in from key in new string[] { "mnuQuiet", "mnuVerbose", "mnuVerbosePauseOnExit" }
                                                     let items = from item in mnuRunningMode.Items where item.Name == key select item
                                                     from item in items
                                                     select item)
                {
                    MenuItemHighlightController((MenuFlyoutItem)item!, false);
                }

                switch ((long)quietude["Value"])
                {
                    case 0:
                        MenuItemHighlightController(mnuQuiet, true);
                        break;
                    case 1:
                        MenuItemHighlightController(mnuVerbose, true);
                        break;
                    case 2:
                        MenuItemHighlightController(mnuVerbosePauseOnExit, true);
                        break;
                }
            }
        }

        /// <summary>
        /// Save current editor settings
        /// </summary>
        private void MainWindow_Closed(object sender, object e)
        {
            if (_richEditBoxes.Count > 0)
            {
                foreach (KeyValuePair<object, CustomRichEditBox> _reb in _richEditBoxes)
                {
                    if (_reb.Value.IsDirty)
                    {
                        object key = _reb.Key;
                        CustomRichEditBox aRichEditBox = _richEditBoxes[key];
                        foreach (object? item in tabControl.MenuItems)
                        {
                            CustomTabItem? cti = item as CustomTabItem;
                            string content = cti.Content.ToString().Replace(" ", "");
                            if (content == key as string)
                            {
                                Debug.WriteLine(cti.Content);
                                cti.Focus(FocusState.Keyboard); // was Pointer
                            }
                        }
                    }
                }
            }
        }

        #region Event Handlers
        private static InterpreterParametersStructure ClonePerTabSettings(InterpreterParametersStructure? perTabInterpreterParameters)
        {
            InterpreterParametersStructure clone = [];
            foreach (string outerKey in perTabInterpreterParameters.Keys)
            {
                FactorySettingsStructure inner = [];
                foreach (string innerKey in perTabInterpreterParameters[outerKey].Keys)
                {
                    inner[innerKey] = perTabInterpreterParameters[outerKey][innerKey];
                }
                clone[outerKey] = inner;
            }
            return clone;
        }

        public string GetLanguageNameOfCurrentTab(InterpreterParametersStructure? tabSettingJson)
        {
            long langValue = InterfaceLanguageID;
            string langName = string.Empty;

            IEnumerable<Dictionary<string, Dictionary<string, string>>> languages = from lang in LanguageSettings.Keys
                                                                                    where long.Parse(LanguageSettings[lang]["GLOBAL"]["ID"]) == langValue
                                                                                    select LanguageSettings[lang];

            if (languages.Any())
            {
                Dictionary<string, Dictionary<string, string>> first = languages.First();
                long value = (long)tabSettingJson["Language"]["Value"];
                string type = value.GetType().Name;
                long i = 0;
                if (type == "Int32")
                {
                    i = (int)value;
                }

                if (type == "Int64")
                {
                    i = (long)value;
                }

                string nameName = $"{101 + i}";
                langName = first["GLOBAL"][nameName];
            }
            return langName;
        }

        private void UpdateLanguageNameInStatusBar(InterpreterParametersStructure? tabSettingJson)
        {
            languageName.Text = GetLanguageNameOfCurrentTab(tabSettingJson);
            InterpreterLanguageID = (long)tabSettingJson["Language"]["Value"];
            InterpreterLanguageName = GetLanguageNameFromID(InterpreterLanguageID); // languageName.Text;
            //Type_1_UpdateVirtualRegistry("InterpreterLanguageName", InterpreterLanguageName);
            //Type_1_UpdateVirtualRegistry("InterpreterLanguageID", InterpreterLanguageID);
        }

        private string? GetLanguageNameFromID(long interpreterLanguageID) => (from lang
                                                                              in LanguageSettings
                                                                              where long.Parse(lang.Value["GLOBAL"]["ID"]) == interpreterLanguageID
                                                                              select lang.Value["GLOBAL"]["Name"]).First();

        #endregion

        public void HandleCustomPropertySaving(StorageFile file, CustomTabItem navigationViewItem)
        {
            string rtfContent = File.ReadAllText(file.Path);
            StringBuilder rtfBuilder = new(rtfContent);

            Regex ques = new(Regex.Escape("?"));
            string info = @"{\info {\*\ilang ?} {\*\ilength ?} }"; // {\*\ipadout ?}
            info = ques.Replace(info, $"{navigationViewItem.TabSettingsDict["Language"]["Value"]}", 1);
            info = ques.Replace(info, (bool)navigationViewItem.TabSettingsDict["VariableLength"]["Value"] ? "1" : "0", 1);

            MainPage.Track("info=", info);

            Regex regex = CustomRTFRegex();

            MatchCollection matches = regex.Matches(rtfContent);

            IEnumerable<Match> infos = from match in matches where match.Value == @"\info" select match;

            if (infos.Any())
            {
                string fullBlock = rtfContent.Substring(infos.First().Index, infos.First().Length);
                MatchCollection blockMatches = regex.Matches(fullBlock);
            }
            else
            {
                const string start = @"{\rtf1";
                int i = rtfContent.IndexOf(start);
                int j = i + start.Length;
                rtfBuilder.Insert(j, info);
            }

            MainPage.Track("rtfBuilder=", rtfBuilder.ToString());

            File.WriteAllText(file.Path, rtfBuilder.ToString(), Encoding.ASCII);
        }

        public void HandleCustomPropertyLoading(StorageFile file, CustomRichEditBox customRichEditBox)
        {
            string rtfContent = File.ReadAllText(file.Path);
            Regex regex = CustomRTFRegex();
            string orientation = "00";
            MatchCollection matches = regex.Matches(rtfContent);

            IEnumerable<Match> infos = from match in matches where match.Value.StartsWith(@"\info") select match;
            if (infos.Any())
            {
                IEnumerable<Match> ilang = from match in matches where match.Value.Contains(@"\ilang") select match;
                if (ilang.Any())
                {
                    string[] items = ilang.First().Value.Split(' ');
                    if (items.Any())
                    {
                        (long id, string orientation) internalLanguageIdAndOrientation = ConvertILangToInternalLanguageAndOrientation(long.Parse(items[1].Replace("}", "")));
                        Type_3_UpdateInFocusTabSettings("Language", true, internalLanguageIdAndOrientation.id);
                        orientation = internalLanguageIdAndOrientation.orientation;
                    }
                }
                IEnumerable<Match> ilength = from match in matches where match.Value.Contains(@"\ilength") select match;
                if (ilength.Any())
                {
                    string[] items = ilength.First().Value.Split(' ');
                    if (items.Any())
                    {
                        string flag = items[1].Replace("}", "");
                        if (flag == "0")
                        {
                            Type_3_UpdateInFocusTabSettings("VariableLength", false, false);
                        }
                        else
                        {
                            Type_3_UpdateInFocusTabSettings("VariableLength", true, true);
                        }
                    }
                }
            }
            else
            {
                IEnumerable<Match> deflang = from match in matches where match.Value.StartsWith(@"\deflang") select match;
                if (deflang.Any())
                {
                    string deflangId = deflang.First().Value.Replace(@"\deflang", "");
                    (long id, string orientation) internalLanguageIdAndOrientation = ConvertILangToInternalLanguageAndOrientation(long.Parse(deflangId));
                    Type_3_UpdateInFocusTabSettings("Language", true, internalLanguageIdAndOrientation.id);
                    orientation = internalLanguageIdAndOrientation.orientation;
                }
                else
                {
                    IEnumerable<Match> lang = from match in matches where match.Value.StartsWith(@"\lang") select match;
                    if (lang.Any())
                    {
                        string langId = lang.First().Value.Replace(@"\lang", "");
                        (long id, string orientation) internalLanguageIdAndOrientation = ConvertILangToInternalLanguageAndOrientation(long.Parse(langId));
                        Type_3_UpdateInFocusTabSettings("Language", true, internalLanguageIdAndOrientation.id);
                        orientation = internalLanguageIdAndOrientation.orientation;
                    }
                    else
                    {
                        Type_3_UpdateInFocusTabSettings("Language", true, 0L);
                    }
                }
                if (rtfContent.Contains("<# "))
                {
                    Type_3_UpdateInFocusTabSettings("Language", true, rtfContent.Contains("<# "));
                }
            }
            if (orientation[1] == '1')
            {
                customRichEditBox.FlowDirection = FlowDirection.RightToLeft;
            }
        }

        private (long id, string orientation) ConvertILangToInternalLanguageAndOrientation(long v)
        {
            foreach (string language in LanguageSettings.Keys)
            {
                Dictionary<string, string> global = LanguageSettings[language]["GLOBAL"];
                if (long.Parse(global["ID"]) == v)
                {
                    return (long.Parse(global["ID"]), global["TextOrientation"]);
                }
                else
                {
                    if (global["ilangAlso"].Split(',').Contains(v.ToString()))
                    {
                        return (long.Parse(global["ID"]), global["TextOrientation"]);
                    }
                }
            }
            return (long.Parse(LanguageSettings["English"]["GLOBAL"]["ID"]), LanguageSettings["English"]["GLOBAL"]["TextOrientation"]);
        }

        private static void HandlePossibleAmpersandInMenuItem(string name, MenuFlyoutItemBase mfib)
        {
            if (name.Contains('&'))
            {
                string accel = name.Substring(name.IndexOf("&") + 1, 1);
                mfib.KeyboardAccelerators.Add(new KeyboardAccelerator()
                {
                    Key = Enum.Parse<VirtualKey>(accel.ToUpperInvariant()),
                    Modifiers = VirtualKeyModifiers.Menu
                });
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

        private static void HandlePossibleAmpersandInMenuItem(string name, MenuBarItem mbi)
        {
            if (name.Contains('&'))
            {
                string accel = name.Substring(name.IndexOf("&") + 1, 1);
                mbi.KeyboardAccelerators.Add(new KeyboardAccelerator()
                {
                    Key = Enum.Parse<VirtualKey>(accel.ToUpperInvariant()),
                    Modifiers = VirtualKeyModifiers.Menu
                });
                name = name.Replace("&", "");
            }
            mbi.Title = name;
        }

        private static void HandlePossibleAmpersandInMenuItem(string name, MenuFlyoutItem mfi)
        {
            if (name.Contains('&'))
            {
                string accel = name.Substring(name.IndexOf("&") + 1, 1);
                mfi.KeyboardAccelerators.Add(new KeyboardAccelerator()
                {
                    Key = Enum.Parse<VirtualKey>(accel.ToUpperInvariant()),
                    Modifiers = VirtualKeyModifiers.Menu
                });
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
                    foreach (string key in interpreterParametersStructure.Keys)
                    {
                        if ((bool)interpreterParametersStructure[key]["Defined"])
                        {
                            string entry = $"/{interpreterParametersStructure[key]["Key"]}";
                            object value = interpreterParametersStructure[key]["Value"];
                            string type = value.GetType().Name;
                            switch (type)
                            {
                                case "Boolean":
                                    if ((bool)value) paras.Add(entry);
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
            List<string> paras = [];
            if (navigationViewItem != null)
                paras = [.. BuildWith(navigationViewItem.TabSettingsDict)];

            return string.Join<string>(" ", [.. paras]);
        }

        private void UpdateCommandLineInStatusBar()
        {
            tabCommandLine.Text = BuildTabCommandLine(); ;
        }


        #region Getters
        private object Type_1_GetVirtualRegistry(string name)
        {
            object result = ApplicationData.Current.LocalSettings.Values[name];
            Track(name, result);
            return result;
        }

        private T Type_1_GetVirtualRegistry<T>(string name)
        {
            object result = ApplicationData.Current.LocalSettings.Values[name];
            Track(name, result);
            return (T)result;
        }

        private bool Type_1_GetVirtualRegistry_Boolean(string name)
        {
            object result = ApplicationData.Current.LocalSettings.Values[name];
            Track(name, result);
            return (bool)result;
        }
        #endregion

        #region Setters

        // 1. virt reg
        private void Type_1_UpdateVirtualRegistry<T>(string name, T value)
        {
            Track(name, value);
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }

        // 2. pertab
        private void Type_2_UpdatePerTabSettings<T>(string name, bool enabled, T value)
        {
            Track(name, enabled, value);
            PerTabInterpreterParameters[name]["Defined"] = enabled;
            PerTabInterpreterParameters[name]["Value"] = value!;
        }

        // 3. currtab
        private void Type_3_UpdateInFocusTabSettings<T>(string name, bool enabled, T value)
        {
            Track(name, enabled, value);
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            navigationViewItem.TabSettingsDict[name]["Defined"] = enabled;
            navigationViewItem.TabSettingsDict[name]["Value"] = value!;
        }
        #endregion
    }
}
