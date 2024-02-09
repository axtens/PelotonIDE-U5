using ClosedXML.Excel;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Office2019.Presentation;

using Microsoft.UI.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

using Windows.Storage;

using Group = System.Text.RegularExpressions.Group;
using LanguageConfigurationStructure = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>>;
using LanguageConfigurationStructureSelection =
    System.Collections.Generic.Dictionary<string,
        System.Collections.Generic.Dictionary<string, string>>;

namespace PelotonIDE.Presentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TranslatePage : Microsoft.UI.Xaml.Controls.Page
    {
        //List<PropertyBag>? OldPlexes;

        LanguageConfigurationStructure? Langs;
        string? SourcePath { get; set; }
        string? SourceSpec { get; set; }

        long Quietude { get; set; }
        internal static List<Plex>? Plexes { get; private set; }

        [GeneratedRegex(@"<(?:#|@) (.+?)>(.*?)</(?:#|@)>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "en-AU")]
        private static partial Regex PelotonFullPattern();

        [GeneratedRegex(@"<# (.+?)>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "en-AU")]
        private static partial Regex PelotonVariableSpacedPattern();

        [GeneratedRegex(@"<@ (...\s{0,1})+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "en-AU")]
        private static partial Regex PelotonFixedSpacedPattern();

        public TranslatePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationData parameters = (NavigationData)e.Parameter;

            if (parameters.Source == "MainPage")
            {
                Plexes = (List<Plex>?)parameters.KVPs["Plexes"];
                Langs = (LanguageConfigurationStructure)parameters.KVPs["Languages"];
                //string? tabLanguageName = parameters.KVPs["TabLanguageName"].ToString();
                int tabLanguageId = (int)(long)parameters.KVPs["TabLanguageID"];
                string? interfaceLanguageName = parameters.KVPs["InterfaceLanguageName"].ToString();
                Quietude = (long)parameters.KVPs["Quietude"];
                //int interfaceLanguageID = (int)(long)parameters.KVPs["InterfaceLanguageID"];
                SourcePath = parameters.KVPs["SourcePath"].ToString();
                SourceSpec = parameters.KVPs["SourceSpec"].ToString();

                FillLanguagesIntoList(Langs, interfaceLanguageName!, sourceLanguageList);
                FillLanguagesIntoList(Langs, interfaceLanguageName!, targetLanguageList);

                LanguageConfigurationStructureSelection language = Langs[interfaceLanguageName!];
                cmdCancel.Content = language["frmMain"]["cmdCancel"];
                cmdSaveMemory.Content = language["frmMain"]["cmdSaveMemory"];
                chkSpaceOut.Content = language["frmMain"]["chkSpaceOut"];
                chkVarLengthFrom.Content = language["frmMain"]["chkVarLengthFrom"];
                chkVarLengthTo.Content = language["frmMain"]["chkVarLengthTo"];

                CustomRichEditBox rtb = ((CustomRichEditBox)parameters.KVPs["RichEditBox"]);
                rtb.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string selectedText);
                sourceText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, selectedText);
                if (selectedText.Contains("</#>"))
                {
                    chkVarLengthFrom.IsChecked = true;
                }

                if (ProbablySpacedInstructions(selectedText))
                {
                    chkSpaceIn.IsChecked = true;
                }

                //var index = (int)(long)parameters.KVPs["InterpreterLanguage"];
                sourceLanguageList.SelectedIndex = tabLanguageId;
                //sourceLanguageList.Focus(FocusState.Keyboard);
                (sourceLanguageList.ItemContainerGenerator.ContainerFromIndex(tabLanguageId) as ListBoxItem)?.Focus(FocusState.Programmatic);
                // Plexes = GetAllPlexes();
                // this.OldPlexes = GetAllOldPlexes();
            }
        }

        //private List<PropertyBag> GetAllOldPlexes()
        //{
        //    List<PropertyBag> result = [];
        //    foreach (string file in Directory.GetFiles(@"c:\protium\bin\lexers", "*.plx"))
        //    {
        //        PropertyBag pb = new();
        //        pb.LoadBagFromFile(file);
        //        result.Add(pb);
        //    }
        //    return result;
        //}

        private bool ProbablySpacedInstructions(string selectedText)
        {
            int result = 0;
            Regex pattern = PelotonFullPattern();
            MatchCollection matches = pattern.Matches(selectedText);
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                ReadOnlySpan<char> group = match.Groups[1].ValueSpan;
                var enu = group.EnumerateRunes();
                do
                {
                    if (enu.Current.Value == ' ') result++;
                } while (enu.MoveNext());
            }
            return result > 0;
        }

        private static void FillLanguagesIntoList(LanguageConfigurationStructure languages, string interfaceLanguageName, ListBox listBox)
        {
            if (languages is null)
            {
                throw new ArgumentNullException(nameof(languages));
            }
            // what is current language?
            Dictionary<string, string> globals = languages[interfaceLanguageName]["GLOBAL"];
            for (int i = 0; i < languages.Keys.Count; i++)
            {
                var names = from lang in languages.Keys
                            where languages.ContainsKey(lang) && languages[lang]["GLOBAL"]["ID"] == i.ToString()
                            let name = languages[lang]["GLOBAL"]["Name"]
                            select name;
                if (names.Any())
                {
                    string name = names.First();
                    bool present = LanguageIsPresentInPlexes(name);
                    
                    ListBoxItem listBoxItem = new()
                    {
                        Content = globals[$"{100 + i + 1}"],
                        Name = name,  
                        IsEnabled = present
                    };
                    listBox.Items.Add(listBoxItem);
                }
            }
        }

        private static bool LanguageIsPresentInPlexes(string name) => (from plex in Plexes where plex.Meta.Language == name.Replace(" ", "") select plex).Any();

        private string TranslateCode(string code, string sourceLanguageName, string targetLanguageName)
        {
            MainPage.Track("TranslateCode", "code=", code, "sourceLanguageName=", sourceLanguageName, "targetLanguageName=", targetLanguageName);

            bool variableTarget = chkVarLengthTo.IsChecked ?? false;
            bool variableSource = chkVarLengthFrom.IsChecked ?? false;
            bool fixedTarget = chkVarLengthTo.IsChecked == false;
            bool fixedSource = chkVarLengthFrom.IsChecked == false;
            bool spaced = chkSpaceOut.IsChecked ?? false;

            //PropertyBag englishPBFixed = (from bag 
            //                              in this.OldPlexes 
            //                              where bag.Name == "ENGLISH" && bag.ReadValueAsString("variable") == "False" 
            //                              select bag).First();

            string? sourceName = (sourceLanguageName).Replace(" ", "").ToUpperInvariant();
            string? targetName = (targetLanguageName).Replace(" ", "").ToUpperInvariant();

            
            /*
             * IEnumerable<PropertyBag> sourcePBVariable = from bag
                                                        in this.OldPlexes
                                                        where bag.Name == sourceName + "FULL" 
                                                        select bag;
            IEnumerable<PropertyBag> targetPBVariable = from bag
                                                        in this.OldPlexes
                                                        where bag.Name == targetName + "FULL"
                                                        select bag;
            IEnumerable<PropertyBag> sourcePBFixed = from bag
                                                        in this.OldPlexes
                                                        where bag.Name == sourceName 
                                                        select bag;
            IEnumerable<PropertyBag> targetPBFixed = from bag
                                                        in this.OldPlexes
                                                        where bag.Name == targetName
                                                        select bag;
            */

            Plex englishFixed = (from plex in Plexes where plex.Meta.Language == "English" && !plex.Meta.Variable select plex).First();

            IEnumerable<Plex> sourcePlexVariable = from plex in Plexes where plex.Meta.Language == sourceLanguageName.Replace(" ", "") && plex.Meta.Variable select plex;
            IEnumerable<Plex> targetPlexVariable = from plex in Plexes where plex.Meta.Language == targetLanguageName.Replace(" ", "") && plex.Meta.Variable select plex;

            IEnumerable<Plex> sourcePlexFixed = from plex in Plexes where plex.Meta.Language == sourceLanguageName.Replace(" ", "") && !plex.Meta.Variable select plex;
            IEnumerable<Plex> targetPlexFixed = from plex in Plexes where plex.Meta.Language == targetLanguageName.Replace(" ", "") && !plex.Meta.Variable select plex;


            MainPage.Track("TranslateCode", "variableTarget=", variableTarget, "variableSource=", variableSource, "fixedTarget=", fixedTarget, "fixedSource=", fixedSource, "spaced=", spaced);

            //PropertyBag sourcePB = new();
            //PropertyBag targetPB = new();

            Plex source = new();
            Plex target = new();

            //sourcePB = variableSource && sourcePBVariable.Any() ? sourcePBVariable.First() : sourcePBFixed.First();
            //targetPB = variableTarget && targetPBVariable.Any() ? targetPBVariable.First() : targetPBFixed.First();

            source = variableSource && sourcePlexVariable.Any() ? sourcePlexVariable.First() : sourcePlexFixed.First();
            target = variableTarget && targetPlexVariable.Any() ? targetPlexVariable.First() : targetPlexFixed.First();

            string result = variableSource && sourcePlexVariable.Any()
                ? ProcessVariableToFixedOrVariable(code, source, target, spaced, variableTarget)
                : ProcessFixedToFixedOrVariableWithOrWithoutSpace(code, source, target, spaced, variableTarget);

            //string result = variableSource && sourcePBVariable.Any()
            //    ? ProcessVariableToFixedOrVariablePB(code, sourcePB, targetPB, spaced, variableTarget)
            //    : ProcessFixedToFixedOrVariableWithOrWithoutSpacePB(code, sourcePB, targetPB, spaced, variableTarget);

            string? pathToSource = SourcePath; // Path.GetDirectoryName(SourceSpec);
            string? nameOfSource = Path.GetFileNameWithoutExtension(SourceSpec);
            string? xlsxPath = Path.Combine(pathToSource ?? ".", "p.xlsx");

            MainPage.Track("TranslateCode", "pathToSource=", pathToSource, "nameOfSource=", nameOfSource, "xlsxPath=", xlsxPath);

            bool ok = false;

            (ok, XLWorkbook? workbook) = GetNamedExcelWorkbook(xlsxPath);
            if (!ok) return result;

            (ok, IXLWorksheet? worksheet) = GetNamedWorksheetInExcelWorkbook(workbook, nameOfSource);
            if (!ok)
            {
                (ok, worksheet) = GetNamedWorksheetInExcelWorkbook(workbook, "Document#");
                if (!ok) return result;
            }

            (ok, int sourceCol, int targetCol) = GetSourceAndTargetColumnsFromWorksheet(worksheet, source.Meta.LanguageId, target.Meta.LanguageId);
            //(ok, int sourceCol, int targetCol) = GetSourceAndTargetColumnsFromWorksheet(worksheet, sourceIdx, targetIdx);
            if (!ok) return result;

            // iterate thru strings in source language, building dictionary of replacements ordered by length of sourceText
            SortedDictionary<string, (double _typeCode, string _text)> sortedDictionary = new(new LongestToShortestLengthComparer());
            (ok, SortedDictionary<string, (double _typeCode, string _text)> dict) = FillSortedDictionaryFromWorksheet(sortedDictionary, worksheet, sourceCol, targetCol);
            if (!ok) return result;

            long DEF_opcode = englishFixed.OpcodesByKey["DEF"]; //englishPBFixed.Keywords["DEF"];// englishFixed.OpcodesByKey["DEF"];
            long KOP_opcode = englishFixed.OpcodesByKey["KOP"]; //englishPBFixed.Keywords["KOP"];// englishFixed.OpcodesByKey["KOP"];
            long RST_opcode = englishFixed.OpcodesByKey["RST"]; //englishPBFixed.Keywords["RST"];// englishFixed.OpcodesByKey["RST"];

            
            foreach (string key in dict.Keys)
            {
                MainPage.Track("TranslateCode", "key=", key, "dict[key]._typeCode=", dict[key]._typeCode, "dict[key]._text=", dict[key]._text);

                //result = UpdateInLabelSpace(result, key, sortedDictionary[key]); // smartness:1
                //bool srcVariable;
                switch (dict[key]._typeCode)
                {
                    case 1: // undefined
                        break;
                    case 2: // KOP
                        //srcVariable = sourcePB.ReadValueAsString("variable") == "True";
                        string kopPattern = $"<{(source.Meta.Variable ? "#" : "@")} {source.OpcodesByValue[DEF_opcode]}{source.OpcodesByValue[KOP_opcode]}.*?>([^|<]+)";
                        //string kopPattern = $"<{(srcVariable ? "#":"@")} {sourcePB.Identifiers[DEF_opcode]}{sourcePB.Identifiers[KOP_opcode]}.*?>([^|<]+)";
                        Regex kopRegex = new(kopPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
                        MatchCollection kopMatches = kopRegex.Matches(result);

                        break;
                    case 3: // Code Block 
                        break;
                    case 4: // SQL
                        //srcVariable = sourcePB.ReadValueAsString("variable") == "True";
                        string rstPattern = $"<{(source.Meta.Variable ? "#" : "@")} {source.OpcodesByValue[RST_opcode]}.*?>([^<]+)";
                        //string rstPattern = $"<{(srcVariable ? "#" : "@")} {sourcePB.Identifiers[RST_opcode]}.*?>([^<]+)";
                        Regex rstRegex = new(rstPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
                        MatchCollection rstMatches = rstRegex.Matches(result);

                        break;
                    case 5: // undefind
                        break;
                    case 6: // file extension
                        break;
                    case 7: // Pattern
                        break;
                    case 8: // Syskey
                        break;
                    case 9: // Protium symbol
                        break;
                    case 10: // Wildcard
                        break;
                    case 11: // String Literal
                        result = result.Replace(key, dict[key]._text, StringComparison.CurrentCultureIgnoreCase); // smartness:0
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private string ProcessFixedToFixedOrVariableWithOrWithoutSpacePB(string buff, PropertyBag source, PropertyBag target, bool spaced, bool variableTarget)
        {
            var pattern = PelotonFixedSpacedPattern();
            MatchCollection matches = pattern.Matches(buff);
            for (int mi = matches.Count - 1; mi >= 0; mi--)
            {
                //var max = kopMatches[mi].Groups[2].Captures.Count - 1;
                for (int i = matches[mi].Groups[1].Captures.Count - 1; i >= 0; i--)
                {
                    var capture = matches[mi].Groups[1].Captures[i];
                    var key = capture.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Trim();
                    if (source.Keywords.TryGetValue(key, out long opcode))
                    {
                        if (target.Identifiers.TryGetValue(opcode, out string? value))
                        {
                            var newKey = value;
                            var next = buff.Substring(capture.Index + capture.Length, 1);
                            buff = buff.Remove(capture.Index, capture.Length)
                                .Insert(capture.Index, newKey + ((spaced && next != ">") ? " " : ""));
                        }
                    }
                }
                // var tag = kopMatches[mi].Groups[1].Captures[0];
            }
            return target.ReadValueAsString("variable") == "True" ? buff.Replace("<@ ", "<# ").Replace("</@>", "</#>") : buff;
        }

        private string ProcessVariableToFixedOrVariablePB(string code, PropertyBag source, PropertyBag target, bool spaced, bool variableTarget)
        {
            var variableLengthWords = from variableLengthWord in source.Keywords.Keys orderby -variableLengthWord.Length select variableLengthWord;

            var fixedLengthEquivalents = (from word in variableLengthWords
                                          let sourceop = source.Keywords[word]
                                          let targetword = target.Identifiers[sourceop]
                                          select (word, targetword)).ToDictionary(x => x.word, x => x.targetword);

            var codeBlocks = GetCodeBlocks(code); // in reverse order

            foreach (var block in codeBlocks)
            {
                var codeChunk = block.Value;
                foreach (var vlw in variableLengthWords)
                {
                    var spacedVlw = vlw + " ";

                    if (codeChunk.Contains(spacedVlw, StringComparison.CurrentCulture))
                    {
                        if (spaced)
                        {
                            codeChunk = codeChunk.Replace(spacedVlw, fixedLengthEquivalents[vlw] + " ").Trim();
                        }
                        else
                        {
                            codeChunk = codeChunk.Replace(spacedVlw, fixedLengthEquivalents[vlw]).Trim();
                        }
                        continue;
                    }
                    if (codeChunk.Contains(vlw, StringComparison.CurrentCulture))
                    {
                        if (spaced)
                        {
                            codeChunk = codeChunk.Replace(vlw, fixedLengthEquivalents[vlw] + " ").Trim();
                        }
                        else
                        {
                            codeChunk = codeChunk.Replace(vlw, fixedLengthEquivalents[vlw]).Trim();
                        }
                    }
                }
                code = code.Remove(block.Index, block.Length)
                    .Insert(block.Index, codeChunk);
            }
            return variableTarget ? code.Replace("<@", "<#").Replace("</@>", "</#>") : code.Replace("<#", "<@").Replace("</#>", "</@>");
        }

        private string UpdateInLabelSpace(string result, string sourceText, string targetText)
        {
            var pattern = PelotonFullPattern();
            MatchCollection matches = pattern.Matches(result);
            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match match = matches[i];
                Group label = match.Groups[2];
                var index = label.Index;
                var length = label.Length;
                var value = label.Value;
                if (value.Contains(sourceText, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = result.Remove(index, length);
                    value = value.Replace(sourceText, targetText, StringComparison.CurrentCultureIgnoreCase);
                    result = result.Insert(index, value);
                }
            }
            return result;
        }

        private static string ProcessVariableToFixedOrVariable(string code, Plex source, Plex target, bool spaced, bool variableTarget)
        {
            var variableLengthWords = from variableLengthWord in source.OpcodesByKey.Keys orderby -variableLengthWord.Length select variableLengthWord;

            var fixedLengthEquivalents = (from word in variableLengthWords
                                          let sourceop = source.OpcodesByKey[word]
                                          let targetword = target.OpcodesByValue[sourceop]
                                          select (word, targetword)).ToDictionary(x => x.word, x => x.targetword);

            var codeBlocks = GetCodeBlocks(code); // in reverse order

            foreach (var block in codeBlocks)
            {
                var codeChunk = block.Value;
                foreach (var vlw in variableLengthWords)
                {
                    var spacedVlw = vlw + " ";

                    if (codeChunk.Contains(spacedVlw, StringComparison.CurrentCulture))
                    {
                        if (spaced)
                        {
                            codeChunk = codeChunk.Replace(spacedVlw, fixedLengthEquivalents[vlw] + " ").Trim();
                        }
                        else
                        {
                            codeChunk = codeChunk.Replace(spacedVlw, fixedLengthEquivalents[vlw]).Trim();
                        }
                        continue;
                    }
                    if (codeChunk.Contains(vlw, StringComparison.CurrentCulture))
                    {
                        if (spaced)
                        {
                            codeChunk = codeChunk.Replace(vlw, fixedLengthEquivalents[vlw] + " ").Trim();
                        }
                        else
                        {
                            codeChunk = codeChunk.Replace(vlw, fixedLengthEquivalents[vlw]).Trim();
                        }
                    }
                }
                code = code.Remove(block.Index, block.Length)
                    .Insert(block.Index, codeChunk);
            }
            return variableTarget ? code.Replace("<@", "<#").Replace("</@>", "</#>") : code.Replace("<#", "<@").Replace("</#>", "</@>");
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

        private static string ProcessFixedToFixedOrVariableWithOrWithoutSpace(string buff, Plex sourcePlex, Plex targetPlex, bool spaceOut, bool variableTarget)
        {
            var pattern = PelotonFixedSpacedPattern();
            MatchCollection matches = pattern.Matches(buff);
            for (int mi = matches.Count - 1; mi >= 0; mi--)
            {
                //var max = kopMatches[mi].Groups[2].Captures.Count - 1;
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
                // var tag = kopMatches[mi].Groups[1].Captures[0];
            }
            return targetPlex.Meta.Variable ? buff.Replace("<@ ", "<# ").Replace("</@>", "</#>") : buff;
        }
    }
}
