using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
//using Windows.UI.Xaml;
using LanguageConfigurationStructure = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>>;


namespace PelotonIDE.Presentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TranslatePage : Page
    {

        LanguageConfigurationStructure? LanguageSettings = new();
        string? InterfaceLanguageName = "English";
        public TranslatePage()
        {
            this.InitializeComponent();

            FillLanguagesIntoList(sourceLanguageList);
            FillLanguagesIntoList(targetLanguageList);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (MainToTranslateParams)e.Parameter;

            parameters.CustomREB.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string selectedText);
            sourceText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedText);

            sourceLanguageList.SelectedIndex = (int)parameters.LanguageID;
        }

        private static async Task<LanguageConfigurationStructure?> GetLanguageConfiguration()
        {
            var languageConfig = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\LanguageConfiguration.json"));
            string languageConfigString = File.ReadAllText(languageConfig.Path);
            return JsonConvert.DeserializeObject<LanguageConfigurationStructure>(languageConfigString);
        }

        private async void FillLanguagesIntoList(ListBox listBox)
        {

            LanguageSettings = await GetLanguageConfiguration();

            if (InterfaceLanguageName == null || !LanguageSettings.ContainsKey(InterfaceLanguageName))
            {
                return;
            }

            // what is current language?
            var globals = LanguageSettings[InterfaceLanguageName]["GLOBAL"];
            var count = LanguageSettings.Keys.Count;
            for (var i = 0; i < count; i++)
            {
                var names = from lang in LanguageSettings.Keys
                            where LanguageSettings.ContainsKey(lang) && LanguageSettings[lang]["GLOBAL"]["ID"] == i.ToString()
                            let name = LanguageSettings[lang]["GLOBAL"]["Name"]
                            select name;
                if (names.Any())
                {
                    ListBoxItem listBoxItem = new()
                    {
                        Content = globals[$"{100 + i + 1}"],
                        Name = names.First()  //languageJson[key]["GLOBAL"]["ID"]
                    };
                    listBox.Items.Add(listBoxItem);
                }
            }

            if (listBox.Name == "sourceLanguageList")
            {
                for (int i = 0; i < sourceLanguageList.Items.Count(); i++)
                {
                    if (sourceLanguageList.SelectedIndex != i)
                    {
                        (sourceLanguageList.Items[i] as ListBoxItem).IsEnabled = false;
                    }
                }
            }
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (targetLanguageList.SelectedIndex == -1)
            {
                ContentDialog dialog = new()
                {
                    XamlRoot = this.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Target Language Not Selected",
                    PrimaryButtonText = "OK",
                };
                var result = await dialog.ShowAsync();
            }
            else
            {
                /*TranslateToMainParams parameters = new TranslateToMainParams()
                {
                    selectedLangauge = targetLanguageList.SelectedIndex,
                    translatedREB = targetText

                };*/
                NavigationData nd = new()
                {
                    Source = "TranslatePage",
                    KVPs = new() {
                        { "TargetLanguage" , targetLanguageList.SelectedIndex },
                        { "TargetText" ,  targetText}
                    }
                };
                Frame.Navigate(typeof(MainPage), nd);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null);
        }

        private void targetLanguageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
            targetText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedLanguage.Name);
        }
    }
}
