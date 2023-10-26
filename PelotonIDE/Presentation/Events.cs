using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.System;
using Windows.UI.Core;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        public static FontIcon tick = new FontIcon()
        {
            FontFamily = new FontFamily("Segoe MDL2 Assets"),
            Glyph = "\uF0B7"
        };

        private void InterpretBar_RunningMode_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in from key in new string[] { "mnuQuiet", "mnuVerbose", "mnuVerbosePauseOnExit" }
                                 let items = from item in mnuRunningMode.Items where item.Name == key select item
                                 from item in items
                                 select item)
            {
                (item as MenuFlyoutItem).Icon = null;
            }
            GlobalInterpreterParameters["Quietude"]["Defined"] = false;

            var me = sender as MenuFlyoutItem;
            var clicked = me.Name;
            mnuRunningMode.Tag = clicked;
            switch (clicked)
            {
                case "mnuQuiet":
                    me.Icon = tick;
                    GlobalInterpreterParameters["Quietude"]["Value"] = 0;
                    break;
                case "mnuVerbose":
                    me.Icon = tick;
                    GlobalInterpreterParameters["Quietude"]["Value"] = 1;
                    break;
                case "mnuVerbosePauseOnExit":
                    me.Icon = tick;
                    GlobalInterpreterParameters["Quietude"]["Value"] = 2;
                    break;
            }
            GlobalInterpreterParameters["Quietude"]["Defined"] = true;
            UpdateTabCommandLine(tabCommandLine);
        }

        private void FileNew_Click(object sender, RoutedEventArgs e)
        {
            CreateNewRichEditBox();
        }

        private async void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void FileClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void FileSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private async void FileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void EditCopy_Click(object sender, RoutedEventArgs e)
        {
            CopyText();
        }

        private void EditCut_Click(object sender, RoutedEventArgs e)
        {
            Cut();
        }

        private async void EditPaste_Click(object sender, RoutedEventArgs e)
        {
            Paste();
        }

        private void EditSelectAll_Click(object sender, RoutedEventArgs e)
        {
            SelectAll();
        }

        private void OutputLeft_Click(object sender, RoutedEventArgs e)
        {
            HandleOutputPanelChange(OutputPanelPosition.Left);
        }

        private void OutputBottom_Click(object sender, RoutedEventArgs e)
        {
            HandleOutputPanelChange(OutputPanelPosition.Bottom);
        }

        private void OutputRight_Click(object sender, RoutedEventArgs e)
        {
            HandleOutputPanelChange(OutputPanelPosition.Right);
        }

        private void TabControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (tabControl.SelectedItem != null)
            {
                CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
                tabControl.Content = _richEditBoxes[navigationViewItem.Tag];
            }
        }

        private void RichEditBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Debug.WriteLine($"{e.Key}");
            if (e.Key == VirtualKey.CapitalLock)
            {
                CAPS.Text = Console.CapsLock ? "CAPS" : "caps";
            }
            if (e.Key == VirtualKey.NumberKeyLock)
            {
                NUM.Text = Console.NumberLock ? "NUM" : "num";
            }
            if (e.Key == VirtualKey.Scroll)
            {
                SCRL.Text = Console.NumberLock ? "SCRL" : "scrl";
            }
            if (e.Key == VirtualKey.Insert)
            {
            }
            if (tabControl.Content is CustomRichEditBox currentRichEditBox)
            {
                currentRichEditBox.Document.GetText(TextGetOptions.None, out string text);
                //wordCount.Text = text.Split(' ').Length - 1 + " words";
                int caretPosition = currentRichEditBox.Document.Selection.StartPosition;
                int lineNumber = 1;
                int charNumber = 0;
                for (int i = 0; i < caretPosition; i++)
                {
                    charNumber++;
                    if (text[i] == '\r')
                    {
                        lineNumber++;
                        charNumber = 0;
                    }
                }
                int charsSinceLastLineBreak = 1;
                for (int i = caretPosition - 1; i >= 0; i--)
                {
                    if (text[i] == '\r')
                    {
                        break;
                    }
                    charsSinceLastLineBreak++;
                }
                cursorPosition.Text = "Line " + lineNumber + ", Char " + charsSinceLastLineBreak;
            }
        }

        private void Thumb_DragDelta(object sender, Microsoft.UI.Xaml.Controls.Primitives.DragDeltaEventArgs e)
        {
            var yadjust = outputPanel.Height - e.VerticalChange;
            var xRightAdjust = outputPanel.Width - e.HorizontalChange;
            var xLeftAdjust = outputPanel.Width + e.HorizontalChange;
            if (outputPosition == OutputPanelPosition.Bottom)
            {
                if (yadjust >= 0)
                {
                    outputPanel.Height = yadjust;
                }
            }
            else if (outputPosition == OutputPanelPosition.Left)
            {
                if (xLeftAdjust >= 0)
                {
                    outputPanel.Width = xLeftAdjust;
                }
            }
            else if (outputPosition == OutputPanelPosition.Right)
            {
                if (xRightAdjust >= 0)
                {
                    outputPanel.Width = xRightAdjust;
                }
            }

            if (outputPosition == OutputPanelPosition.Bottom)
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeNorthSouth, 0));
            }
            else
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeWestEast, 0));
            }
        }

        private void OutputPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (outputPosition == OutputPanelPosition.Bottom)
            {
                outputPanelTabView.Width = outputPanel.ActualWidth;
                outputPanelTabView.Height = outputPanel.ActualHeight;
                outputThumb.Width = outputPanel.ActualWidth;
                outputThumb.Height = 5;
            }
            else if (outputPosition == OutputPanelPosition.Right)
            {
                outputPanelTabView.Width = outputPanel.ActualWidth;
                outputPanelTabView.Height = outputPanel.ActualHeight;
                outputThumb.Width = 5;
                outputThumb.Height = outputPanel.ActualHeight;
            }
            else if (outputPosition == OutputPanelPosition.Left)
            {
                outputPanelTabView.Width = outputPanel.ActualWidth;
                outputPanelTabView.Height = outputPanel.ActualHeight;
                outputThumb.Width = 5;
                outputThumb.Height = outputPanel.ActualHeight;
                Canvas.SetLeft(outputThumb, outputPanel.ActualWidth - 1);
            }
        }

        private async void OutputThumb_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (outputPosition == OutputPanelPosition.Bottom)
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeNorthSouth, 0));
            }
            else
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeWestEast, 0));
            }
        }

        private void OutputThumb_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 0));
        }

        private void OutputThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 0));
        }

        private void Internationalization_Click(object sender, RoutedEventArgs e)
        {
            var me = (MenuFlyoutItem)sender;
            var name = me.Name;
            HandleLanguageChange(name);
        }

        private void Cut()
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            string selectedText = currentRichEditBox.Document.Selection.Text;
            var dataPackage = new DataPackage();
            dataPackage.SetText(selectedText);
            Clipboard.SetContent(dataPackage);
            currentRichEditBox.Document.Selection.Delete(Microsoft.UI.Text.TextRangeUnit.Character, 1);
        }

        private async void TransformButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Reverse?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };
            DialogContentPage dialogContentPage = new();

            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            currentRichEditBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string selectedText);
            selectedText = selectedText.TrimEnd('\r');
            dialogContentPage.SetText(selectedText);
            dialog.Content = dialogContentPage;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string reversedString = Reverse(selectedText);
                CreateNewRichEditBox();
                navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
                currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
                currentRichEditBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, reversedString);
            }
        }

        private void ToggleOutputButton_Click(object sender, RoutedEventArgs e)
        {
            outputPanel.Visibility = outputPanelShowing ? Visibility.Collapsed : Visibility.Visible;
            outputPanelShowing = !outputPanelShowing;
        }

        // InsertCodeTemplate_Click

        private void InsertCodeTemplate_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            var selection = currentRichEditBox.Document.Selection;
            if (selection != null)
            {
                selection.StartPosition = selection.EndPosition;
                selection.Text = "<@ ></@>";
                selection.EndPosition = selection.StartPosition;
                currentRichEditBox.Document.Selection.Move(TextRangeUnit.Character, 3);
            }

        }

        private void InsertVariableLengthCodeTemplate_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            var selection = currentRichEditBox.Document.Selection;
            if (selection != null)
            {
                selection.StartPosition = selection.EndPosition;
                selection.Text = "<# ></#>";
                selection.EndPosition = selection.StartPosition;
                currentRichEditBox.Document.Selection.Move(TextRangeUnit.Character, 3);
            }

        }

        private void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            currentRichEditBox.Document.GetText(TextGetOptions.None, out string selectedText);
            ExecuteInterpreter(selectedText.TrimEnd('\r'));  // FIXME pass in some kind of identifier to connect to the tab
        }

        private void RunSelectedCodeButton_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];

            var selection = currentRichEditBox.Document.Selection;

            string selectedText = selection.Text;
            selectedText.TrimEnd('\r');
            if (selectedText.Length > 0)
            {
                ExecuteInterpreter(selectedText); // FIXME pass in some kind of identifier to connect to the tab
            }
        }

        private void VariableLength_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];

            navigationViewItem.tabSettingJson["VariableLength"]["Defined"] = true;
            if (mnuVariableLength.Icon == null)
            {
                mnuVariableLength.Icon = tick;
                navigationViewItem.tabSettingJson["VariableLength"]["Value"] = true;
            }
            else
            {
                mnuVariableLength = null;
                navigationViewItem.tabSettingJson["VariableLength"]["Value"] = false;
            }

            UpdateTabCommandLine(tabCommandLine);
        }

        private void MnuLanguage_Click(object sender, RoutedEventArgs e)
        {
            var me = (MenuFlyoutItem)sender;
            var lang = me.Name;

            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];

            var text = me.Text;

            var id = LanguageSettings[lang]["GLOBAL"]["ID"];

            navigationViewItem.tabSettingJson["Language"]["Defined"] = true;
            navigationViewItem.tabSettingJson["Language"]["Value"] = id;

            languageName.Text = LanguageSettings[currentLanguageName]["GLOBAL"][$"{101+int.Parse(id)}"]; // the international language setting actually, not lang
                
            UpdateTabCommandLine(tabCommandLine);
        }

        private async void ResetToFactorySettings_Click(object sender, RoutedEventArgs e)
        {
            foreach (var setting in ApplicationData.Current.LocalSettings.Values)
            {
                Debug.WriteLine($"{setting.Key} => {setting.Value}");
                ApplicationData.Current.LocalSettings.DeleteContainer(setting.Key);
            }
            await ApplicationData.Current.ClearAsync(); 
        }
        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "PelotonIDE v20230614",
                Content = "Based on original code by\r\nHakob Chalikyan <hchalikyan3@gmail.com>",
                CloseButtonText = "OK"
            };
            _ = dialog.ShowAsync();
            
        }

    }
}
