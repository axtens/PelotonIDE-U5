using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;

using Windows.ApplicationModel.DataTransfer;
using Windows.System;
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
            //Style = new Style()
            //{
            //    TargetType = CustomRichEditBox,
            //    Setters = {}
            //}
            //Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0xF9, 0xF8, 0xBD));
            //Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0xF9, 0xF8, 0xBD));

        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            var isCtrlPressed = ctrlState.HasFlag(CoreVirtualKeyStates.Down);

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
            var dataPackageView = Clipboard.GetContent();
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
            Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out var allText);
            var endPosition = allText.Length - 1;
            Document.Selection.SetRange(0, endPosition);
        }
    }
}
