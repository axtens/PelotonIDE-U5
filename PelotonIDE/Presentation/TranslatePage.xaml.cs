using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
//using Windows.UI.Xaml;
using LanguageConfigurationStructure = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>>;
using LanguageConfigurationStructureSelection =
    System.Collections.Generic.Dictionary<string,
        System.Collections.Generic.Dictionary<string, string>>;
using Microsoft.UI.Text;
using static System.Net.Mime.MediaTypeNames;

namespace PelotonIDE.Presentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TranslatePage : Page
    {
        //LanguageConfigurationStructure? LanguageSettings = [];
        //readonly string? InterfaceLanguageName = "English";
        List<Plex>? Plexes;

        LanguageConfigurationStructure? Langs;

        public TranslatePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (NavigationData)e.Parameter;

            if (parameters.Source == "MainPage")
            {
                Langs = (LanguageConfigurationStructure)parameters.KVPs["Languages"];
                var interfaceLanguageName = parameters.KVPs["InterfaceLanguageName"].ToString();
                //var interfaceLanguageID = (int)(long)parameters.KVPs["InterfaceLanguageID"];

                FillLanguagesIntoList(Langs, interfaceLanguageName!, sourceLanguageList);
                FillLanguagesIntoList(Langs, interfaceLanguageName!, targetLanguageList);

                var language = Langs[interfaceLanguageName!];
                cmdCancel.Content = language["frmMain"]["cmdCancel"];
                cmdSaveMemory.Content = language["frmMain"]["cmdSaveMemory"];
                chkSpaceOut.Content = language["frmMain"]["chkSpaceOut"];
                chkVarLengthFrom.Content = language["frmMain"]["chkVarLengthFrom"];
                chkVarLengthTo.Content = language["frmMain"]["chkVarLengthTo"];

                var rtb = ((CustomRichEditBox)parameters.KVPs["RichEditBox"]);
                rtb.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string selectedText);
                sourceText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedText);

                var index = (int)(long)parameters.KVPs["InterpreterLanguage"];
                sourceLanguageList.SelectedIndex = index;
                //sourceLanguageList.Focus(FocusState.Keyboard);
                (sourceLanguageList.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem)?.Focus(FocusState.Programmatic);

                this.Plexes = GetAllPlexes();
            }
        }

        private List<Plex>? GetAllPlexes()
        {
            List<Plex> list = [];
            foreach (var file in Directory.GetFiles(@"c:\peloton\bin\lexers", "*.lex"))
            {
                var data = File.ReadAllBytes(file);
                using MemoryStream stream = new(data);
                using BsonDataReader reader = new(stream);
                JsonSerializer serializer = new();
                Plex? p = serializer.Deserialize<Plex>(reader);
                list.Add(p!);
            }

            return list;
        }

        private static void FillLanguagesIntoList(LanguageConfigurationStructure languages, string interfaceLanguageName, ListBox listBox)
        {
            if (languages is null)
            {
                throw new ArgumentNullException(nameof(languages));
            }
            // what is current language?
            var globals = languages[interfaceLanguageName]["GLOBAL"];
            var count = languages.Keys.Count;
            for (var i = 0; i < count; i++)
            {
                var names = from lang in languages.Keys
                            where languages.ContainsKey(lang) && languages[lang]["GLOBAL"]["ID"] == i.ToString()
                            let name = languages[lang]["GLOBAL"]["Name"]
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
                _ = await dialog.ShowAsync();
            }
            else
            {
                targetText.Document.GetText(TextGetOptions.None, out string txt);
                Frame.Navigate(typeof(MainPage), new NavigationData()
                {
                    Source = "TranslatePage",
                    KVPs = new() {
                        { "TargetLanguage" , (long)targetLanguageList.SelectedIndex },
                        { "TargetText" ,  txt}
                    }
                });
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null);
        }

        private void ChkSpaceOut_Click(object sender, RoutedEventArgs e)
        {
            if (targetLanguageList.SelectedItem == null) return;
            if (sourceLanguageList.SelectedItem == null) return;
            ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
            // sourceText.FlowDirection = GetFlowDirection(((ListBoxItem)sourceLanguageList.SelectedItem).Name);

            //targetText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedLanguage.Name);
            sourceText.Document.GetText(TextGetOptions.None, out string code);
            targetText
                .Document
                .SetText(
                    TextSetOptions.None,
                    TranslateCode(code, ((ListBoxItem)sourceLanguageList.SelectedItem).Name, ((ListBoxItem)targetLanguageList.SelectedItem).Name));
            // targetText.FlowDirection = GetFlowDirection(((ListBoxItem)targetLanguageList.SelectedItem).Name);
        }

        private FlowDirection GetFlowDirection(string name)
        {
            var thisLanguage = Langs[name];
            var thisGlobal = thisLanguage["GLOBAL"];
            var thisRTL = bool.Parse(thisGlobal["RTL"] ?? "false");
            return thisRTL ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private void TargetLanguageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
            //targetText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedLanguage.Name);
            // sourceText.FlowDirection = GetFlowDirection(((ListBoxItem)sourceLanguageList.SelectedItem).Name);
            sourceText.Document.GetText(TextGetOptions.None, out string code);
            targetText
                .Document
                .SetText(
                    TextSetOptions.None,
                    TranslateCode(code, ((ListBoxItem)sourceLanguageList.SelectedItem).Name, ((ListBoxItem)targetLanguageList.SelectedItem).Name));
            // targetText.FlowDirection = GetFlowDirection(((ListBoxItem)targetLanguageList.SelectedItem).Name);

        }

        private string TranslateCode(string code, string sourceLanguageName, string targetLanguageName)
        {
            var sourcePlexVariable = from plex in this.Plexes where plex.Meta.Language == sourceLanguageName.Replace(" ", "") && plex.Meta.Variable select plex;
            var targetPlexVariable = from plex in this.Plexes where plex.Meta.Language == targetLanguageName.Replace(" ", "") && plex.Meta.Variable select plex;

            var sourcePlexFixed = from plex in this.Plexes where plex.Meta.Language == sourceLanguageName.Replace(" ", "") && !plex.Meta.Variable select plex;
            var targetPlexFixed = from plex in this.Plexes where plex.Meta.Language == targetLanguageName.Replace(" ", "") && !plex.Meta.Variable select plex;

            var variableTarget = chkVarLengthTo.IsChecked ?? false;
            var variableSource = chkVarLengthFrom.IsChecked ?? false;

            Plex source = new();
            Plex target = new();

            source = variableSource && sourcePlexVariable.Any() ? sourcePlexVariable.First() : sourcePlexFixed.First();
            target = variableTarget && targetPlexVariable.Any() ? targetPlexVariable.First() : targetPlexFixed.First();

            var spaced = chkSpaceOut.IsChecked ?? false;
            return TranslatePage.ProcessCode(code, source, target, spaced);
        }

        private static string ProcessCode(string buff, Plex sourcePlex, Plex targetPlex, bool spaceOut)
        {
            var pattern = PelotonRegex();
            MatchCollection matches = pattern.Matches(buff);
            for (int mi = matches.Count - 1; mi >= 0; mi--)
            {
                //var max = matches[mi].Groups[2].Captures.Count - 1;
                for (int i = matches[mi].Groups[1].Captures.Count - 1; i >= 0; i--)
                {
                    var capture = matches[mi].Groups[1].Captures[i];
                    if (sourcePlex.OpcodesByKey.ContainsKey(capture.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture)))
                    {
                        var opcode = sourcePlex.OpcodesByKey[capture.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture)];
                        if (targetPlex.OpcodesByValue.ContainsKey(opcode))
                        {
                            var newKey = targetPlex.OpcodesByValue[opcode];
                            var next = buff.Substring(capture.Index + capture.Length, 1);
                            buff = buff.Remove(capture.Index, capture.Length)
                                .Insert(capture.Index, newKey + ((spaceOut && next != ">") ? " " : ""));
                        }
                    }
                }
                // var tag = matches[mi].Groups[1].Captures[0];
            }
            return buff;
        }

        private void SourceLanguageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem? selectedLanguage = sourceLanguageList.SelectedItem as ListBoxItem;
            //sourceText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedLanguage.Name);
            // sourceText.FlowDirection = GetFlowDirection(((ListBoxItem)sourceLanguageList.SelectedItem).Name);
            sourceText.Document.GetText(TextGetOptions.None, out string code);
            if ((ListBoxItem)targetLanguageList.SelectedItem != null)
            {
                targetText
                .Document
                .SetText(
                    TextSetOptions.None,
                    TranslateCode(code, ((ListBoxItem)sourceLanguageList.SelectedItem).Name, ((ListBoxItem)targetLanguageList.SelectedItem).Name));
                // targetText.FlowDirection = GetFlowDirection(((ListBoxItem)targetLanguageList.SelectedItem).Name);
            }
        }

        [GeneratedRegex("<(?:@|#) (...)+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "en-AU")]
        private static partial Regex PelotonRegex();

        private void ChkVarLengthFrom_Click(object sender, RoutedEventArgs e)
        {
            if (targetLanguageList.SelectedItem == null) return;
            if (sourceLanguageList.SelectedItem == null) return;
            ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
            //targetText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedLanguage.Name);
            // sourceText.FlowDirection = GetFlowDirection(((ListBoxItem)sourceLanguageList.SelectedItem).Name);
            sourceText.Document.GetText(TextGetOptions.None, out string code);
            targetText
                .Document
                .SetText(
                    TextSetOptions.None,
                    TranslateCode(code, ((ListBoxItem)sourceLanguageList.SelectedItem).Name, ((ListBoxItem)targetLanguageList.SelectedItem).Name));
            // targetText.FlowDirection = GetFlowDirection(((ListBoxItem)targetLanguageList.SelectedItem).Name);
        }

        private void ChkVarLengthTo_Click(object sender, RoutedEventArgs e)
        {
            if (targetLanguageList.SelectedItem == null) return;
            if (sourceLanguageList.SelectedItem == null) return;
            ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
            //targetText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedLanguage.Name);
            // sourceText.FlowDirection = GetFlowDirection(((ListBoxItem)sourceLanguageList.SelectedItem).Name);
            sourceText.Document.GetText(TextGetOptions.None, out string code);
            targetText
                .Document
                .SetText(
                    TextSetOptions.None,
                    TranslateCode(code, ((ListBoxItem)sourceLanguageList.SelectedItem).Name, ((ListBoxItem)targetLanguageList.SelectedItem).Name));
            // targetText.FlowDirection = GetFlowDirection(((ListBoxItem)targetLanguageList.SelectedItem).Name);
        }
    }
}
