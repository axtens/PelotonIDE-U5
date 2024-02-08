using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// using Uno.Extensions.Authentication.WinUI;

using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;

namespace PelotonIDE.Presentation
{
    public partial class CustomRichEditBox : RichEditBox
    {
        public bool IsRTF { get; set; }
        public bool IsDirty { get; set; }

        public CustomRichEditBox()
        {
            IsSpellCheckEnabled = false;
            IsRTF = true;
            SelectionFlyout = null;
            ContextFlyout = null;
            TextAlignment = TextAlignment.DetectFromContent;
            FlowDirection = FlowDirection.LeftToRight;
            FontFamily = new FontFamily("Lucida Sans Unicode,Tahoma");
            
            // https://stackoverflow.com/ai/search/16916
            //Background = new SolidColorBrush(Color.FromArgb(255,0xF9,0xF8, 0xbd)); // "#F9F8BD"
            //PointerEntered += (sender, e) => e.Handled = true;
            //Style = (Style)Application.Current.Resources["CustomRichEditBoxStyle"];
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            CoreVirtualKeyStates ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            bool isCtrlPressed = ctrlState.HasFlag(CoreVirtualKeyStates.Down);

            if (e.Key == VirtualKey.X && isCtrlPressed)
            {
                Cut();
                return;
            }
            if (e.Key == VirtualKey.C && isCtrlPressed)
            {
                CopyText();
                return;
            }
            if (e.Key == VirtualKey.V && isCtrlPressed)
            {
                PasteText();
                return;
            }
            if (e.Key == VirtualKey.A && isCtrlPressed)
            {
                SelectAll();
                return;
            }
            if (e.Key == VirtualKey.Tab)
            {
                Document.Selection.TypeText("\t");
                e.Handled = true;
                return;
            }
            base.OnKeyDown(e);
        }

        private void Cut()
        {
            string selectedText = Document.Selection.Text;
            DataPackage dataPackage = new();
            dataPackage.SetText(selectedText);
            Clipboard.SetContent(dataPackage);
            Document.Selection.Delete(Microsoft.UI.Text.TextRangeUnit.Character, 1);
        }

        private void CopyText()
        {
            string selectedText = Document.Selection.Text;
            DataPackage dataPackage = new();
            dataPackage.SetText(selectedText);
            Clipboard.SetContent(dataPackage);
        }

        private async void PasteText()
        {
            DataPackageView dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                string textToPaste = await dataPackageView.GetTextAsync();

                if (!string.IsNullOrEmpty(textToPaste))
                {
                    Document.Selection.Paste(0);
                }
            }
        }

        private void SelectAll()
        {
            Focus(FocusState.Pointer);
            Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string? allText);
            int endPosition = allText.Length - 1;
            Document.Selection.SetRange(0, endPosition);
        }
    }
}
