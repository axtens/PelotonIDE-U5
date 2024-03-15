using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Input;

using Newtonsoft.Json;

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Windows.Storage;
using Windows.System;

using Colors = Microsoft.UI.Colors;
using FactorySettingsStructure = System.Collections.Generic.Dictionary<string, object>;
using InterpreterParametersStructure = System.Collections.Generic.Dictionary<string,
    System.Collections.Generic.Dictionary<string, object>>;
using InterpreterParameterStructure = System.Collections.Generic.Dictionary<string, object>;
using LanguageConfigurationStructure = System.Collections.Generic.Dictionary<string,
    System.Collections.Generic.Dictionary<string,
        System.Collections.Generic.Dictionary<string, string>>>;
using RenderingConstantsStructure = System.Collections.Generic.Dictionary<string,
        System.Collections.Generic.Dictionary<string, object>>;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Microsoft.UI.Xaml.Controls.Page
    {
        [GeneratedRegex("\\{\\*?\\\\[^{}]+}|[{}]|\\\\\\n?[A-Za-z]+\\n?(?:-?\\d+)?[ ]?", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-AU")]
        private static partial Regex CustomRTFRegex();
        readonly Dictionary<object, CustomRichEditBox> _richEditBoxes = [];
        // bool outputPanelShowing = true;
        enum OutputPanelPosition
        {
            Left,
            Bottom,
            Right
        }
        //string? InterfaceLanguageName = "English";
        //long InterfaceLanguageID = 0;

        //string? InterpreterLanguageName;
        //long InterpreterLanguageID;

        //bool VariableLength;
        //long Quietude = 2;

        //OutputPanelPosition outputPanelPosition = OutputPanelPosition.Bottom;
        string? Engine = string.Empty;
        string? Scripts = string.Empty;
        string? InterpreterP2 = string.Empty;
        string? InterpreterP3 = string.Empty;

        int TabControlCounter = 2; // Because the XAML defines the first tab

        InterpreterParametersStructure? PerTabInterpreterParameters;
        RenderingConstantsStructure? RenderingConstants = null;

        /// <summary>
        /// does not change
        /// </summary>
        LanguageConfigurationStructure? LanguageSettings;
        FactorySettingsStructure? FactorySettings;
        readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        // public LanguageConfigurationStructure? LanguageSettings1 { get => LanguageSettings; set => LanguageSettings = value; }
        readonly List<Plex>? Plexes = GetAllPlexes();

        Dictionary<string, List<string>> LangLangs = [];

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


        /*
        public static async Task<RenderingConstantsStructure?> GetJSONRenderingConstants()
        {
            StorageFile renderingConstantsStorage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\RenderingConstants.json")); // \Presentation\RenderingConstants.json
            string renderingConstantsJson = File.ReadAllText(renderingConstantsStorage.Path);
            return JsonConvert.DeserializeObject<RenderingConstantsStructure>(renderingConstantsJson);
        }
        */
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
            string interfaceLanguageName = Type_1_GetVirtualRegistry<string>("InterfaceLanguageName");
            if (interfaceLanguageName == null || !LanguageSettings.ContainsKey(interfaceLanguageName))
            {
                return;
            }

            menuBarItem.Items.Clear();

            // what is current language?
            Dictionary<string, string> globals = LanguageSettings[interfaceLanguageName]["GLOBAL"];
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
                        Foreground = names.First() == Type_1_GetVirtualRegistry<string>("InterfaceLanguageName") ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                        Background = names.First() == Type_1_GetVirtualRegistry<string>("InterfaceLanguageName") ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                    };
                    menuFlyoutItem.Click += routedEventHandler; //  Internationalization_Click;
                    menuBarItem.Items.Add(menuFlyoutItem);
                }
            }
        }

        private async void InterpreterLanguageSelectionBuilder(MenuBarItem menuBarItem, string menuLabel, RoutedEventHandler routedEventHandler)
        {
            Telemetry telem = new();
            telem.SetEnabled(false);

            LanguageSettings ??= await GetLanguageConfiguration();
            string interfaceLanguageName = Type_1_GetVirtualRegistry<string>("InterfaceLanguageName");

            if (interfaceLanguageName == null || !LanguageSettings.ContainsKey(interfaceLanguageName))
            {
                return;
            }

            menuBarItem.Items.Remove(item => item.Name == menuLabel && item.GetType().Name == "MenuFlyoutSubItem");

            MenuFlyoutSubItem sub = new()
            {
                // <!--<MenuFlyoutSubItem Text="Choose interface language" BorderBrush="LightGray" BorderThickness="1" names:Name="SettingsBar_InterfaceLanguage" />-->
                Text = LanguageSettings[interfaceLanguageName]["frmMain"][menuLabel],
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderBrush = new SolidColorBrush() { Color = Colors.LightGray },
                Name = menuLabel
            };

            // what is current language?
            Dictionary<string, string> globals = LanguageSettings[interfaceLanguageName]["GLOBAL"];
            int count = LanguageSettings.Keys.Count;
            for (int i = 0; i < count; i++)
            {
                IEnumerable<string> names = from lang in LanguageSettings.Keys
                                            where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                                            let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                                            select name;

                telem.Transmit("names.Any=", names.Any());

                if (names.Any())
                {
                    MenuFlyoutItem menuFlyoutItem = new()
                    {
                        Text = globals[$"{100 + i + 1}"],
                        Name = names.First(),
                        Foreground = names.First() == Type_1_GetVirtualRegistry<string>("InterpreterLanguageName") ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                        Background = names.First() == Type_1_GetVirtualRegistry<string>("InterpreterLanguageName") ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                    };
                    menuFlyoutItem.Click += routedEventHandler;
                    sub.Items.Add(menuFlyoutItem);
                }
            }
            menuBarItem.Items.Add(sub);
        }
        private static void MenuItemHighlightController(MenuFlyoutItem? menuFlyoutItem, bool onish)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            telem.Transmit("menuFlyoutItem.Name=", menuFlyoutItem.Name, "onish=", onish);
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
            Telemetry telem = new();
            telem.SetEnabled(true);
            telem.Transmit("menuFlyoutItem.Name=", menuFlyoutItem.Name, "showEnabled=", showEnabled);
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

        private void UpdateTimeoutInMenu()
        {
            foreach (var item in mnuTimeout.Items)
            {
                MenuItemHighlightController((MenuFlyoutItem)item!, false);
            }
            long currTimeout = Type_1_GetVirtualRegistry<long>("Timeout");

            switch (currTimeout)
            {
                case 0:
                    MenuItemHighlightController(mnu20Seconds, true);
                    break;

                case 1:
                    MenuItemHighlightController(mnu100Seconds, true);
                    break;

                case 2:
                    MenuItemHighlightController(mnu200Seconds, true);
                    break;

                case 3:
                    MenuItemHighlightController(mnu1000Seconds, true);
                    break;

                case 4:
                    MenuItemHighlightController(mnuInfinite, true);
                    break;

            }
        }
        private void UpdateMenuRunningModeInMenu(InterpreterParameterStructure quietude)
        {
            if ((bool)quietude["Defined"])
            {
                mnuRunningMode.Items.ForEach(item =>
                {
                    MenuItemHighlightController((MenuFlyoutItem)item, false);
                    if ((long)quietude["Value"] == long.Parse((string)item.Tag))
                    {
                        MenuItemHighlightController((MenuFlyoutItem)item, true);
                    }
                });
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
            Telemetry telem = new();
            telem.SetEnabled(true);

            long langValue;
            string langName;
            if (AnInFocusTabExists())
            {
                langValue = Type_3_GetInFocusTab<long>("Language");
                langName = LanguageSettings[Type_1_GetVirtualRegistry<string>("InterfaceLanguageName")]["GLOBAL"][$"{101 + langValue}"];
            }
            else
            {
                langValue = Type_2_GetPerTabSettings<long>("Language");
                langName = LanguageSettings[Type_1_GetVirtualRegistry<string>("InterfaceLanguageName")]["GLOBAL"][$"{101 + langValue}"];
            }
            telem.Transmit("langValue=", langValue, "langName=", langName);
            return langName;
        }

        private void UpdateLanguageNameInStatusBar(InterpreterParametersStructure? tabSettingJson)
        {
            languageName.Text = GetLanguageNameOfCurrentTab(tabSettingJson);
        }

        private string? GetLanguageNameFromID(long interpreterLanguageID) => (from lang
                                                                              in LanguageSettings
                                                                              where long.Parse(lang.Value["GLOBAL"]["ID"]) == interpreterLanguageID
                                                                              select lang.Value["GLOBAL"]["Name"]).First();

        #endregion

        public void HandleCustomPropertySaving(StorageFile file, CustomTabItem navigationViewItem)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            string rtfContent = File.ReadAllText(file.Path);
            StringBuilder rtfBuilder = new(rtfContent);

            const int ONCE = 1;

            var inFocusTab = navigationViewItem.TabSettingsDict;
            Regex ques = new(Regex.Escape("?"));
            string info = @"{\info {\*\ilang ?} {\*\ilength ?} {\*\itimeout ?} {\*\iquietude ?} {\*\itransput ?} }"; // {\*\ipadout ?}
            info = ques.Replace(info, $"{inFocusTab["Language"]["Value"]}", ONCE);
            info = ques.Replace(info, (bool)inFocusTab["VariableLength"]["Value"] ? "1" : "0", ONCE);
            info = ques.Replace(info, $"{(long)inFocusTab["Timeout"]["Value"]}", ONCE);
            info = ques.Replace(info, $"{(long)inFocusTab["Quietude"]["Value"]}", ONCE);
            info = ques.Replace(info, $"{(long)inFocusTab["Transput"]["Value"]}", ONCE);

            telem.Transmit("info=", info);

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

            telem.Transmit("rtfBuilder=", rtfBuilder.ToString());

            string? text = rtfBuilder.ToString();
            if (text.EndsWith((char)0x0)) text = text.Remove(text.Length - 1);
            while (text.LastIndexOf("\\par\r\n}") > -1) text = text.Remove(text.LastIndexOf("\\par\r\n}"), 6);
            File.WriteAllText(file.Path, text, Encoding.ASCII);
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

                MarkupToInFocusSetting(matches, @"\itimeout", "Timeout");
                MarkupToInFocusSetting(matches, @"\iquietude", "Quietude");
                MarkupToInFocusSetting(matches, @"\itransput", "Transput");
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

        private void MarkupToInFocusSetting(MatchCollection matches, string markup, string setting)
        {
            IEnumerable<Match> itransput = from match in matches where match.Value.Contains(markup) select match;
            if (itransput.Any())
            {
                string[] transputs = itransput.First().Value.Split(' ');
                if (transputs.Any())
                {
                    string num = transputs[1].Replace("}", "");
                    Type_3_UpdateInFocusTabSettings<long>(setting, true, long.Parse(num));
                }
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
            return (long.Parse(LanguageSettings["English"]["GLOBAL"]["ID"]), LanguageSettings["English"]["GLOBAL"]["TextOrientation"]); // default
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
                try
                {
                    mbi.KeyboardAccelerators.Add(new KeyboardAccelerator()
                    {
                        Key = Enum.Parse<VirtualKey>(accel.ToUpperInvariant()),
                        Modifiers = VirtualKeyModifiers.Menu
                    });
                }
                catch (Exception ex)
                {
                    Telemetry telem = new();
                    telem.SetEnabled(true);
                    telem.Transmit(ex.Message, accel);
                }
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
                        if ((bool)interpreterParametersStructure[key]["Defined"] && !(bool)interpreterParametersStructure[key]["Internal"])
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
            tabCommandLine.Text = BuildTabCommandLine();
        }


        private void FormatMenu_FontSize_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            var me = (MenuFlyoutItem)sender;
            telem.Transmit(me.Name);

            CustomRichEditBox currentRichEditBox = _richEditBoxes[((CustomTabItem)tabControl.SelectedItem).Tag];

            currentRichEditBox.Document.Selection.CharacterFormat.Size = long.Parse((string)me.Tag);
            currentRichEditBox.Document.Selection.SelectOrDefault(x => x);
        }

        private void TabViewItem_Html_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);
        }

        private void FileCopyHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);
        }

        private void ClipboardCopyHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);
        }

        private void ClearHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);
        }

        private void TabViewItem_Html_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);
        }

        private void TabViewItem_Error_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);
        }

        private void TabViewItem_Output_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);
        }

        private void TabViewItem_Logo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);

        }

        private void TabViewItem_Logo_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (TabViewItem)sender;
            telem.Transmit(me.Name);

        }

        private void FileCopyLogoButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (MenuFlyoutItem)sender;
            telem.Transmit(me.Name);

        }

        private void ClipboardCopyLogoButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (MenuFlyoutItem)sender;
            telem.Transmit(me.Name);

        }

        private void ClearLogoButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            var me = (MenuFlyoutItem)sender;
            telem.Transmit(me.Name);
        }

        private void ContentControl_Rendering_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            ContentControl me = (ContentControl)sender;

            object prevContent = me.Content;

            MenuFlyout mf = new();

            string interfaceLanguageName = Type_1_GetVirtualRegistry<string>("InterfaceLanguageName");

            if (!AnInFocusTabExists()) return;

            string? inFocusRendering = Type_3_GetInFocusTab<string>("Rendering");
            telem.Transmit("inFocusRendering=", inFocusRendering);

            Dictionary<string, string> frmMain = LanguageSettings[interfaceLanguageName]["frmMain"];

            foreach (string key in new string[] { "Output", "Error", "Html", "Logo", "RTF" })
            {
                long renderNumber = (long)RenderingConstants["Rendering"][key];
                MenuFlyoutItem menuFlyoutItem = new()
                {
                    Name = key,
                    Text = frmMain[$"tab{key}"],
                    Foreground = inFocusRendering.Contains(renderNumber.ToString()) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                    Background = inFocusRendering.Contains(renderNumber.ToString()) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                    Tag = new Dictionary<string, object>()
                        {
                            { "Globals", frmMain },
                            { "CurrentValue", inFocusRendering }
                        }
                };
                menuFlyoutItem.Click += ContentControl_Rendering_MenuFlyoutItem_Click; // this has to reset the cell to its original value
                telem.Transmit(menuFlyoutItem.Text, menuFlyoutItem.Name, menuFlyoutItem.Foreground.ToString(), menuFlyoutItem.Background.ToString());
                mf.Items.Add(menuFlyoutItem);
            }

            FrameworkElement? senderElement = sender as FrameworkElement;

            mf.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private void ContentControl_Rendering_MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            MenuFlyoutItem me = (MenuFlyoutItem)sender;
            string meName = me.Name.Replace("tab", "");

            string render = ((long)RenderingConstants["Rendering"][meName]).ToString();

            if (AnInFocusTabExists())
            {
                List<string> keys = [.. Type_3_GetInFocusTab<string>("Rendering").Split(',')];
                if (keys.Contains(render))
                {
                    keys.Remove(render);
                }
                else
                {
                    keys.Add(render);
                }
                Type_3_UpdateInFocusTabSettings<string>("Rendering", true, string.Join(",", keys));
            }
            UpdateCommandLineInStatusBar();
        }

        private void TabViewItem_RTF_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void FileCopyRTFButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClipboardCopyRTFButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearRTFButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabViewItem_RTF_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void InterpretMenu_Transput_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            MenuFlyoutItem me = (MenuFlyoutItem)sender;
            foreach (var mfi in from MenuFlyoutSubItem mfsi in mnuTransput.Items.Cast<MenuFlyoutSubItem>()
                                where mfsi != null
                                where mfsi.Items.Count > 0
                                from MenuFlyoutItem mfi in mfsi.Items.Cast<MenuFlyoutItem>()
                                select mfi)
            {
                MenuItemHighlightController((MenuFlyoutItem)mfi, false);
                if ((string)me.Tag == (string)mfi.Tag)
                {
                    MenuItemHighlightController((MenuFlyoutItem)mfi, true);
                }
            }
            Type_2_UpdatePerTabSettings("Transput", true, long.Parse((string)me.Tag));
            Type_3_UpdateInFocusTabSettings("Transput", true, long.Parse((string)me.Tag));
            UpdateCommandLineInStatusBar();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            MenuFlyoutItem me = (MenuFlyoutItem)sender;
            telem.Transmit(me.Name);

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                Verb = "open",
                FileName = @"c:\protium\bin\help\protium.chm",
                WindowStyle = ProcessWindowStyle.Normal
            };
            // startInfo.Verbs.ToList().ForEach(V => telem.Transmit(V));
            Process.Start(startInfo);
            
        }
    }
}
