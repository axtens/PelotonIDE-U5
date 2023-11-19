using Microsoft.UI.Xaml.Documents;

using System.Diagnostics;
using System.Text;

using Windows.Storage;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private void ExecuteInterpreter(string selectedText)
        {
            // load tab settings
            string engineArguments = BuildTabCommandLine();

            // override with matching tab settings
            // generate arguments string
            string stdOut;
            string stdErr;
            if (ApplicationData.Current.LocalSettings.Values["Engine"].ToString() == "Interpreter.P3")
            {
                (stdOut, stdErr) = RunPeloton(engineArguments, selectedText);
            }
            else
            {
                (stdOut, stdErr) = RunProtium(engineArguments, selectedText);
            }

            const string stamp = ">\r\n"; // System.DateTime.Now.ToString("O") + "\r\n";
            stdErr = stdErr.Insert(0, stamp);
            stdOut = stdOut.Insert(0, stamp);
            Run run = new();
            Paragraph paragraph = new();
            if (!string.IsNullOrEmpty(stdOut))
            {
                run.Text = stdOut;
                paragraph.Inlines.Add(run);
                outputText.Blocks.Add(paragraph);
            }

            run = new();
            paragraph = new();
            if (!string.IsNullOrEmpty(stdErr))
            {
                run.Text = stdErr;
                paragraph.Inlines.Add(run);
                errorText.Blocks.Add(paragraph);
            }
        }

        public static (string StdOut, string StdErr) RunProtium(string args, string buff)
        {
            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P2"].ToString();
            var temp = Path.GetTempFileName();
            File.WriteAllText(temp, buff);

            args += $" /F:\"{temp}\"";

            ProcessStartInfo info = new()
            {
                Arguments = $"{args}",
                FileName = Exe,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var proc = Process.Start(info);
            StringBuilder stdout = new();
            StringBuilder stderr = new();

            proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) => stdout.AppendLine(e.Data);
            proc.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => stderr.AppendLine(e.Data);

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            proc.Dispose();

            return (StdOut: stdout.ToString().Trim(), StdErr: stderr.ToString().Trim());
        }

        public static (string StdOut, string StdErr) RunPeloton(string args, string buff)
        {
            string? Exe = ApplicationData.Current.LocalSettings.Values["Interpreter.P3"].ToString();

            ProcessStartInfo info = new()
            {
                Arguments = $"{args}",
                FileName = Exe,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
            };

            var proc = Process.Start(info);
            StringBuilder stdout = new();
            StringBuilder stderr = new();

            StreamWriter stream = proc.StandardInput;
            stream.Write(buff);
            stream.Close();

            proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) => stdout.AppendLine(e.Data);
            proc.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => stderr.AppendLine(e.Data);

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            proc.Dispose();

            return (StdOut: stdout.ToString().Trim(), StdErr: stderr.ToString().Trim());
        }
    }
}
