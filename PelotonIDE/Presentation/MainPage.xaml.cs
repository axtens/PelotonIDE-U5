using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using System.Diagnostics;
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
        Dictionary<object, CustomRichEditBox> _richEditBoxes = new Dictionary<object, CustomRichEditBox>();
        bool outputPanelShowing = true;
        enum OutputPanelPosition
        {
            Left,
            Bottom,
            Right
        }
        OutputPanelPosition outputPosition = OutputPanelPosition.Bottom;
        public MainPage()
        {
            this.InitializeComponent();
            CustomRichEditBox richEditBox = new CustomRichEditBox();
            richEditBox.Tag = "Tab1";
            richEditBox.KeyDown += RichEditBox_KeyDown;
            richEditBox.SelectionFlyout = null;
            richEditBox.ContextFlyout = null;
            tabControl.Content = richEditBox;
            _richEditBoxes[richEditBox.Tag] = richEditBox;
            tabControl.SelectedItem = tab1;
            var isCapsLocked = Console.CapsLock;
            var isNumLocked = Console.NumberLock;
            capsLock.Text = isCapsLocked ? "Caps Lock: On" : "Caps Lock: Off";
            numsLock.Text = isNumLocked ? "Num Lock: On" : "Num Lock: Off";
        }

        #region Event Handlers

        private void RichEditBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.CapitalLock)
            {
                var isCapsLocked = Console.CapsLock;
                capsLock.Text = isCapsLocked ? "Caps Lock: On" : "Caps Lock: Off";
            }
            if (e.Key == VirtualKey.NumberKeyLock)
            {
                var isNumLocked = Console.NumberLock;
                numsLock.Text = isNumLocked ? "Num Lock: On" : "Num Lock: Off";
            }
            CustomRichEditBox? currentRichEditBox = tabControl.Content as CustomRichEditBox;
            if (currentRichEditBox != null)
            {
                string text = "";
                currentRichEditBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out text);
                wordCount.Text = text.Split(' ').Count() - 1 + " words";
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

        private void newFileButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewRichEditBox();
        }

        private async void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void closeFileButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void saveFileButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyText();
        }

        private void cutButton_Click(object sender, RoutedEventArgs e)
        {
            Cut();
        }

        private async void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            Paste();
        }

        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            SelectAll();
        }

        private void tabControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (tabControl.SelectedItem != null)
            {
                NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
                tabControl.Content = _richEditBoxes[navigationViewItem.Tag];
            }
        }

        private void OutputLeft_Click(object sender, RoutedEventArgs e)
        {
            outputPosition = OutputPanelPosition.Left;

            RelativePanel.SetAlignLeftWithPanel(outputPanel, true);
            RelativePanel.SetAlignRightWithPanel(outputPanel, false);
            RelativePanel.SetBelow(outputPanel, newFileButton);
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

        private void OutputBottom_Click(object sender, RoutedEventArgs e)
        {
            outputPosition = OutputPanelPosition.Bottom;

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

        private void OutputRight_Click(object sender, RoutedEventArgs e)
        {
            outputPosition = OutputPanelPosition.Right;

            RelativePanel.SetAlignLeftWithPanel(outputPanel, false);
            RelativePanel.SetAlignRightWithPanel(outputPanel, true);
            RelativePanel.SetBelow(outputPanel, newFileButton);
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

        private async void transformButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Reverse?";
            dialog.PrimaryButtonText = "Yes";
            dialog.SecondaryButtonText = "No";
            DialogContentPage dialogContentPage = new DialogContentPage();

            NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            currentRichEditBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string selectedText);
            selectedText = selectedText.ToString().TrimEnd('\r');
            dialogContentPage.SetText(selectedText);
            dialog.Content = dialogContentPage;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string reversedString = Reverse(selectedText);
                CreateNewRichEditBox();
                navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
                currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
                currentRichEditBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, reversedString);
            }
        }

        private void showOutputButton_Click(object sender, RoutedEventArgs e)
        {
            outputPanel.Visibility = outputPanelShowing ? Visibility.Collapsed : Visibility.Visible;
            outputPanelShowing = !outputPanelShowing;
        }

        #endregion

        #region Edit Handlers

        private void CreateNewRichEditBox()
        {
            CustomRichEditBox richEditBox = new CustomRichEditBox();
            richEditBox.KeyDown += RichEditBox_KeyDown;
            NavigationViewItem navigationViewItem = new NavigationViewItem()
            {
                Content = "Tab " + (tabControl.MenuItems.Count + 1),
                Tag = "Tab" + (tabControl.MenuItems.Count + 1)
            };
            richEditBox.Tag = navigationViewItem.Tag;
            tabControl.Content = richEditBox;
            _richEditBoxes[richEditBox.Tag] = richEditBox;
            tabControl.MenuItems.Add(navigationViewItem);
            tabControl.SelectedItem = navigationViewItem;
            richEditBox.Focus(FocusState.Keyboard);
        }

        private void Cut()
        {
            NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            string selectedText = currentRichEditBox.Document.Selection.Text;
            var dataPackage = new DataPackage();
            dataPackage.SetText(selectedText);
            Clipboard.SetContent(dataPackage);
            currentRichEditBox.Document.Selection.Delete(Microsoft.UI.Text.TextRangeUnit.Character, 1);
        }

        private async void Open()
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".rtf");

            // For Uno.WinUI-based apps
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App._window);
            WinRT.Interop.InitializeWithWindow.Initialize(open, hwnd);

            StorageFile pickedFile = await open.PickSingleFileAsync();
            if (pickedFile != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await pickedFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Load the file into the Document property of the RichEditBox.
                    CreateNewRichEditBox();
                    NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.MenuItems[tabControl.MenuItems.Count - 1];
                    navigationViewItem.Content = pickedFile.DisplayName;
                    CustomRichEditBox newestRichEditBox = _richEditBoxes[navigationViewItem.Tag];
                    newestRichEditBox.Document.LoadFromStream(Microsoft.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                }
            }
        }

        private async void Save()
        {
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            // For Uno.WinUI-based apps
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App._window);
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
                    CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
                    currentRichEditBox.Document.SaveToStream(Microsoft.UI.Text.TextGetOptions.FormatRtf, randAccStream);
                }

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox =
                        new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
                NavigationViewItem savedItem = (NavigationViewItem)tabControl.SelectedItem;
                savedItem.Content = file.Name;
            }
        }

        private void CopyText()
        {
            NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            string selectedText = currentRichEditBox.Document.Selection.Text;
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(selectedText);
            Clipboard.SetContent(dataPackage);
        }

        private void Close()
        {
            if (tabControl.MenuItems.Count > 0)
            {
                NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
                _richEditBoxes.Remove(navigationViewItem.Tag);
                tabControl.MenuItems.Remove(tabControl.SelectedItem);
                if (tabControl.MenuItems.Count > 0)
                {
                    tabControl.SelectedItem = tabControl.MenuItems[tabControl.MenuItems.Count - 1];
                }
                else
                {
                    tabControl.Content = null;
                }
            }
        }

        private async void Paste()
        {
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                string textToPaste = await dataPackageView.GetTextAsync();

                if (!string.IsNullOrEmpty(textToPaste))
                {
                    NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
                    CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
                    currentRichEditBox.Document.Selection.Paste(0);
                }
            }
        }

        private void SelectAll()
        {
            NavigationViewItem navigationViewItem = (NavigationViewItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            currentRichEditBox.Focus(FocusState.Pointer);
            currentRichEditBox.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out var allText);
            var endPosition = allText.Length - 1;
            currentRichEditBox.Document.Selection.SetRange(0, endPosition);
        }

        #endregion

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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

        private void outputPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (outputPosition == OutputPanelPosition.Bottom)
            {
                outputPanelTabView.Width = outputPanel.ActualWidth;
                outputThumb.Width = outputPanel.ActualWidth;
                outputThumb.Height = 5;
            } 
            else if (outputPosition == OutputPanelPosition.Right)
            {
                outputPanelTabView.Width = outputPanel.ActualWidth;
                outputThumb.Width = 5;
                outputThumb.Height = outputPanel.ActualHeight;
            }
            else if (outputPosition == OutputPanelPosition.Left)
            {
                outputPanelTabView.Width = outputPanel.ActualWidth;
                outputThumb.Width = 5;
                outputThumb.Height = outputPanel.ActualHeight;
                Canvas.SetLeft(outputThumb, outputPanel.ActualWidth - 1);
            }
        }

        private async void outputThumb_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
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

        private void outputThumb_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 0));
        }

        private void outputThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 0));
        }
    }
}