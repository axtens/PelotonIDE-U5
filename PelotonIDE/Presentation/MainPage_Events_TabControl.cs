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
            }
        }

    }
}
