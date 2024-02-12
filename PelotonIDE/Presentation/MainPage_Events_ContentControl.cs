using Microsoft.UI;
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
        private void ContentControl_LanguageName_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ContentControl me = (ContentControl)sender;

            object prevContent = me.Content;

            MenuFlyout mf = new();

            string interfaceLanguageName = Type_1_GetVirtualRegistry<string>("InterfaceLanguageName");
            Dictionary<string, string> globals = LanguageSettings[Type_1_GetVirtualRegistry<string>("InterfaceLanguageName")]["GLOBAL"];
            int count = LanguageSettings.Keys.Count;
            for (int i = 0; i < count; i++)
            {
                IEnumerable<string> names = from lang in LanguageSettings.Keys
                                            where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                                            let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                                            select name;
                if (names.Any())
                {
                    MenuFlyoutItem menuFlyoutItem = new()
                    {
                        Text = globals[$"{100 + i + 1}"],
                        Name = names.First(),
                        Foreground = names.First() ==Type_1_GetVirtualRegistry<string>("InterpreterLanguageName") ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black),
                        Background = names.First() == Type_1_GetVirtualRegistry<string>("InterpreterLanguageName") ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White),
                        Tag = new Dictionary<string, object>()
                        {
                            {"MenuFlyout",mf },
                            {"ContentControlPreviousContent",prevContent },
                            {"ContentControl" ,me}
                        }
                    };
                    menuFlyoutItem.Click += ContentControl_Click; // this has to reset the cell to its original value
                    mf.Items.Add(menuFlyoutItem);
                }
            }
            FrameworkElement? senderElement = sender as FrameworkElement;

            //the code can show the flyout in your mouse click 
            mf.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));

            //me.Content = mfsu;
        }


        private void ContentControl_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem me = (MenuFlyoutItem)sender;
            string name = me.Name;
            languageName.Text = me.Text;
            // change the current tab to that lang but don't change the pertab settings
            Dictionary<string, string> globals = LanguageSettings[name]["GLOBAL"];
            string id = globals["ID"];

            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            //CustomRichEditBox currentRichEditBox = _richEditBoxes[navigationViewItem.Tag];

            Dictionary<string, Dictionary<string, object>>? currentTabSettings = navigationViewItem.TabSettingsDict;
            currentTabSettings["Language"]["Defined"] = true;
            currentTabSettings["Language"]["Value"] = long.Parse(id);

            //ChangeHighlightOfMenuBarForLanguage((MenuBarItem)me, name);
            UpdateLanguageInContextualMenu(me, me.Text, name);
            if (me.Tag is Dictionary<string, object> parent)
            {
                if (parent.ContainsKey("ContentControl") && parent.ContainsKey("ContentControlPreviousContent"))
                    parent["ContentControl"] = parent["ContentControlPreviousContent"];
                // ((MenuFlyoutSubItem)parent["MenuFlyoutSubItem"]) ;
            }
            UpdateCommandLineInStatusBar();
        }

        private void UpdateLanguageInContextualMenu(MenuFlyoutItem me, string internationalizedName, string name)
        {
            if (me.Tag is Dictionary<string, object> parent)
            {
                IList<MenuFlyoutItemBase> subMenus = ((MenuFlyout)parent["MenuFlyout"]).Items; //  from menu in ((MenuFlyoutSubItem)me.Tag).Items select menu;
                if (subMenus != null)
                {
                    foreach (MenuFlyoutItemBase item in subMenus)
                    {
                        if (item.Name == name)
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
        }

        private void ContentControl_FixedVariable_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }
    }
}
