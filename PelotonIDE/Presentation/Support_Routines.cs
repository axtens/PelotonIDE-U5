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

            HandlePossibleAmpersandInMenuItem(selectedLanguage["mnuQuiet"], mnuQuiet);
            HandlePossibleAmpersandInMenuItem(selectedLanguage["mnuVerbose"], mnuVerbose);
            HandlePossibleAmpersandInMenuItem(selectedLanguage["mnuVerbosePauseOnExit"], mnuVerbosePauseOnExit);

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

        private void HandleOutputPanelChange(OutputPanelPosition panelPos)
        {
            if (panelPos == OutputPanelPosition.Left)
            {
                outputPanelPosition = OutputPanelPosition.Left;

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
            }
            else if (panelPos == OutputPanelPosition.Bottom)
            {
                outputPanelPosition = OutputPanelPosition.Bottom;

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
            }
            else if (panelPos == OutputPanelPosition.Right)
            {
                outputPanelPosition = OutputPanelPosition.Right;

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
            }
        }

        private void UpdateLanguageInInterpreterMenu(MenuBarItem mnuRun, string InterpreterLanguageName)
        {
            IEnumerable<MenuFlyoutItemBase> subMenus = from menu in mnuRun.Items where menu.Name == "mnuLanguage" select menu;
            if (subMenus != null)
            {
                MenuFlyoutItemBase first = subMenus.First();
                foreach (MenuFlyoutItemBase? item in ((MenuFlyoutSubItem)first).Items)
                {
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

        public static void Track(params object?[] args)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            string path = Path.Combine(folder.Path, "pi.log");
            
            List<string> frame = [];
            var trace = new System.Diagnostics.StackTrace();
            /*for (int i = 1; i < trace.FrameCount; i++)
            {
                string name = trace.GetFrame(i).GetMethod().Name;
                frame.Add(name);
                if (name.Substring(0, 1) == "<") break;
            }
            string you = string.Join(':',frame.ToArray().Reverse());
            */
            string you = trace.GetFrame(1).GetMethod().Name;

            StringBuilder sb = new();
            sb.Append($"From {you}: ");
            for (int i = 0; i < args.Length; i++)
            {
                string item = $"{args[i]}";
                if (i == 0)
                {
                    sb.Append(item);
                }
                else
                {
                    string prev = $"{args[i - 1]}";
                    if (prev.EndsWith("="))
                    {
                        sb.Append(item);
                    }
                    else
                    {
                        sb.Append(' ').Append(item);
                    }
                }
            }
            //File.AppendAllText(path, "---\r\n", Encoding.UTF8);
            File.AppendAllText(path, $"{DateTime.Now:o} > {sb}\r\n", Encoding.UTF8);
        }

        static List<Plex>? GetAllPlexes()
        {
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
    }
}
