using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Documents;

using System.Diagnostics;
using System.Text;

using Uno.Extensions;

using Windows.Storage;
using Windows.UI.Core;

using Paragraph = Microsoft.UI.Xaml.Documents.Paragraph;
using Run = Microsoft.UI.Xaml.Documents.Run;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private void ExecuteInterpreter(string selectedText)
        {
            Track("ExecuteInterpreter", "selectedText=", selectedText);
            // load tab settings

            DispatcherQueue dispatcher = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

            CustomTabItem navigationViewItem = (CustomTabItem)tabControl.SelectedItem;
            long quietude = (long)navigationViewItem.TabSettingsDict["Quietude"]["Value"];
            string engineArguments = BuildTabCommandLine();

            // override with matching tab settings
            // generate arguments string
            string stdOut;
            string stdErr;
            if (ApplicationData.Current.LocalSettings.Values["Engine"].ToString() == "Interpreter.P3")
            {
                (stdOut, stdErr) = RunPeloton2(engineArguments, selectedText, quietude, dispatcher);
            }
            else
            {
                (stdOut, stdErr) = RunProtium(engineArguments, selectedText, quietude);
            }

            Track("ExecuteInterpreter", "stdOut=", stdOut, "stdErr=", stdErr);

            if (!string.IsNullOrEmpty(stdErr))
            {
                AddInsertParagraph(errorText, stdErr, false);
            }
            if (!string.IsNullOrEmpty(stdOut))
            {
                AddInsertParagraph(outputText, stdOut, false);
            }
        }

        public void AddOutput(string text)
        {
            AddInsertParagraph(outputText, text, true, false);
        }

        public void AddError(string text)
        {
            AddInsertParagraph(errorText, text, true, false);
        }

        private static void AddInsertParagraph(RichEditBox reb, string text, bool addInsert = true, bool withPrefix = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            Track("AddInsertParagraph", "text=", text, "addInsert=", addInsert, "withPrefix=", withPrefix);
            const string stamp = "> ";
            if (withPrefix)
                text = text.Insert(0, stamp);

            reb.IsReadOnly = false;
            reb.Document.GetText(Microsoft.UI.Text.TextGetOptions.UseLf, out string? t);
            if (addInsert)
            {
                reb.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, t + "\n" + text);
                //reb.Document.GetRange(t.Length, t.Length).Text = t;
            }
            else
            {
                //reb.Document.GetRange(0, 0).Text = t;
                reb.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, text + "\n" + t);
            }
            reb.Focus(FocusState.Programmatic);
            reb.IsReadOnly = true;
        }

        public (string StdOut, string StdErr) RunProtium(string args, string buff, long quietude)
        {
            Track("RunProtium", args, buff);

            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P2"].ToString();
            string temp = Path.GetTempFileName();
            File.WriteAllText(temp, buff, Encoding.Unicode);

            args = args.Replace(":", "=");

            args += $" /F:\"{temp}\"";

            Track("RunProtium", args, buff);

            ProcessStartInfo info = new()
            {
                Arguments = $"{args}",
                FileName = Exe,
                UseShellExecute = false,
                CreateNoWindow = args.Contains("/Q=0"),
            };

            Process? proc = Process.Start(info);
            proc.WaitForExit();
            proc.Dispose();

            return (StdOut: File.ReadAllText(Path.ChangeExtension(temp, "out")), StdErr: string.Empty);
        }

        public (string StdOut, string StdErr) RunPeloton(string args, string buff, long quietude)
        {
            Track("RunPeloton", args, buff);

            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P3"].ToString();
            string t_in = Path.GetTempFileName();
            string t_out = Path.ChangeExtension(t_in, "out");
            string t_err = Path.ChangeExtension(t_in, "err");

            File.WriteAllText(t_in, buff);

            //args = args.Replace(":", "=");

            args += $" /F:\"{t_in}\""; // 1>\"{t_out}\" 2>\"{t_err}\"";

            Track("RunPeloton", args, buff);

            ProcessStartInfo info = new()
            {
                Arguments = $"{args}",
                FileName = Exe,
                UseShellExecute = false,
                CreateNoWindow = args.Contains("/Q:0"),
            };

            Process? proc = Process.Start(info);
            proc.WaitForExit();
            proc.Dispose();

            return (StdOut: File.Exists(t_out) ? File.ReadAllText(t_out) : string.Empty, StdErr: File.Exists(t_err) ? File.ReadAllText(t_err) : string.Empty);
        }

        public (string StdOut, string StdErr) RunPeloton2(string args, string buff, long quietude, DispatcherQueue dispatcher)
        {
            Track("RunPeloton2", args, buff);

            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P3"].ToString();

            Track("RunPeloton2", "Exe=", Exe);

            ProcessStartInfo info = new()
            {
                Arguments = $"{args}",
                FileName = Exe,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process? proc = Process.Start(info);
            StringBuilder stdout = new();
            StringBuilder stderr = new();

            StreamWriter stream = proc.StandardInput;
            stream.Write(buff);
            stream.Close();

            proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (quietude == 0)
                {
                    stdout.AppendLine(e.Data);
                }
                else
                {
                    dispatcher.TryEnqueue(() =>
                    {
                        Track("RunPeloton2", "stdout e.Data=", e.Data!);
                        AddOutput(e.Data!);
                    });
                }
            };
            proc.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                stderr.AppendLine(e.Data);
            };

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            proc.Dispose();

            Track("RunPeloton2", "Disposed");

            return (StdOut: stdout.ToString().Trim(), StdErr: stderr.ToString().Trim());
        }

    }
}
