using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
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
        }
    }
}
