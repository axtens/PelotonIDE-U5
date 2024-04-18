﻿using DocumentFormat.OpenXml.Bibliography;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace PelotonIDE.Presentation
{
    public static class Telemetry
    {
        private static bool enabled;
        private static bool firsted = false;
        public static bool GetEnabled()
        {
            return enabled;
        }

        public static void SetEnabled(bool value)
        {
            enabled = value;
        }

        public static void Transmit(params object?[] args)
        {
            if (!enabled) return;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            string path = Path.Combine(folder.Path, $"{DateTime.Now:yyyy-MM-dd-HH}_pi.log");

            List<string> frame = [];
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            string you = trace.GetFrame(1).GetMethod().Name;

            StringBuilder sb = new();
            if (!firsted)
            {
                sb.Append("---");
                firsted = true;
                File.AppendAllText(path, $"{DateTime.Now:o} > {sb}\r\n", Encoding.UTF8);
            }
            else
            {
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
            }
            return;
        }
    }
}
