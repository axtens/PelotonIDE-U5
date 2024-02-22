using DocumentFormat.OpenXml.Bibliography;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace PelotonIDE.Presentation
{
    public class Telemetry
    {
        private bool enabled;

        public Telemetry()
        {
            SetEnabled(false);
        }

        public bool GetEnabled()
        {
            return enabled;
        }

        public Telemetry SetEnabled(bool value)
        {
            enabled = value;
            return this;
        }

        public Telemetry Transmit(params object?[] args)
        {
            if (!enabled) return this;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            string path = Path.Combine(folder.Path, $"{DateTime.Now:yyyy-MM-dd-HH}_pi.log");

            List<string> frame = [];
            var trace = new System.Diagnostics.StackTrace();
            string you = trace.GetFrame(1).GetMethod().Name;

            StringBuilder sb = new();
            sb.Append($"From {you}: ");
            for (int i = 0; i < args.Length; i++)
            {
                string item = $"{args[i]}";
                if (i == 0)
                {
                    sb.Append(item);
                }
                else
                {
                    string prev = $"{args[i - 1]}";
                    if (prev.EndsWith("="))
                    {
                        sb.Append(item);
                    }
                    else
                    {
                        sb.Append(' ').Append(item);
                    }
                }
            }
            File.AppendAllText(path, $"{DateTime.Now:o} > {sb}\r\n", Encoding.UTF8);
            return this;
        }
    }
}
