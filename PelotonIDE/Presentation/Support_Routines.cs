using Microsoft.UI;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Web;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private bool InFocusTabIsPrFile()
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            if (navigationViewItem.IsNewFile) return false;
            return navigationViewItem.SavedFilePath.Path.ToUpperInvariant().EndsWith(".PR");
        }

        private bool InFocusTabSettingIsDifferent<T>(string setting, T value)
        {
            T? lhs = Type_3_GetInFocusTab<T>(setting);
            return $"{lhs}" != $"{value}";
        }

        private void SetMenuText(Dictionary<string, string> selectedLanguage) // #StatusBar #InterfaceLanguage
        {
            menuBar.Items.ForEach(item =>
            {
                HandlePossibleAmpersandInMenuItem(selectedLanguage[item.Name], item);
                item.Items.ForEach(subitem =>
                {
                    if (selectedLanguage.TryGetValue(subitem.Name, out string? value))
                        HandlePossibleAmpersandInMenuItem(value, subitem);

                });
            });

            foreach ((string key, MenuFlyoutItem opt) keyControl in new List<(string, MenuFlyoutItem)>
            {
                ("mnuQuiet", mnuQuiet),
                ("mnuVerbose", mnuVerbose),
                ("mnuVerbosePauseOnExit", mnuVerbosePauseOnExit),
                ("mnu20Seconds", mnu20Seconds),
                ("mnu100Seconds", mnu100Seconds),
                ("mnu200Seconds", mnu200Seconds),
                ("mnu1000Seconds", mnu1000Seconds),
                ("mnuInfinite", mnuInfinite)
            })
            {
                HandlePossibleAmpersandInMenuItem(selectedLanguage[keyControl.key], keyControl.opt);
            }

            foreach ((string key, TabViewItem tvi) keyControl in new List<(string, TabViewItem)>
            {
                ("tabOutput", tabOutput ),
                ("tabError", tabError),
                ("tabHtml", tabHtml ),
                ("tabLogo", tabLogo),
            })
            {
                keyControl.tvi.Header = selectedLanguage[keyControl.key];
            }

            foreach ((string key, MenuFlyoutItem mfi) keyControl in new List<(string, MenuFlyoutItem)>
            {
                ("tabOutput", mnuOutput ),
                ("tabError", mnuError),
                ("tabHtml", mnuHTML ),
                ("tabLogo", mnuLogo),
                ("tabRTF",mnuRTF)

            })
            {
                keyControl.mfi.Text = selectedLanguage[keyControl.key];
            }

            txtRendering.Text = selectedLanguage["txtRendering"];
            mnuRendering.Text = selectedLanguage["txtRendering"];

            ToolTipService.SetToolTip(butNew, selectedLanguage["new.Tip"]);
            ToolTipService.SetToolTip(butOpen, selectedLanguage["open.Tip"]);
            ToolTipService.SetToolTip(butSave, selectedLanguage["save.Tip"]);
            ToolTipService.SetToolTip(butSaveAs, selectedLanguage["save.Tip"]);
            // ToolTipService.SetToolTip(butClose, selectedLanguage["close.Tip"]);
            ToolTipService.SetToolTip(butCopy, selectedLanguage["copy.Tip"]);
            ToolTipService.SetToolTip(butCut, selectedLanguage["cut.Tip"]);
            ToolTipService.SetToolTip(butPaste, selectedLanguage["paste.Tip"]);
            ToolTipService.SetToolTip(butSelectAll, selectedLanguage["mnuSelectOther(7)"]);
            ToolTipService.SetToolTip(butTransform, selectedLanguage["mnuTranslate"]);
            // ToolTipService.SetToolTip(toggleOutputButton, selectedLanguage["mnuToggleOutput"]);
            ToolTipService.SetToolTip(butGo, selectedLanguage["run.Tip"]);
        }

        private void HandleOutputPanelChange(string changeTo)
        {
            Telemetry t = new();
            t.SetEnabled(true);

            var outputPanelPosition = (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), changeTo);

            var outputPanelHeight = Type_1_GetVirtualRegistry<double>("OutputPanelHeight");
            var outputPanelWidth = Type_1_GetVirtualRegistry<double>("OutputPanelWidth");
            var outputPanelShowing = Type_1_GetVirtualRegistry<bool>("OutputPanelShowing");

            var outputPanelTabViewSettings = Type_1_GetVirtualRegistry<string>("OutputPanelTabView_Settings");
            var tabControlSettings = Type_1_GetVirtualRegistry<string>("TabControl_Settings");

            var optvPosition = FromBarredString_String(outputPanelTabViewSettings, 0);
            var optvHeight = FromBarredString_Double(outputPanelTabViewSettings, 1);
            var optvWidth = FromBarredString_Double(outputPanelTabViewSettings, 2);

            var tcPosition = FromBarredString_String(tabControlSettings, 0);
            var tcHeight = FromBarredString_Double(tabControlSettings, 1);
            var tcWidth = FromBarredString_Double(tabControlSettings, 2);

            switch (outputPanelPosition)
            {
                case OutputPanelPosition.Left:
                    //outputPanelPosition = OutputPanelPosition.Left;
                    Type_1_UpdateVirtualRegistry<string>("OutputPanelPosition", OutputPanelPosition.Left.ToString());
                    RelativePanel.SetAlignLeftWithPanel(outputPanel, true);
                    RelativePanel.SetAlignRightWithPanel(outputPanel, false);
                    RelativePanel.SetBelow(outputPanel, butNew);

                    //t.Transmit("(before) outputPanel.Width=", outputPanel.Width);
                    //t.Transmit("(before) outputPanel.Height=", outputPanel.Height);

                    outputPanel.Height = outputPanelHeight; // Type_1_GetVirtualRegistry<double?>("OutputPanelHeight") ?? 200.0;
                    outputPanel.Width = outputPanelWidth; //  Type_1_GetVirtualRegistry<double?>("OutputPanelWidth") ?? 400.0;

                    //t.Transmit(pos, "(after) outputPanel.Width=", outputPanel.Width);
                    //t.Transmit(pos, "(after) outputPanel.Height=", outputPanel.Height);

                    outputPanel.MinWidth = 175;
                    //outputPanel.MaxWidth = 700;
                    outputPanel.ClearValue(HeightProperty);
                    outputPanel.ClearValue(MaxHeightProperty);

                    RelativePanel.SetAbove(tabControl, statusBar);
                    RelativePanel.SetRightOf(tabControl, outputPanel);
                    RelativePanel.SetAlignLeftWithPanel(tabControl, false);
                    RelativePanel.SetAlignRightWithPanel(tabControl, true);

                    outputLeftButton.BorderBrush = new SolidColorBrush(Colors.DodgerBlue);
                    outputBottomButton.BorderBrush = new SolidColorBrush(Colors.LightGray);
                    outputRightButton.BorderBrush = new SolidColorBrush(Colors.LightGray);

                    outputLeftButton.Background = new SolidColorBrush(Colors.DeepSkyBlue);
                    outputBottomButton.Background = new SolidColorBrush(Colors.Transparent);
                    outputRightButton.Background = new SolidColorBrush(Colors.Transparent);

                    Canvas.SetLeft(outputThumb, outputPanel.Width - 1);
                    Canvas.SetTop(outputThumb, 0);

                    Type_1_UpdateVirtualRegistry("OutputPanelWidth", outputPanel.Width);

                    outputDockingFlyout.Hide();

                    break;
                case OutputPanelPosition.Bottom:
                    //outputPanelPosition = OutputPanelPosition.Bottom;
                    Type_1_UpdateVirtualRegistry<string>("OutputPanelPosition", OutputPanelPosition.Bottom.ToString());
                    RelativePanel.SetAlignLeftWithPanel(tabControl, true);
                    RelativePanel.SetAlignRightWithPanel(tabControl, true);
                    RelativePanel.SetRightOf(tabControl, null);
                    RelativePanel.SetAbove(tabControl, outputPanel);

                    RelativePanel.SetAlignLeftWithPanel(outputPanel, true);
                    RelativePanel.SetAlignRightWithPanel(outputPanel, true);
                    RelativePanel.SetBelow(outputPanel, null);

                    //t.Transmit("(before) outputPanel.Width=", outputPanel.Width);
                    //t.Transmit("(before) outputPanel.Height=", outputPanel.Height);

                    outputPanel.Height = outputPanelHeight; // Type_1_GetVirtualRegistry<double?>("OutputPanelHeight") ?? 200.0;
                    outputPanel.Width = outputPanelWidth; // Type_1_GetVirtualRegistry<double?>("OutputPanelWidth") ?? 400.0;

                    //t.Transmit(pos, "(after) outputPanel.Width=", outputPanel.Width);
                    //t.Transmit(pos, "(after) outputPanel.Height=", outputPanel.Height);


                    //outputPanel.MaxHeight = 500;
                    outputPanel.MinHeight = 100;
                    outputPanel.ClearValue(WidthProperty);
                    outputPanel.ClearValue(MaxWidthProperty);

                    outputBottomButton.BorderBrush = new SolidColorBrush(Colors.DodgerBlue);
                    outputLeftButton.BorderBrush = new SolidColorBrush(Colors.LightGray);
                    outputRightButton.BorderBrush = new SolidColorBrush(Colors.LightGray);

                    outputBottomButton.Background = new SolidColorBrush(Colors.DeepSkyBlue);
                    outputLeftButton.Background = new SolidColorBrush(Colors.Transparent);
                    outputRightButton.Background = new SolidColorBrush(Colors.Transparent);

                    Canvas.SetLeft(outputThumb, 0);
                    Canvas.SetTop(outputThumb, -4);

                    Type_1_UpdateVirtualRegistry("OutputPanelHeight", outputPanel.Height);

                    outputDockingFlyout.Hide();
                    break;
                case OutputPanelPosition.Right:
                    //outputPanelPosition = OutputPanelPosition.Right;
                    Type_1_UpdateVirtualRegistry<string>("OutputPanelPosition", OutputPanelPosition.Right.ToString());
                    RelativePanel.SetAlignLeftWithPanel(outputPanel, false);
                    RelativePanel.SetAlignRightWithPanel(outputPanel, true);
                    RelativePanel.SetBelow(outputPanel, butNew);

                    //t.Transmit("(before) outputPanel.Width=", outputPanel.Width);
                    //t.Transmit("(before) outputPanel.Height=", outputPanel.Height);

                    outputPanel.Height = outputPanelHeight;// Type_1_GetVirtualRegistry<double?>("OutputPanelHeight") ?? 200.0;
                    outputPanel.Width = outputPanelWidth; // Type_1_GetVirtualRegistry<double?>("OutputPanelWidth") ?? 400.0;

                    //t.Transmit(pos, "(after) outputPanel.Width=", outputPanel.Width);
                    //t.Transmit(pos, "(after) outputPanel.Height=", outputPanel.Height);

                    outputPanel.MinWidth = 175;
                    //outputPanel.MaxWidth = 700;
                    outputPanel.ClearValue(HeightProperty);
                    outputPanel.ClearValue(MaxHeightProperty);

                    RelativePanel.SetAbove(tabControl, statusBar);
                    RelativePanel.SetLeftOf(tabControl, outputPanel);
                    RelativePanel.SetAlignLeftWithPanel(tabControl, true);
                    RelativePanel.SetAlignRightWithPanel(tabControl, false);

                    outputRightButton.BorderBrush = new SolidColorBrush(Colors.DodgerBlue);
                    outputBottomButton.BorderBrush = new SolidColorBrush(Colors.LightGray);
                    outputLeftButton.BorderBrush = new SolidColorBrush(Colors.LightGray);

                    outputRightButton.Background = new SolidColorBrush(Colors.DeepSkyBlue);
                    outputBottomButton.Background = new SolidColorBrush(Colors.Transparent);
                    outputLeftButton.Background = new SolidColorBrush(Colors.Transparent);

                    Canvas.SetLeft(outputThumb, -4);
                    Canvas.SetTop(outputThumb, 0);

                    outputDockingFlyout.Hide();

                    Type_1_UpdateVirtualRegistry("OutputPanelWidth", outputPanel.Width);

                    break;
            }

        }

        private void ChangeHighlightOfMenuBarForLanguage(MenuBarItem mnuRun, string InterpreterLanguageName)
        {
            Telemetry t = new();
            t.SetEnabled(true);

            t.Transmit("InterpreterLanguageName=", InterpreterLanguageName);
            IEnumerable<MenuFlyoutItemBase> subMenus = from menu in mnuRun.Items where menu.Name == "mnuLanguage" select menu;
            t.Transmit("subMenus.Any()=", subMenus.Any());
            if (subMenus.Any())
            {
                MenuFlyoutItemBase first = subMenus.First();

                foreach (MenuFlyoutItemBase? item in ((MenuFlyoutSubItem)first).Items)
                {
                    t.Transmit("item.Name=", item.Name, "InterpreterLanguageName=", InterpreterLanguageName);
                    if (item.Name == InterpreterLanguageName)
                    {
                        item.Foreground = new SolidColorBrush(Colors.White);
                        item.Background = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        item.Foreground = new SolidColorBrush(Colors.Black);
                        item.Background = new SolidColorBrush(Colors.White);
                    }
                }
            }
        }


        static List<Plex>? GetAllPlexes()
        {
            //IReadOnlyDictionary<string, ApplicationDataContainer> folder = ApplicationData.Current.LocalSettings.Containers;

            List<Plex> list = [];
            foreach (var file in Directory.GetFiles(@"c:\peloton\bin\lexers", "*.lex"))
            {
                byte[] data = File.ReadAllBytes(file);
                using MemoryStream stream = new(data);
                using BsonDataReader reader = new(stream);
                JsonSerializer serializer = new();
                Plex? p = serializer.Deserialize<Plex>(reader);
                list.Add(p!);
            }

            return list;
        }

        private bool AnInFocusTabExists()
        {
            return _richEditBoxes.Count > 0;
        }

        private CustomTabItem? InFocusTab()
        {
            if (AnInFocusTabExists())
            {
                return (CustomTabItem)tabControl.SelectedItem;
            }
            else
            {
                return null;
            }
        }

        #region Getters

        private T? Type_3_GetInFocusTab<T>(string name)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            T? result = default;
            if (navigationViewItem != null)
            {
                result = ((bool)navigationViewItem.TabSettingsDict[name]["Defined"]) ? (T)navigationViewItem.TabSettingsDict[name]["Value"] : default;
            }
            return result;
        }

        private T Type_1_GetVirtualRegistry<T>(string name)
        {
            Telemetry t = new();
            t.SetEnabled(false);
            object result = ApplicationData.Current.LocalSettings.Values[name];
            t.Transmit(name + "=", name, "result=", result);
            return (T)result;
        }

        private T? Type_2_GetPerTabSettings<T>(string name)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            return (bool)PerTabInterpreterParameters[name]["Defined"] ? (T?)(T)PerTabInterpreterParameters[name]["Value"] : default;
        }
        #endregion

        #region Setters

        // 1. virt reg
        private void Type_1_UpdateVirtualRegistry<T>(string name, T value)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit(name, value);
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }

        // 2. pertab
        private void Type_2_UpdatePerTabSettings<T>(string name, bool enabled, T value)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit(name, enabled, value);
            PerTabInterpreterParameters[name]["Defined"] = enabled;
            PerTabInterpreterParameters[name]["Value"] = value!;
        }

        // 3. currtab
        private void Type_3_UpdateInFocusTabSettings<T>(string name, bool enabled, T value)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit(name, enabled, value);
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            navigationViewItem.TabSettingsDict[name]["Defined"] = enabled;
            navigationViewItem.TabSettingsDict[name]["Value"] = value!;
        }

        private async Task Type_3_UpdateInFocusTabSettingsIfPermittedAsync<T>(string name, bool defined, T value, string prompt)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit(name, defined, value);
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            var currentDefined = (bool)navigationViewItem.TabSettingsDict[name]["Defined"];
            var currentValue = (T)navigationViewItem.TabSettingsDict[name]["Value"];
            if (currentDefined == defined && $"{currentValue}" == $"{value}")
            {
                return;
            }
            if (await ChangingSettingsAllowed(prompt))
            {
                navigationViewItem.TabSettingsDict[name]["Defined"] = defined;
                navigationViewItem.TabSettingsDict[name]["Value"] = value!;

            }
        }

        private async Task<bool> ChangingSettingsAllowed(string prompt)
        {
            string il = Type_1_GetVirtualRegistry<string>("InterfaceLanguageName");
            Dictionary<string, string> global = LanguageSettings[il]["GLOBAL"];

            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = prompt,
                PrimaryButtonText = global["1209"],
                SecondaryButtonText = global["1207"]
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary) { return true; }
            if (result == ContentDialogResult.Primary) { return false; }
            return false;
        }

        #endregion
        private void IfNotInVirtualRegistryUpdateItFromFactorySettingsOrDefault<T>(string name, Dictionary<string, object>? factory, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(name)) return;
            if (factory.TryGetValue(name, out object? factoryValue))
            {
                LocalSettings.Values[name] = factoryValue;
                return;
            }
            LocalSettings.Values[name] = (defaultValue.GetType().BaseType.Name == "Enum") ? defaultValue.ToString() : defaultValue;
            return;
        }

        private void SerializeTabsToVirtualRegistry()
        {
            Telemetry t = new();
            t.SetEnabled(true);
            string list = string.Join(',', outputPanelTabView.TabItems.Select(e =>
            {
                TabViewItem f = (TabViewItem)e;
                return (f.IsSelected ? "*" : "") + f.Name;
            }));
            t.Transmit(list);
            Type_1_UpdateVirtualRegistry<string>("TabViewLayout", list);
        }

        private void DeserializeTabsFromVirtualRegistry()
        {
            Telemetry t = new();
            t.SetEnabled(true);

            var tabViewLayout = Type_1_GetVirtualRegistry<string>("TabViewLayout");
            if (tabViewLayout == null) return;

            string frontMost = "";
            tabViewLayout.Split(',').ForEach(key =>
            {
                if (key.StartsWith("*"))
                {
                    key = key[1..];
                    frontMost = key;
                    t.Transmit("frontMost=", frontMost);
                }
                TabViewItem found = (TabViewItem)outputPanelTabView.FindName(key);
                //found.IsSelected = false;
                outputPanelTabView.TabItems.Remove(found);
                outputPanelTabView.TabItems.Add(found);
                t.Transmit("Remove/Add=", found.Name);

            });
            if (frontMost.Length > 0)
            {
                TabViewItem found = (TabViewItem)outputPanelTabView.FindName(frontMost);
                outputPanelTabView.SelectedItem = found;
                t.Transmit(found.Name, "is selected item");
            }
        }

        private bool FromBarredString_Boolean(string list, int entry)
        {
            string item = list.Split(['|'])[entry];
            return bool.Parse(item);
        }
        private string FromBarredString_String(string list, int entry)
        {
            string item = list.Split(['|'])[entry];
            return item;
        }
        private double FromBarredString_Double(string list, int entry)
        {
            string item = list.Split(['|'])[entry];
            return double.Parse(item);
        }

    }
}
