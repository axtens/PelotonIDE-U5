using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.System;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private void RichEditBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            SolidColorBrush Black = new(Colors.Black);
            SolidColorBrush LightGrey = new(Colors.LightGray);

            Debug.WriteLine($"{e.Key}");
            if (e.Key == VirtualKey.CapitalLock)
            {
                CAPS.Text = "CAPS";
                CAPS.Foreground = Console.CapsLock ? Black : (Brush)LightGrey;
            }
            if (e.Key == VirtualKey.NumberKeyLock)
            {
                NUM.Text = "NUM";
                NUM.Foreground = Console.NumberLock ? Black : (Brush)LightGrey;
            }
            if (e.Key == VirtualKey.Scroll)
            {
            }
            if (e.Key == VirtualKey.Insert)
            {
            }
            if (tabControl.Content is CustomRichEditBox currentRichEditBox && !e.KeyStatus.IsExtendedKey && e.Key != VirtualKey.Control)
            {
                currentRichEditBox.IsDirty = true;
            }
        }

        private void CustomREBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
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
                    if (text[i] == '\v' || text[i] == '\r')
                    {
                        lineNumber++;
                        charNumber = 0;
                    }
                }
                int charsSinceLastLineBreak = 1;
                for (int i = caretPosition - 1; i >= 0; i--)
                {
                    if (text[i] == '\v' || text[i] == '\r')
                    {
                        break;
                    }
                    charsSinceLastLineBreak++;
                }
                cursorPosition.Text = "Line " + lineNumber + ", Char " + charNumber;
            }
        }
    }
}
