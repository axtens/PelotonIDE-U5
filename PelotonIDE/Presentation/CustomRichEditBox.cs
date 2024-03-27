﻿using DocumentFormat.OpenXml.Office2019.Presentation;

using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using Newtonsoft.Json;

using System.Diagnostics;

using Uno.Extensions;



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
        //public string PreviousSelection { get; set; }

        public CustomRichEditBox()
        {
            IsSpellCheckEnabled = false;
            IsRTF = true;
            SelectionFlyout = null;
            ContextFlyout = null;
            TextAlignment = TextAlignment.DetectFromContent;
            FlowDirection = FlowDirection.LeftToRight;
            FontFamily = new FontFamily("Lucida Sans Unicode,Tahoma");
            PointerReleased += CustomRichEditBox_PointerReleased;
            SelectionChanged += CustomRichEditBox_SelectionChanged;
            //SelectionChanging += CustomRichEditBox_SelectionChanging;
            //PreviousSelection = "0,0,";
            //KeyDown += CustomRichEditBox_KeyDown;
            //KeyUp += CustomRichEditBox_KeyUp;
            //PreviewKeyDown += CustomRichEditBox_PreviewKeyDown;
            // https://stackoverflow.com/ai/search/16916
            //Background = new SolidColorBrush(Color.FromArgb(255,0xF9,0xF8, 0xbd)); // "#F9F8BD"
            //PointerEntered += (sender, e) => e.Handled = true;
            //Style = (Style)Application.Current.Resources["CustomRichEditBoxStyle"];
        }

        private void CustomRichEditBox_SelectionChanging(RichEditBox sender, RichEditBoxSelectionChangingEventArgs args)
        {
            //var blueback = Windows.UI.Color.FromArgb(0x00, 0x55, 0x76, 0xa2);
            //var whitefront = Windows.UI.Color.FromArgb(0x00, 0xff, 0xff, 0xff);
            //var normal = Windows.UI.Color.FromArgb(0x00, 0xf9, 0xf8, 0xbd);

            //Telemetry t = new();
            //t.SetEnabled(true);
            //CustomRichEditBox me = ((CustomRichEditBox)sender);
            //ITextSelection selection = me.Document.Selection;
            //selection.GetText(TextGetOptions.None, out string text);
            //t.Transmit("me.Name=",me.Name,"Text=",text);
            //selection.SelectOrDefault(x => x);
            //int caretPosition = selection.StartPosition;
            //int start = selection.StartPosition;
            //int end = selection.EndPosition;
            //if (end != start)
            //{
            //    var bc = selection.CharacterFormat.BackgroundColor;
            //    me.PreviousSelection = $"{start},{end},{bc.A}-{bc.R}-{bc.G}-{bc.B}";
            //    bc = blueback;
            //}
        }

        private void CustomRichEditBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            CoreVirtualKeyStates ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            CoreVirtualKeyStates shiftState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
            bool isCtrlPressed = ctrlState.HasFlag(CoreVirtualKeyStates.Down);
            bool isShiftPressed = shiftState.HasFlag(CoreVirtualKeyStates.Locked);

            CustomRichEditBox me = (CustomRichEditBox)sender;
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit("e.Key=", e.Key, "e.KeyStatus.ScanCode=", e.KeyStatus.ScanCode, "ctrlState=", ctrlState, "shiftState=", shiftState);
            base.OnPreviewKeyDown(e);
        }

        private void CustomRichEditBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            CoreVirtualKeyStates ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            CoreVirtualKeyStates shiftState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
            bool isCtrlPressed = ctrlState.HasFlag(CoreVirtualKeyStates.Down);
            bool isShiftPressed = shiftState.HasFlag(CoreVirtualKeyStates.Locked);

            CustomRichEditBox me = (CustomRichEditBox)sender;
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit("e.Key=", e.Key, "e.KeyStatus.ScanCode=", e.KeyStatus.ScanCode, "ctrlState=", ctrlState, "shiftState=", shiftState);
            base.OnKeyUp(e);
        }

        private void CustomRichEditBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CoreVirtualKeyStates ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            CoreVirtualKeyStates shiftState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
            bool isCtrlPressed = ctrlState.HasFlag(CoreVirtualKeyStates.Down);
            bool isShiftPressed = shiftState.HasFlag(CoreVirtualKeyStates.Locked);

            //Window curWin = Window.Current;
            //CoreWindow corWin = curWin.CoreWindow;
            //CoreVirtualKeyStates insertState = corWin.GetKeyState(VirtualKey.Insert);

            var insertState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Insert);
            var overwriteMode = insertState.HasFlag(CoreVirtualKeyStates.Locked);

            CustomRichEditBox me = (CustomRichEditBox)sender;
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit("e.Key=", e.Key, "e.KeyStatus.ScanCode=", e.KeyStatus.ScanCode, "ctrlState=", ctrlState, "shiftState=", shiftState, "insertState=", insertState, "overwriteMode=", overwriteMode);
            //base.OnKeyDown(e);
        }

        private void CustomRichEditBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var blueback = Windows.UI.Color.FromArgb(0x00,0x55,0x76,0xa2);
            var whitefront = Windows.UI.Color.FromArgb(0x00, 0xff, 0xff, 0xff);
            var normal = Windows.UI.Color.FromArgb(0x00, 0xf9, 0xf8, 0xbd);

            Telemetry t = new();
            t.SetEnabled(true);
            CustomRichEditBox me = ((CustomRichEditBox)sender);
            ITextSelection selection = me.Document.Selection;
            selection.GetText(TextGetOptions.None, out string text);
            t.Transmit(text);
            selection.SelectOrDefault(x => x);
            int caretPosition = selection.StartPosition;
            int start = selection.StartPosition;
            int end = selection.EndPosition;
            t.Transmit("start=", start, "end=", end);
            if (start != end)
            {
                //if (me.PreviousSelection != "0,0," )
                //{
                //    var parts = me.PreviousSelection.Split([',']);
                //    selection.StartPosition = int.Parse(parts[0]);
                //    selection.EndPosition = int.Parse(parts[1]);
                //    var argb = parts[2].Split(['-']).Select(e => byte.Parse(e)).ToArray();
                //    selection.CharacterFormat.BackgroundColor = Color.FromArgb(argb[0], argb[1], argb[2], argb[3]);
                //    me.PreviousSelection = "0,0,";
                //}
                //t.Transmit("me.Tag=", me.PreviousSelection);
                //var bc = selection.CharacterFormat.BackgroundColor;
                //me.PreviousSelection = $"{start},{end},{bc.A}-{bc.R}-{bc.G}-{bc.B}";
                //bc = blueback;
                
                // selection.CharacterFormat.BackgroundColor = highlight; // FIXME. Keep the last selection and reset it when here again. 
            }
        }

        private void CustomRichEditBox_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            t.Transmit(((RichEditBox)sender).Name, e.GetType().FullName);
            base.OnPointerReleased(e);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            Telemetry t = new();
            t.SetEnabled(true);
            CoreVirtualKeyStates ctrlState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
            CoreVirtualKeyStates shiftState = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
            bool isCtrlPressed = ctrlState.HasFlag(CoreVirtualKeyStates.Down);
            bool isShiftPressed = shiftState.HasFlag(CoreVirtualKeyStates.Locked);

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
            if (e.Key == VirtualKey.Tab && isCtrlPressed)
            {
                int direction = ctrlState.ToString().Contains("Locked") ? -1 : 1;
                t.Transmit("e.Key=", e.Key, "ctrlState=", ctrlState, "shiftState=", shiftState, "isCtrlPressed=", isCtrlPressed, "isShiftPressed=", isShiftPressed);
                e.Handled = true;
                return;
            }
            if (e.Key == VirtualKey.Tab)
            {
                t.Transmit("e.Key=", e.Key, "ctrlState=", ctrlState, "shiftState=", shiftState, "isCtrlPressed=", isCtrlPressed, "isShiftPressed=", isShiftPressed);
                if (!isShiftPressed)
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
