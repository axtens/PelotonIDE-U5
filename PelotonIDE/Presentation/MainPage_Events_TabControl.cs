using Microsoft.UI.Xaml.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private void TabControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (tabControl.SelectedItem != null)
            {
                CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
                tabControl.Content = _richEditBoxes[navigationViewItem.Tag];
                if (navigationViewItem.TabSettingsDict == null)
                {
                    return;
                }
                string currentLanguageName = GetLanguageNameOfCurrentTab(navigationViewItem.TabSettingsDict);
                if (languageName.Text != currentLanguageName)
                {
                    languageName.Text = currentLanguageName;
                }
                UpdateCommandLineInStatusBar();
                UpdateStatusBarFromInFocusTab();
            }
        }
        private void TabControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            telem.Transmit(((CustomTabItem)sender).Name, e.GetType().FullName);
        }

        private void CustomTabItem_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            telem.Transmit(((CustomTabItem)sender).Name, e.GetType().FullName);

        }

        private async void TabControl_RightTapped(object sender, RightTappedRoutedEventArgs e) // fires first for all tabs other than tab1
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            CustomTabItem selectedItem = (CustomTabItem)((NavigationView)sender).SelectedItem;

            CustomRichEditBox currentRichEditBox = _richEditBoxes[selectedItem.Tag];
            // var t1 = tab1;
            if (currentRichEditBox.IsDirty)
            {
                if (!await AreYouSureToClose()) return;
            }
            _richEditBoxes.Remove(selectedItem.Tag);
            tabControl.MenuItems.Remove(selectedItem);
            if (tabControl.MenuItems.Count > 0)
            {
                tabControl.SelectedItem = tabControl.MenuItems[tabControl.MenuItems.Count - 1];
            }
            else
            {
                tabControl.Content = null;
                tabControl.SelectedItem = null;
            }
            UpdateCommandLineInStatusBar();
            UpdateStatusBarFromInFocusTab();
        }

        private void CustomTabItem_RightTapped(object sender, RightTappedRoutedEventArgs e) // fires on tab1 then fires TabControl_RightTapped
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            telem.Transmit(((CustomTabItem)sender).Name, e.GetType().FullName);

        }
    }
}
