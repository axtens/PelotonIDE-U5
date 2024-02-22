using Microsoft.UI;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private void SetMenuText(Dictionary<string, string> selectedLanguage)
        {
            foreach (MenuBarItem? mi in menuBar.Items)
            {
                //Debug.WriteLine($"mi {mi.Name}");
                HandlePossibleAmpersandInMenuItem(selectedLanguage[mi.Name], mi);

                foreach (MenuFlyoutItemBase? mii in mi.Items)
                {
                    //Debug.WriteLine($"mii {mii.Name}");
                    if (selectedLanguage.TryGetValue(mii.Name, out string? value))
                        HandlePossibleAmpersandInMenuItem(value, mii);
                }
            }

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

            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnuQuiet"], mnuQuiet);
            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnuVerbose"], mnuVerbose);
            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnuVerbosePauseOnExit"], mnuVerbosePauseOnExit);

            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnu20Seconds"], mnu20Seconds);
            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnu100Seconds"], mnu100Seconds);
            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnu200Seconds"], mnu200Seconds);
            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnu1000Seconds"], mnu1000Seconds);
            //HandlePossibleAmpersandInMenuItem(selectedLanguage["mnuInfinite"], mnuInfinite);


            // HandlePossibleAmpersandInMenuItem(selectedLanguage["chkSpaceOut"], mnuSpaced);
            // HandlePossibleAmpersandInMenuItem(selectedLanguage["cmdClearAll"], mnuReset);

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

        private void HandleOutputPanelChange(string pos)
        {
            var outputPanelPosition = (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), pos);
            switch (outputPanelPosition)
            {
                case OutputPanelPosition.Left:
                    //outputPanelPosition = OutputPanelPosition.Left;

                    RelativePanel.SetAlignLeftWithPanel(outputPanel, true);
                    RelativePanel.SetAlignRightWithPanel(outputPanel, false);
                    RelativePanel.SetBelow(outputPanel, mnuNew);
                    outputPanel.Width = 200;
                    outputPanel.MinWidth = 175;
                    outputPanel.MaxWidth = 700;
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

                    outputDockingFlyout.Hide();
                    break;
                case OutputPanelPosition.Bottom:
                    //outputPanelPosition = OutputPanelPosition.Bottom;

                    RelativePanel.SetAlignLeftWithPanel(tabControl, true);
                    RelativePanel.SetAlignRightWithPanel(tabControl, true);
                    RelativePanel.SetRightOf(tabControl, null);
                    RelativePanel.SetAbove(tabControl, outputPanel);

                    RelativePanel.SetAlignLeftWithPanel(outputPanel, true);
                    RelativePanel.SetAlignRightWithPanel(outputPanel, true);
                    RelativePanel.SetBelow(outputPanel, null);
                    outputPanel.Height = 200;
                    outputPanel.MaxHeight = 500;
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

                    outputDockingFlyout.Hide();
                    break;
                case OutputPanelPosition.Right:
                    //outputPanelPosition = OutputPanelPosition.Right;

                    RelativePanel.SetAlignLeftWithPanel(outputPanel, false);
                    RelativePanel.SetAlignRightWithPanel(outputPanel, true);
                    RelativePanel.SetBelow(outputPanel, mnuNew);
                    outputPanel.Width = 200;
                    outputPanel.MinWidth = 175;
                    outputPanel.MaxWidth = 700;
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
                    break;
            }
        }

        private void ChangeHighlightOfMenuBarForLanguage(MenuBarItem mnuRun, string InterpreterLanguageName)
        {
            Telemetry telem = new();
            telem.SetEnabled(false);

            telem.Transmit("InterpreterLanguageName=", InterpreterLanguageName);
            IEnumerable<MenuFlyoutItemBase> subMenus = from menu in mnuRun.Items where menu.Name == "mnuLanguage" select menu;
            telem.Transmit("subMenus.Any()=", subMenus.Any());
            if (subMenus.Any())
            {
                MenuFlyoutItemBase first = subMenus.First();
                foreach (MenuFlyoutItemBase? item in ((MenuFlyoutSubItem)first).Items)
                {
                    telem.Transmit("item.Name=", item.Name, "InterpreterLanguageName=", InterpreterLanguageName);
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

        #region Getters

        private T? Type_3_GetInFocusTab<T>(string name)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
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
            Telemetry telem = new();
            telem.SetEnabled(false);
            object result = ApplicationData.Current.LocalSettings.Values[name];
            telem.Transmit(name + "=", name, "result=", result);
            return (T)result;
        }

        private T? Type_2_GetPerTabSettings<T>(string name)
        {
            Telemetry telem = new();
            telem.SetEnabled(false);
            return (bool)PerTabInterpreterParameters[name]["Defined"] ? (T?)(T)PerTabInterpreterParameters[name]["Value"] : default;
        }
        #endregion

        #region Setters

        // 1. virt reg
        private void Type_1_UpdateVirtualRegistry<T>(string name, T value)
        {
            Telemetry telem = new();
            telem.SetEnabled(false);
            telem.Transmit(name, value);
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }

        // 2. pertab
        private void Type_2_UpdatePerTabSettings<T>(string name, bool enabled, T value)
        {
            Telemetry telem = new();
            telem.SetEnabled(false);
            telem.Transmit(name, enabled, value);
            PerTabInterpreterParameters[name]["Defined"] = enabled;
            PerTabInterpreterParameters[name]["Value"] = value!;
        }

        // 3. currtab
        private void Type_3_UpdateInFocusTabSettings<T>(string name, bool enabled, T value)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            telem.Transmit(name, enabled, value);
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            navigationViewItem.TabSettingsDict[name]["Defined"] = enabled;
            navigationViewItem.TabSettingsDict[name]["Value"] = value!;
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


    }
}
