using Microsoft.UI.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private void MnuTranslate_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            currentRichEditBox.Document.GetText(TextGetOptions.None, out string? text);
            if (!navigationViewItem.TabSettingsDict.TryGetValue("Language", out Dictionary<string, object>? ldict))
            {
                return;
            }
            long tabLangId = (long)ldict["Value"];

            if (!navigationViewItem.TabSettingsDict.TryGetValue("Quietude", out Dictionary<string, object>? qdict))
            {
                return;
            }
            long quietude = (long)qdict["Value"];

            IEnumerable<string> tabLangName = from lang in LanguageSettings where long.Parse(lang.Value["GLOBAL"]["ID"]) == tabLangId select lang.Key;
            string? savedFilePath = navigationViewItem.SavedFilePath != null ? Path.GetDirectoryName(navigationViewItem.SavedFilePath.Path) : null;
            string? mostRecentPickedFilePath;
            if (Type_1_GetVirtualRegistry("MostRecentPickedFilePath") != null)
            {
                mostRecentPickedFilePath = Type_1_GetVirtualRegistry("MostRecentPickedFilePath").ToString();
            }
            else
            {
                mostRecentPickedFilePath = (string?)string.Empty;
            }

            Frame.Navigate(typeof(TranslatePage), new NavigationData()
            {
                Source = "MainPage",
                KVPs = new()
                {
                    { "RichEditBox", (CustomRichEditBox)tabControl!.Content },
                    { "TabLanguageID",tabLangId },
                    { "TabLanguageName", tabLangName.First() },
                    { "TabVariableLength", text.Contains("<# ") && text.Contains("</#>") },
                    { "InterpreterLanguage",  InterpreterLanguageID},
                    { "InterfaceLanguageID", InterfaceLanguageID},
                    { "InterfaceLanguageName",InterfaceLanguageName! },
                    { "Languages", LanguageSettings! },
                    { "SourceSpec", navigationViewItem.SavedFilePath == null ? navigationViewItem.Content : navigationViewItem.SavedFilePath.Path},
                    { "SourcePath", $"{savedFilePath ?? mostRecentPickedFilePath ?? Scripts}" },
                    { "Quietude", quietude },
                    { "Plexes", Plexes! }
                }
            });
        }


        private void ToggleOutputButton_Click(object sender, RoutedEventArgs e)
        {
            outputPanel.Visibility = outputPanelShowing ? Visibility.Collapsed : Visibility.Visible;
            outputPanelShowing = !outputPanelShowing;
        }

        private void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];
            currentRichEditBox.Document.GetText(TextGetOptions.UseCrlf, out string selectedText); // FIXME don't interpret nothig
            selectedText = selectedText.TrimEnd("\r\n");
            if (selectedText.Length > 0)
                ExecuteInterpreter(selectedText);
        }

        private void RunSelectedCodeButton_Click(object sender, RoutedEventArgs e)
        {
            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];

            ITextSelection selection = currentRichEditBox.Document.Selection;

            string selectedText = selection.Text;
            selectedText = selectedText.TrimEnd('\r');
            if (selectedText.Length > 0)
            {
                ExecuteInterpreter(selectedText.Replace("\r", "\r\n")); // FIXME pass in some kind of identifier to connect to the tab
            }
        }
    }
}
