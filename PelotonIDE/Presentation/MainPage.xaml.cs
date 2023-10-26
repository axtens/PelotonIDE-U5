using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;

using Newtonsoft.Json;

using System.Diagnostics;
using System.Text;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;

using LanguageConfigurationStructure = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>>;
using InterpreterParametersStructure = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;
using System.Text.RegularExpressions;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        readonly Dictionary<object, CustomRichEditBox> _richEditBoxes = new();
        bool outputPanelShowing = true;
        enum OutputPanelPosition
        {
            Left,
            Bottom,
            Right
        }
        string currentLanguageName = "English";
        int currentLanguageId = 0;
        OutputPanelPosition outputPosition = OutputPanelPosition.Bottom;
        string pelotonEXE = string.Empty;
        string pelotonARG = string.Empty;

        InterpreterParametersStructure? GlobalInterpreterParameters = new();
        InterpreterParametersStructure? PerTabInterpreterParameters = new();
        LanguageConfigurationStructure? LanguageSettings = new();

        public MainPage()
        {

            this.InitializeComponent();

            // GetGlobals();
            CustomRichEditBox customREBox = new()
            {
                Tag = "Tab1",
                //Background = new SolidColorBrush(new Color() { A = 0xFF, R = 0xf9, G = 0xf8, B = 0xbd }),
                //Foreground = new SolidColorBrush(new Color() { A = 0xFF, R = 0xf9, G = 0xf8, B = 0xbd })
            };
            customREBox.KeyDown += RichEditBox_KeyDown;
            customREBox.AcceptsReturn = true;

            // richEditBox.Background = 
            tabControl.Content = customREBox;
            _richEditBoxes[customREBox.Tag] = customREBox;
            tab1.tabSettingJson = null;
            tabControl.SelectedItem = tab1;
            App._window.Closed += MainWindow_Closed;

            FillLanguagesIntoMenu(mnuSettings, "mnuSelectLanguage", Internationalization_Click);
            FillLanguagesIntoMenu(mnuRun, "mnuLanguage", MnuLanguage_Click);

            UpdateTabCommandLine();
        }

        private InterpreterParametersStructure CopyFromGlobalCodeRunCargo()
        {
            InterpreterParametersStructure tsj = new();
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
            Dictionary<string, object> dict = new();
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

        private static async Task<Dictionary<string, object>?> GetGlobalSettings()
        {
            var globalSettings = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\GlobalSettings.json"));
            string globalSettingsString = File.ReadAllText(globalSettings.Path);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(globalSettingsString);
        }

        private async void FillLanguagesIntoMenu(MenuBarItem menuBarItem, string menuLabel, RoutedEventHandler routedEventHandler)
        {
            var tabset = await GetGlobalInterpreterParameters();

            var languageJsonFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\LanguageConfiguration.json"));
            string languageJsonString = File.ReadAllText(languageJsonFile.Path);
            var languageJsonObject = JsonConvert.DeserializeObject<LanguageConfigurationStructure>(languageJsonString);
            LanguageSettings = languageJsonObject;

            var sub = new MenuFlyoutSubItem
            {
                // <!--<MenuFlyoutSubItem Text="Choose interface language" BorderBrush="LightGray" BorderThickness="1" x:Name="SettingsBar_InterfaceLanguage" />-->
                Text = languageJsonObject[currentLanguageName]["frmMain"][menuLabel],
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderBrush = new SolidColorBrush() { Color = Colors.LightGray },
                Name = menuLabel
            };


            //var items = new List<MenuFlyoutItem>();

            // what is current language?
            var globals = LanguageSettings[currentLanguageName]["GLOBAL"];
            var count = LanguageSettings.Keys.Count;
            for (var i = 0; i < count; i++)
            {
                var x = from lang in LanguageSettings.Keys
                        where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                        let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                        select name;
                if (x.Any())
                {
                    MenuFlyoutItem menuFlyoutItem = new()
                    {
                        Text = globals[$"{100 + i + 1}"],
                        Name = x.First()  //languageJson[key]["GLOBAL"]["ID"]
                    };
                    menuFlyoutItem.Click += routedEventHandler; //  Internationalization_Click;
                    sub.Items.Add(menuFlyoutItem);
                }
            }
            menuBarItem.Items.Add(sub);
        }

        /// <summary>
        /// Load previous editor settings
        /// </summary>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            GlobalInterpreterParameters = await MainPage.GetGlobalInterpreterParameters();
            PerTabInterpreterParameters = await MainPage.GetPerTabInterpreterParameters();

            if (tab1.tabSettingJson == null)
                tab1.tabSettingJson = Clone(PerTabInterpreterParameters);

            tab1.tabSettingJson["Language"]["Defined"] = true;
            tab1.tabSettingJson["Language"]["Value"] = currentLanguageId; // FIXME?

            UpdateMenuRunningMode(GlobalInterpreterParameters["Quietude"]);
            UpdateTabCommandLine();

            if ((bool)tab1.tabSettingJson["VariableLength"]["Defined"])
            {
                mnuVariableLength.Icon = (bool)tab1.tabSettingJson["VariableLength"]["Value"] ? tick : null;
            }

            CAPS.Text = Console.CapsLock ? "CAPS" : "caps";
            NUM.Text = Console.NumberLock ? "NUM" : "num";

            var GlobalSettings = await GetGlobalSettings();
            foreach (var setting in GlobalSettings.Keys)
            {
                if (!localSettings.Values.ContainsKey(setting))
                {
                    localSettings.Values[setting] = GlobalSettings[setting];
                }
            }
            foreach (var setting in GlobalInterpreterParameters.Keys)
            {
                if (!localSettings.Values.ContainsKey(setting))
                {
                    localSettings.Values[setting] = GlobalInterpreterParameters[setting]["Value"];
                }
            }

            if (localSettings.Values.ContainsKey("OutputPanelPosition"))
            {
                var opp = localSettings.Values["OutputPanelPosition"];
                outputPosition = (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), (string)opp);
                HandleOutputPanelChange(outputPosition);
            }

            if (localSettings.Values.ContainsKey("OutputHeight"))
            {
                var loh = localSettings.Values["OutputHeight"];
                if (loh != null)
                {
                    var lohType = loh.GetType().Name;
                    if (lohType == "Double")
                    {
                        outputPanel.Height = (double)loh;
                    }
                    else if (lohType == "Int64")
                    {
                        outputPanel.Height = (long)loh;
                    }
                }
            }

            if (localSettings.Values.ContainsKey("OutputWidth"))
            {
                outputPanel.Width = (double)localSettings.Values["OutputWidth"];
            }

            if (localSettings.Values.ContainsKey("Language"))
            {
                var savedLang = localSettings.Values["Language"];
                HandleLanguageChange((string)savedLang);
            }

            if (localSettings.Values.ContainsKey("PelotonEXE"))
            {
                pelotonEXE = (string)localSettings.Values["PelotonEXE"];
            }
        }

        private void UpdateMenuRunningMode(Dictionary<string, object> quietude)
        {
            if ((bool)quietude["Defined"] == true)
            {
                foreach (var item in from key in new string[] { "mnuQuiet", "mnuVerbose", "mnuVerbosePauseOnExit" }
                                     let items = from item in mnuRunningMode.Items where item.Name == key select item
                                     from item in items
                                     select item)
                {
                    (item as MenuFlyoutItem).Icon = null;
                }

                var tick = new FontIcon() // FIXME make global-ish
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Glyph = "\uF0B7"
                };

                switch ((long)quietude["Value"])
                {
                    case 0:
                        mnuQuiet.Icon = tick;
                        break;
                    case 1:
                        mnuVerbose.Icon = tick;
                        break;
                    case 2:
                        mnuVerbosePauseOnExit.Icon = tick;
                        break;
                }
            }
        }

        /// <summary>
        /// Save current editor settings
        /// </summary>
        private void MainWindow_Closed(object sender, object e) //FIXME How do I save to JSON?
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["OutputPanelPosition"] = outputPosition.ToString();
            localSettings.Values["OutputHeight"] = outputPanel.Height;
            localSettings.Values["OutputWidth"] = outputPanel.Width;
            localSettings.Values["Language"] = currentLanguageName; // currentLanguage.ToString();
            localSettings.Values["PelotonEXE"] = pelotonEXE;
            // localSettings.Values["PelotonARG"] = pelotonARG;
        }

        #region Event Handlers


        #region Script Runner

        private void CreateNewRichEditBox()
        {
            CustomRichEditBox richEditBox = new()
            {
                isDirty = false,
            };
            richEditBox.KeyDown += RichEditBox_KeyDown;
            richEditBox.AcceptsReturn = true;

            CustomTabItem navigationViewItem = new()
            {
                Content = "Tab " + (tabControl.MenuItems.Count + 1),
                Tag = "Tab" + (tabControl.MenuItems.Count + 1),
                IsNewFile = true,
                tabSettingJson = Clone(PerTabInterpreterParameters),
                Height = 30
            };
            richEditBox.Tag = navigationViewItem.Tag;
            tabControl.Content = richEditBox;
            _richEditBoxes[richEditBox.Tag] = richEditBox;
            tabControl.MenuItems.Add(navigationViewItem);
            tabControl.SelectedItem = navigationViewItem; // in focus?
            richEditBox.Focus(FocusState.Keyboard);
            //navigationViewItem.tabSettingJson["Language"]["Defined"] = true;
            //navigationViewItem.tabSettingJson["Language"]["Value"] = currentLanguageId;
            //languageName.Text = LanguageSettings[currentLanguageName]["GLOBAL"][$"{101 + currentLanguageId}"];
            UpdateLanguageName(navigationViewItem.tabSettingJson);
            UpdateTabCommandLine();
        }

        private InterpreterParametersStructure Clone(InterpreterParametersStructure? perTabInterpreterParameters)
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
            var langValue = currentLanguageId;
            var langName = string.Empty;
            // select from LanguageSettings the record where GLOBAL.ID matches langValue
            var languages = from lang in LanguageSettings.Keys
                            where int.Parse(LanguageSettings[lang]["GLOBAL"]["ID"]) == langValue
                            select LanguageSettings[lang];
            if (languages.Count() > 0)
            {
                var first = languages.First();
                var value = tabSettingJson["Language"]["Value"];
                var type = value.GetType().Name;
                long i = 0;
                if (type == "Int32")
                {
                    i = (int)value;
                }

                if (type =="Int64")
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
        }

        #endregion

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static void HandleCustomPropertySaving(StorageFile file, CustomRichEditBox customRichEditBox, CustomTabItem navigationViewItem)
        {

            string rtfContent = File.ReadAllText(file.Path);

            var ques = new Regex(Regex.Escape("?"));
            string info = @"\info {\ilang ?} {\ilength ?} {\ipadout ?}}";
            info = ques.Replace(info, (string)navigationViewItem.tabSettingJson["Language"]["Value"], 1);
            info = ques.Replace(info, (string)navigationViewItem.tabSettingJson["VariableLength"]["Value"], 1);
            info = ques.Replace(info, (string)navigationViewItem.tabSettingJson["Spaced"]["Value"], 1);

            var regex = new Regex(@"\{\*?\\[^{}]+}|[{}]|\\\n?[A-Za-z]+\n?(?:-?\d+)?[ ]?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var matches = regex.Matches(rtfContent);

            var infos = from match in matches where match.Value == @"\info" select match;

            if (infos.Any())
            {
                var fullBlock = rtfContent.Substring(infos.First().Index, infos.First().Length);
                var blockMatches = regex.Matches(fullBlock);

            }
            else
            {
            }
            // is there an \info section?
            // yes:
            //  upsert \ilang \ipadout and \ilength
            // write back    

            // string settingsJson = System.Text.Json.JsonSerializer.Serialize(customRichEditBox.TabCodeRunCargo);

            // Manipulate the RTF content
            StringBuilder rtfBuilder = new(rtfContent);

            // Add a \language section
            // rtfBuilder.Insert(rtfBuilder.Length, settingsJson);

            // Write the modified RTF content back to the file
            File.WriteAllText(file.Path, rtfBuilder.ToString());
        }

        public static void HandleCustomPropertyLoading(StorageFile file, CustomRichEditBox customRichEditBox, CustomTabItem navigationViewItem)
        {
            string rtfContent = File.ReadAllText(file.Path);
            var regex = new Regex(@"\{\*?\\[^{}]+}|[{}]|\\\n?[A-Za-z]+\n?(?:-?\d+)?[ ]?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var matches = regex.Matches(rtfContent);

            var infos = from match in matches where match.Value == @"\info" select match;
            if (infos.Any())
            {
                var ilang = from match in matches where match.Value.Contains(@"\ilang") select match;
                if (ilang.Any())
                {
                    var items = ilang.First().Value.Split(' ');
                    if (items.Any())
                    {
                        navigationViewItem.tabSettingJson["Language"]["Defined"] = true;
                        navigationViewItem.tabSettingJson["Language"]["Value"] = int.Parse(items[1].Replace("}",""));
                    }
                }
                var ilength = from match in matches where match.Value.Contains(@"\ilength") select match;
                if (ilength.Any())
                {
                    var items = ilength.First().Value.Split(' ');
                    if (items.Any())
                    {
                        navigationViewItem.tabSettingJson["VariableLength"]["Defined"] = true;
                        navigationViewItem.tabSettingJson["VariableLength"]["Value"] = items[1].Replace("}", "") == "1" ? true : false;
                    }

                }
                var ipadout = from match in matches where match.Value.Contains(@"\ipadout") select match;
                if (ipadout.Any())
                {
                    var items = ipadout.First().Value.Split(' ');
                    if (items.Any())
                    {
                        navigationViewItem.tabSettingJson["Spaced"]["Defined"] = true;
                        navigationViewItem.tabSettingJson["Spaced"]["Value"] = items[1].Replace("}", "") == "1" ? true : false;
                    }
                }

            }
            else
            {
                navigationViewItem.tabSettingJson["Language"]["Defined"] = true;
                navigationViewItem.tabSettingJson["Language"]["Value"] = 0;
                navigationViewItem.tabSettingJson["VariableLength"]["Defined"] = true;
                navigationViewItem.tabSettingJson["VariableLength"]["Value"] = rtfContent.Contains("<# ");
            }

            //int startIndex = modifiedRtfContent.LastIndexOf('{');
            //int endIndex = modifiedRtfContent.Length;
            //string serializedObject = modifiedRtfContent[startIndex..endIndex];

            /*try
            {
                // Deserialize the object from JSON
                if (System.Text.Json.JsonSerializer.Deserialize(serializedObject, typeof(TabSpecificSettings)) is TabSpecificSettings tabSpecificSettings)
                {
                    //customRichEditBox.tabSettings.Setting1 = tabSpecificSettings.Setting1;
                    //customRichEditBox.tabSettings.Setting2 = tabSpecificSettings.Setting2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }*/

        }

        private async void HandleLanguageChange(string langName)
        {
            var selectedLanguage = LanguageSettings[langName];

            SetMenuText(selectedLanguage["frmMain"]);
            var selLang = selectedLanguage["GLOBAL"]["153"];
            currentLanguageName = langName;
            currentLanguageId = int.Parse(selectedLanguage["GLOBAL"]["ID"]);
            languageName.Text = selectedLanguage["GLOBAL"]["101"];
            PerTabInterpreterParameters["Language"]["Defined"] = true;
            PerTabInterpreterParameters["Language"]["Value"] = currentLanguageId;


            // languageName.Document.Selection.SetText(TextSetOptions.None, "Language: " + selLang == langName ? $"{langName}" : $"{langName} - {selLang}");
        }

        private void SetMenuText(Dictionary<string, string> selectedLanguage)
        {
            foreach (var mi in menuBar.Items)
            {
                Debug.WriteLine($"mi {mi.Name}");
                MainPage.HandlePossibleAmpersand(selectedLanguage[mi.Name], mi);

                foreach (var mii in mi.Items)
                {
                    Debug.WriteLine($"mii {mii.Name}");
                    if (selectedLanguage.ContainsKey(mii.Name))
                        MainPage.HandlePossibleAmpersand(selectedLanguage[mii.Name], mii);
                }
            }

            MainPage.HandlePossibleAmpersand(selectedLanguage["mnuQuiet"], mnuQuiet);
            MainPage.HandlePossibleAmpersand(selectedLanguage["mnuVerbose"], mnuVerbose);
            MainPage.HandlePossibleAmpersand(selectedLanguage["mnuVerbosePauseOnExit"], mnuVerbosePauseOnExit);


            ToolTipService.SetToolTip(butNew, selectedLanguage["new.Tip"]);
            ToolTipService.SetToolTip(butOpen, selectedLanguage["open.Tip"]);
            ToolTipService.SetToolTip(butSave, selectedLanguage["save.Tip"]);
            ToolTipService.SetToolTip(butSaveAs, selectedLanguage["save.Tip"]);
            // ToolTipService.SetToolTip(butClose, selectedLanguage["close.Tip"]);
            ToolTipService.SetToolTip(butCopy, selectedLanguage["copy.Tip"]);
            ToolTipService.SetToolTip(butCut, selectedLanguage["cut.Tip"]);
            ToolTipService.SetToolTip(butPaste, selectedLanguage["paste.Tip"]);
            //ToolTipService.SetToolTip(butSelectAll, selectedLanguage["mnuDeselect"]);
            ToolTipService.SetToolTip(butTransform, selectedLanguage["mnuTranslate"]);
            // ToolTipService.SetToolTip(toggleOutputButton, selectedLanguage["mnuToggleOutput"]);
            ToolTipService.SetToolTip(butGo, selectedLanguage["run.Tip"]);
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
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];

            List<string> paras = new();
            foreach (var key in GlobalInterpreterParameters.Keys)
            {
                if ((bool)GlobalInterpreterParameters[key]["Defined"])
                {
                    var entry = $"/{GlobalInterpreterParameters[key]["Key"]}";
                    var value = GlobalInterpreterParameters[key]["Value"];
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

            var inTab = navigationViewItem.tabSettingJson;

            if (inTab != null)
            {
                foreach (var key in inTab.Keys)
                {
                    if ((bool)inTab[key]["Defined"])
                    {
                        var entry = $"/{inTab[key]["Key"]}";
                        var value = inTab[key]["Value"];
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
            return string.Join(" ", paras.ToArray());
        }

        private void UpdateTabCommandLine()
        {
            tabCommandLine.Text = BuildTabCommandLine();
        }

        private void ExecuteInterpreter(string selectedText)
        {
            // load tab settings
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            //foreach (var setting in localSettings.Values)
            //{
            //    if (setting.Key.StartsWith("tab_"))
            //        Debug.WriteLine("{0} => {1}", setting.Key, setting.Value);
            //}

            pelotonARG = BuildTabCommandLine();

            // override with matching tab settings
            // generate arguments string
            (string stdOut, string stdErr) = RunPeloton(pelotonEXE, pelotonARG, selectedText);

            Run run = new();
            Paragraph paragraph = new();
            if (!string.IsNullOrEmpty(stdOut))
            {
                run.Text = stdOut;
                paragraph.Inlines.Add(run);
                outputText.Blocks.Add(paragraph);
            }

            run = new();
            paragraph = new();
            if (!string.IsNullOrEmpty(stdErr))
            {
                run.Text = stdErr;
                paragraph.Inlines.Add(run);
                errorText.Blocks.Add(paragraph);
            }
        }


        public static (string StdOut, string StdErr) RunPeloton(string exe, string args, string buff)
        {
            //var temp = Path.GetTempFileName();
            //File.WriteAllText(temp, buff);
            ProcessStartInfo info = new()
            {
                Arguments = $"{args}",
                FileName = exe,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                // CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var proc = Process.Start(info);
            StringBuilder stdout = new();
            StringBuilder stderr = new();

            StreamWriter stream = proc.StandardInput;
            stream.Write(buff);
            stream.Close();

            proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) => stdout.Append(e.Data);
            proc.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => stderr.Append(e.Data);

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            proc.Dispose();

            return (StdOut: stdout.ToString(), StdErr: stderr.ToString());
        }
        #endregion
    }
}
