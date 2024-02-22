using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Documents;

using System.Diagnostics;
using System.Text;

using Uno.Extensions;

using Windows.Devices.PointOfService;
using Windows.Storage;
using Windows.UI.Core;

using Paragraph = Microsoft.UI.Xaml.Documents.Paragraph;
using Run = Microsoft.UI.Xaml.Documents.Run;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private async void ExecuteInterpreter(string selectedText)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            DispatcherQueue dispatcher = DispatcherQueue.GetForCurrentThread();

            if (Type_3_GetInFocusTab<long>("Quietude") == 0 && Type_3_GetInFocusTab<long>("Timeout") > 0 )
            {
                // Yes, No, Cancel

                //Task<int> sure = AreYouSureYouWantToRunALongTimeSilently();
                //sure.ContinueWith(t => t);
                //if (sure.Result == 2) return;
                //if (sure.Result == 1)
                //    Type_3_UpdateInFocusTabSettings<long>("Quietude", true, 1);
                // Task<ContentDialogResult> task = AreYouSureYouWantToRunALongTimeSilently();
                // task.Wait();
                if (!await AreYouSureYouWantToRunALongTimeSilently())
                {
                    return;
                }
            }

            telem.Transmit("selectedText=", selectedText);
            // load tab settings


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

            telem.Transmit("stdOut=", stdOut, "stdErr=", stdErr);

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
            Telemetry telem = new();
            telem.SetEnabled(true);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            telem.Transmit("text=", text, "addInsert=", addInsert, "withPrefix=", withPrefix);
            const string stamp = "> ";
            if (withPrefix)
                text = text.Insert(0, stamp);

            //reb.IsReadOnly = false;
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
            //reb.IsReadOnly = true;
        }

        public (string StdOut, string StdErr) RunProtium(string args, string buff, long quietude)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);
            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P2"].ToString();
            string temp = System.IO.Path.GetTempFileName();
            File.WriteAllText(temp, buff, Encoding.Unicode);

            args = args.Replace(":", "=");

            args += $" /F:\"{temp}\"";

            telem.Transmit("Exe=", Exe, "Args:", args, "Buff=", buff, "Quietude=", quietude);

            ProcessStartInfo info = new()
            {
                Arguments = $"{args}",
                FileName = Exe,
                UseShellExecute = false,
                CreateNoWindow = args.Contains("/Q=0"),
            };

            Process? proc = Process.Start(info);
            proc.WaitForExit(GetTimeoutInMilliseconds());
            proc.Dispose();

            return (StdOut: File.ReadAllText(System.IO.Path.ChangeExtension(temp, "out")), StdErr: string.Empty);
        }

        public (string StdOut, string StdErr) RunPeloton(string args, string buff, long quietude)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P3"].ToString();

            telem.Transmit("Exe=", Exe, "Args:", args, "Buff=", buff, "Quietude=", quietude);

            string t_in = System.IO.Path.GetTempFileName();
            string t_out = System.IO.Path.ChangeExtension(t_in, "out");
            string t_err = System.IO.Path.ChangeExtension(t_in, "err");

            File.WriteAllText(t_in, buff);

            //args = args.Replace(":", "=");

            args += $" /F:\"{t_in}\""; // 1>\"{t_out}\" 2>\"{t_err}\"";

            telem.Transmit(args, buff);

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

        public void Inject(string? arg)
        {
            //outputText.IsReadOnly = false;
            outputText.Document.GetText(Microsoft.UI.Text.TextGetOptions.AdjustCrlf, out string? value);
            outputText.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, $"{value}{arg}");
            //outputText.IsReadOnly = true;
        }
        public (string StdOut, string StdErr) RunPeloton2(string args, string buff, long quietude, DispatcherQueue dispatcher)
        {
            Telemetry telem = new();
            telem.SetEnabled(true);

            string temp = System.IO.Path.GetTempFileName();
            File.WriteAllText(temp, buff, Encoding.Unicode);

            telem.Transmit("temp=", temp);

            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P3"].ToString();

            telem.Transmit("Exe=", Exe, "Args:", args, "Buff=", buff, "Quietude=", quietude);

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

            //inject($"{DateTime.Now:o}(\r");
            Process? proc = Process.Start(info);
            StringBuilder stdout = new();
            StringBuilder stderr = new();

            StreamWriter stream = proc.StandardInput;
            stream.Write(buff);
            stream.Close();

            proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                //if (quietude == 0)
                //{
                //    stdout.AppendLine(e.Data!);
                //}
                //else
                //{
                if (e.Data != null)
                {
                    dispatcher.TryEnqueue(() =>
                    {
                        //Inject($"{DateTime.Now:o}> {e.Data}\r");
                        Inject($"> {e.Data}\r");
                    });
                }
                //}
            };
            proc.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                Telemetry telem = new();
                telem.SetEnabled(true);
                telem.Transmit(e.Data);
                stderr.AppendLine(e.Data);
            };

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit(GetTimeoutInMilliseconds());
            proc.Dispose();

            //inject($"){DateTime.Now:o}\r");

            return (StdOut: stdout.ToString().Trim(), StdErr: stderr.ToString().Trim());
        }

        private int GetTimeoutInMilliseconds()
        {
            long timeout = Type_1_GetVirtualRegistry<long>("Timeout");
            int timeoutInMilliseconds = -1;
            switch (timeout)
            {
                case 0:
                    timeoutInMilliseconds = 20 * 1000; 
                    break;
                case 1:
                    timeoutInMilliseconds = 100 * 1000; 
                    break;
                case 2:
                    timeoutInMilliseconds = 200 * 1000; 
                    break;
                case 3:
                    timeoutInMilliseconds = 1000 * 1000; 
                    break;
                case 4:
                    timeoutInMilliseconds = -1;
                    break;
            }
            return timeoutInMilliseconds;
        }
    }
}
