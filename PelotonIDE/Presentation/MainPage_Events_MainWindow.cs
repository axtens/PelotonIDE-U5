using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.UI;
using Microsoft.UI.Text;

using System.ComponentModel;
using System.Timers;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        public void TimerTick(object? source, ElapsedEventArgs e)
        {
            TIME.Text = DateTime.Now.ToString("HH':'mm':'ss");
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                return;
            }

            NavigationData parameters = (NavigationData)e.Parameter;

            //var selectedLanguage = parameters.selectedLangauge;
            //var translatedREB = parameters.translatedREB;
            switch (parameters.Source)
            {
                case "IDEConfig":
                    //string? engine = LocalSettings.Values["Engine"].ToString();
                    Type_1_UpdateVirtualRegistry("Interpreter.P3", parameters.KVPs["Interpreter"].ToString());
                    Type_1_UpdateVirtualRegistry("Scripts", parameters.KVPs["Scripts"].ToString());
                    break;
                case "TranslatePage":
                    CustomRichEditBox richEditBox = new()
                    {
                        IsDirty = true,
                        IsRTF = true,
                    };
                    richEditBox.KeyDown += RichEditBox_KeyDown;
                    richEditBox.AcceptsReturn = true;
                    richEditBox.Document.SetText(TextSetOptions.UnicodeBidi, parameters.KVPs["TargetText"].ToString());

                    string? langname = LocalSettings.Values["InterfaceLanguageName"].ToString();
                    long quietude = (long)parameters.KVPs["Quietude"];
                    Type_2_UpdatePerTabSettings("Quietude", true, quietude);

                    CustomTabItem navigationViewItem = new()
                    {
                        Content = LanguageSettings[langname!]["GLOBAL"]["Document"] + " " + TabControlCounter, // (tabControl.MenuItems.Count + 1),
                        //Content = "Tab " + (tabControl.MenuItems.Count + 1),
                        Tag = "Tab" + TabControlCounter, // (tabControl.MenuItems.Count + 1),
                        IsNewFile = true,
                        TabSettingsDict = ClonePerTabSettings(PerTabInterpreterParameters),
                        Height = 30,
                    };

                    TabControlCounter += 1;

                    richEditBox.Tag = navigationViewItem.Tag;

                    _richEditBoxes[richEditBox.Tag] = richEditBox;
                    tabControl.MenuItems.Add(navigationViewItem);
                    tabControl.SelectedItem = navigationViewItem;

                    Type_3_UpdateInFocusTabSettings("Language", true, (long)parameters.KVPs["TargetLanguageID"]);
                    if (parameters.KVPs.TryGetValue("TargetVariableLength", out object? value))
                    {
                        Type_3_UpdateInFocusTabSettings("VariableLength", (bool)value, (bool)value);
                    }

                    richEditBox.Focus(FocusState.Keyboard);
                    languageName.Text = null;
                    languageName.Text = GetLanguageNameOfCurrentTab(navigationViewItem.TabSettingsDict);
                    tabCommandLine.Text = BuildTabCommandLine();

                    AfterTranslation = true;

                    break;
            }
        }
        /// <summary>
        /// Load previous editor settings
        /// </summary>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*    
                        string? langname = LocalSettings.Values["InterfaceLanguageName"].ToString();
                        tab1.Content = LanguageSettings[langname!]["GLOBAL"]["Document"] + " 1";
             */

            LanguageSettings ??= await GetLanguageConfiguration();

            SetKeyboardFlags();

            FactorySettings ??= await GetFactorySettings();

            IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<bool>("OutputPanelShowing", FactorySettings, true);
            IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<OutputPanelPosition>("OutputPanelPosition", FactorySettings, (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), "Bottom"));
            IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<long>("OutputPanelHeight", FactorySettings, 200);
            HandleOutputPanelChange(Type_1_GetVirtualRegistry<string>("OutputPanelPosition"));

            outputPanel.Height = Type_1_GetVirtualRegistry<double>("OutputPanelHeight");

            IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<string>("InterfaceLanguageName", FactorySettings, "English");
            IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<long>("InterfaceLanguageID", FactorySettings, 0);
            IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<string>("InterpreterLanguageName", FactorySettings, "English");
            IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<long>("InterpreterLanguageID", FactorySettings, 0);

            if (Type_1_GetVirtualRegistry<string>("InterfaceLanguageName") != null)
            {
                HandleInterfaceLanguageChange(Type_1_GetVirtualRegistry<string>("InterfaceLanguageName"));
            }

            // Engine selection:
            //  Engine will contain either "Interpreter.P2" or "Interpreter.P3"
            //  if Engine is present in LocalSettings, use that value, otherwise retrieve it from FactorySettings and update local settings
            //  if Engine is null (for some reason FactorySettings is broken), use "Interpreter.P3"

            SetEngine();
            SetScripts();
            SetInterpreterNew();
            SetInterpreterOld();

            PerTabInterpreterParameters = await MainPage.GetPerTabInterpreterParameters();

            if (!AfterTranslation)
            {

                //bool VariableLength = GetFactorySettingsWithLocalSettingsOverrideOrDefault<bool>("VariableLength", FactorySettings, LocalSettings, false);
                IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<bool>("VariableLength", FactorySettings, false);
                //Type_1_UpdateVirtualRegistry("VariableLength", VariableLength);
                //long Quietude = GetFactorySettingsWithLocalSettingsOverrideOrDefault<long>("Quietude", FactorySettings, LocalSettings, 2);
                IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<long>("Quietude", FactorySettings, 2);
                //Type_1_UpdateVirtualRegistry("Quietude", Quietude);

                Type_2_UpdatePerTabSettings("Language", true,Type_1_GetVirtualRegistry<long>("InterpreterLanguageID"));
                Type_2_UpdatePerTabSettings("VariableLength", Type_1_GetVirtualRegistry<bool>("VariableLength"), Type_1_GetVirtualRegistry<bool>("VariableLength"));
                Type_2_UpdatePerTabSettings("Quietude", true, Type_1_GetVirtualRegistry<long>("Quietude"));
            }

            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            navigationViewItem.TabSettingsDict ??= ClonePerTabSettings(PerTabInterpreterParameters);

            UpdateTabDocumentNameIfOnlyOneAndFirst(tabControl, Type_1_GetVirtualRegistry<string>("InterfaceLanguageName"));

            if (!AfterTranslation)
            {
                Type_3_UpdateInFocusTabSettings("Language", true, Type_1_GetVirtualRegistry<long>("InterpreterLanguageID"));
                // Do we also set the VariableLength of the inFocusTab?
                //bool VariableLength = GetFactorySettingsWithLocalSettingsOverrideOrDefault<bool>("VariableLength", FactorySettings, LocalSettings, false);
                IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault("VariableLength", FactorySettings, false);
                Type_3_UpdateInFocusTabSettings("VariableLength", Type_1_GetVirtualRegistry<bool>("VariableLength"), Type_1_GetVirtualRegistry<bool>("VariableLength"));
            }
            InterfaceLanguageSelectionBuilder(mnuSelectLanguage, Internationalization_Click);
            InterpreterLanguageSelectionBuilder(mnuRun, "mnuLanguage", MnuLanguage_Click);
            UpdateEngineSelectionFromFactorySettingsInMenu();

            if (!AfterTranslation)
                UpdateMenuRunningModeInMenu(PerTabInterpreterParameters["Quietude"]);

            AfterTranslation = false;

            SetVariableLengthModeInMenu(mnuVariableLength, Type_1_GetVirtualRegistry<bool>("VariableLength"));
            UpdateLanguageNameInStatusBar(navigationViewItem.TabSettingsDict);

            UpdateCommandLineInStatusBar();

            void SetKeyboardFlags()
            {
                CAPS.Foreground = Console.CapsLock ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);
                NUM.Foreground = Console.NumberLock ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);
            }

            void SetEngine()
            {
                if (LocalSettings.Values.TryGetValue("Engine", out object? value))
                {
                    Engine = value.ToString();
                }
                else
                {
                    Engine = FactorySettings["Engine"].ToString();
                }
                Engine ??= "Interpreter.P3";
                Type_1_UpdateVirtualRegistry("Engine", Engine);
            }
            void SetScripts()
            {
                if (LocalSettings.Values.TryGetValue("Scripts", out object? value))
                {
                    Scripts = value.ToString();
                }
                else
                {
                    Scripts = FactorySettings["Scripts"].ToString();
                }
                Scripts ??= @"C:\peloton\code";
                Type_1_UpdateVirtualRegistry("Scripts", Scripts);
            }
            void SetInterpreterOld()
            {
                if (LocalSettings.Values.TryGetValue("Interpreter.P2", out object? value))
                {
                    InterpreterP2 = value.ToString();
                }
                else
                {
                    InterpreterP2 = FactorySettings["Interpreter.P2"].ToString();
                }
                InterpreterP2 ??= @"c:\protium\bin\pdb.exe";
                Type_1_UpdateVirtualRegistry("Interpreter.P2", InterpreterP2);
            }
            void SetInterpreterNew()
            {
                if (LocalSettings.Values.TryGetValue("Interpreter.P3", out object? value))
                {
                    InterpreterP3 = value.ToString();
                }
                else
                {
                    InterpreterP3 = FactorySettings["Interpreter.P3"].ToString();
                }
                InterpreterP3 ??= @"c:\peloton\bin\p3.exe";
                Type_1_UpdateVirtualRegistry("Interpreter.P3", InterpreterP3);
            }
            (tabControl.Content as CustomRichEditBox).Focus(FocusState.Keyboard);

            string currentLanguageName = GetLanguageNameOfCurrentTab(navigationViewItem.TabSettingsDict);
            if (languageName.Text != currentLanguageName)
            {
                languageName.Text = currentLanguageName;
            }
        }

        private void IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<T>(string name, Dictionary<string, object>? factory, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(name)) return;
            if (factory.TryGetValue(name, out object? factoryValue))
            {
                LocalSettings.Values[name] = (T)factoryValue;
                return;
            }
            LocalSettings.Values[name] = (defaultValue.GetType().BaseType.Name == "Enum") ? defaultValue.ToString() : defaultValue;
            return;
        }

        private void UpdateTabDocumentNameIfOnlyOneAndFirst(NavigationView tabControl, string? interfaceLanguageName)
        {
            if (tabControl.MenuItems.Count == 1 && interfaceLanguageName != null && interfaceLanguageName != "English")
            {
                string? content = (string?)((CustomTabItem)tabControl.SelectedItem).Content;
                content = content.Replace(LanguageSettings["English"]["GLOBAL"]["Document"], LanguageSettings[interfaceLanguageName]["GLOBAL"]["Document"]);
                ((CustomTabItem)tabControl.SelectedItem).Content = content;
            }
        }

        private void UpdateEngineSelectionFromFactorySettingsInMenu()
        {
            if (LocalSettings.Values["Engine"].ToString() == "Interpreter.P2")
            {
                MenuItemHighlightController(mnuNewEngine, false);
                MenuItemHighlightController(mnuOldEngine, true);
                interpreter.Text = "P2";
            }
            else
            {
                MenuItemHighlightController(mnuNewEngine, true);
                MenuItemHighlightController(mnuOldEngine, false);
                interpreter.Text = "P3";
            }
        }
    }
}
