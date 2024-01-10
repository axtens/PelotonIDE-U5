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
using Uno;
using System.ComponentModel.Design;
using Uno.Extensions;

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
                var tabLanguageName = parameters.KVPs["TabLanguageName"].ToString();
                var tabLanguageId = (int)(long)parameters.KVPs["TabLanguageID"];
                var interfaceLanguageName = parameters.KVPs["InterfaceLanguageName"].ToString();
                var interfaceLanguageID = (int)(long)parameters.KVPs["InterfaceLanguageID"];

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
                if (selectedText.Contains("</#>"))
                {
                    chkVarLengthFrom.IsChecked = true;
                }

                //var index = (int)(long)parameters.KVPs["InterpreterLanguage"];
                sourceLanguageList.SelectedIndex = tabLanguageId;
                //sourceLanguageList.Focus(FocusState.Keyboard);
                (sourceLanguageList.ItemContainerGenerator.ContainerFromIndex(tabLanguageId) as ListBoxItem)?.Focus(FocusState.Programmatic);

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
                        { "TargetVariableLength", chkVarLengthTo.IsChecked ?? false},
                        { "TargetPadOutCode", chkSpaceOut.IsChecked ?? false},
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
            // ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
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

        /*private FlowDirection GetFlowDirection(string name) // REMOVE. RTL is not this
        {
            var thisLanguage = Langs[name];
            var thisGlobal = thisLanguage["GLOBAL"];
            var thisRTL = bool.Parse(thisGlobal["RTL"] ?? "false");
            return thisRTL ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }*/

        private void TargetLanguageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
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

            if (variableSource && sourcePlexVariable.Any())
            {
                return ProcessVariableToFixedOrVariable(code, source, target, spaced);
            }
            else
            {
                return ProcessFixedToFixedOrVariableWithOrWithoutSpace(code, source, target, spaced);
            }
        }

        private static string ProcessVariableToFixedOrVariable(string code, Plex source, Plex target, bool spaced)
        {
            var variableLengthWords = from variableLengthWord in source.OpcodesByKey.Keys orderby -variableLengthWord.Length select variableLengthWord;

            var fixedLengthEquivalents = (from word in variableLengthWords
                                          let sourceop = source.OpcodesByKey[word]
                                          let targetword = target.OpcodesByValue[sourceop]
                                          select (word, targetword)).ToDictionary(x => x.Item1, x => x.Item2);

            var codeBlocks = GetCodeBlocks(code); // in reverse order

            foreach (var block in codeBlocks)
            {
                var codeChunk = block.Value;
                foreach (var vlw in variableLengthWords)
                {
                    if (!codeChunk.Contains(vlw, StringComparison.CurrentCulture)) continue;
                    if (spaced)
                    {
                        if (codeChunk.IndexOf(vlw) + vlw.Length < codeChunk.Length && codeChunk.Substring(codeChunk.IndexOf(vlw) + vlw.Length, 1) == " ")
                        {
                            codeChunk = codeChunk.Replace(vlw + " ", fixedLengthEquivalents[vlw]);
                        }
                        else
                        {
                            codeChunk = codeChunk.Replace(vlw, fixedLengthEquivalents[vlw]);
                        }
                    }
                    else
                    {
                        codeChunk = codeChunk.Replace(vlw, fixedLengthEquivalents[vlw]);
                    }
                }
                code = code.Remove(block.Index, block.Length);
                code = code.Insert(block.Index, codeChunk);
            }
            /*
            var pattern = PelotonVariableSpacedPattern();
            MatchCollection matches = pattern.Matches(code);
            for (int mi = matches.Count - 1; mi >= 0; mi--)
            {
                for (int i = matches[mi].Groups[1].Captures.Count - 1; i >= 0; i--)
                {
                    var capture = matches[mi].Groups[1].Captures[i];
                    foreach (var variableLengthWord in variableLengthWords)
                    {
                        if (capture.Value.Contains(variableLengthWord))
                        {
                            if (source.OpcodesByKey.TryGetValue(variableLengthWord, out long opcode))
                            {
                                if (target.OpcodesByValue.TryGetValue(opcode, out string? value))
                                {
                                    var newKey = value;
                                    int idx = capture.Index;
                                    int len = capture.Length;
                                    string captured = code.Substring(idx, len);
                                    
                                    string[] parts = captured.Split(" ");
                                    for (int part = parts.Length - 1;part >= 0 ; part--)
                                    {
                                        if (parts[part].Contains(variableLengthWord))
                                            parts[part] = parts[part].Replace(variableLengthWord, newKey);
                                    }
                                    var reCaptured = spaced ? parts.JoinBy(" ") : parts.JoinBy("");
                                    code = code.Remove(idx, len);
                                    code = code.Insert(idx, reCaptured);
                                }
                            }
                        }
                        //var capval = capture.Value;
                    }
                }
            }*/
            return code;
        }

        private static List<Capture> GetCodeBlocks(string code)
        {
            var codeBlocks = new List<Capture>();
            var pattern = PelotonVariableSpacedPattern();
            MatchCollection matches = pattern.Matches(code);
            for (int mi = matches.Count - 1; mi >= 0; mi--)
            {
                for (int i = matches[mi].Groups[1].Captures.Count - 1; i >= 0; i--)
                {
                    Capture cap = matches[mi].Groups[1].Captures[i];
                    if (cap == null) continue;
                    codeBlocks.Add(cap);
                }
            }
            return codeBlocks;
        }

        private static string ProcessFixedToFixedOrVariableWithOrWithoutSpace(string buff, Plex sourcePlex, Plex targetPlex, bool spaceOut)
        {
            var pattern = PelotonFixedSpacedPattern();
            MatchCollection matches = pattern.Matches(buff);
            for (int mi = matches.Count - 1; mi >= 0; mi--)
            {
                //var max = matches[mi].Groups[2].Captures.Count - 1;
                for (int i = matches[mi].Groups[1].Captures.Count - 1; i >= 0; i--)
                {
                    var capture = matches[mi].Groups[1].Captures[i];
                    var key = capture.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Trim();
                    if (sourcePlex.OpcodesByKey.TryGetValue(key, out long opcode))
                    {
                        if (targetPlex.OpcodesByValue.TryGetValue(opcode, out string? value))
                        {
                            var newKey = value;
                            var next = buff.Substring(capture.Index + capture.Length, 1);
                            buff = buff.Remove(capture.Index, capture.Length)
                                .Insert(capture.Index, newKey + ((spaceOut && next != ">") ? " " : ""));
                        }
                    }
                }
                // var tag = matches[mi].Groups[1].Captures[0];
            }
            return targetPlex.Meta.Variable ? buff.Replace("<@ ", "<# ").Replace("</@>", "</#>") : buff;
        }

        private void SourceLanguageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListBoxItem? selectedLanguage = sourceLanguageList.SelectedItem as ListBoxItem;
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

        [GeneratedRegex(@"<# (.+?)>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "en-AU")]
        private static partial Regex PelotonVariableSpacedPattern();

        [GeneratedRegex(@"<@ (...\s{0,1})+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "en-AU")]
        private static partial Regex PelotonFixedSpacedPattern();

        private void ChkVarLengthFrom_Click(object sender, RoutedEventArgs e)
        {
            if (targetLanguageList.SelectedItem == null) return;
            if (sourceLanguageList.SelectedItem == null) return;
            //ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
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
            //ListBoxItem? selectedLanguage = targetLanguageList.SelectedItem as ListBoxItem;
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
