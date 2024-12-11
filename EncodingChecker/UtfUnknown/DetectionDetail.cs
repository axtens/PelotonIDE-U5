// Decompiled with JetBrains decompiler
// Type: UtfUnknown.DetectionDetail
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UtfUnknown.Core.Probers;


#nullable enable
namespace UtfUnknown
{
  public class DetectionDetail
  {
    private static readonly Dictionary<string, string> FixedToSupportCodepageName = new Dictionary<string, string>()
    {
      {
        "cp949",
        "ks_c_5601-1987"
      },
      {
        "iso-2022-cn",
        "x-cp50227"
      }
    };

    public DetectionDetail(
      string encodingShortName,
      float confidence,
      CharsetProber prober = null,
      TimeSpan? time = null,
      string statusLog = null)
    {
      this.EncodingName = encodingShortName;
      this.Confidence = confidence;
      this.Encoding = DetectionDetail.GetEncoding(encodingShortName);
      this.Prober = prober;
      this.Time = time;
      this.StatusLog = statusLog;
    }

    public DetectionDetail(CharsetProber prober, TimeSpan? time = null)
      : this(prober.GetCharsetName(), prober.GetConfidence(), prober, time, prober.DumpStatus())
    {
    }

    public string EncodingName { get; }

    public Encoding Encoding { get; set; }

    public float Confidence { get; set; }

    public CharsetProber Prober { get; set; }

    public bool HasBOM { get; set; }

    public TimeSpan? Time { get; set; }

    public string StatusLog { get; set; }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 3);
      interpolatedStringHandler.AppendLiteral("Detected ");
      interpolatedStringHandler.AppendFormatted(this.EncodingName);
      interpolatedStringHandler.AppendLiteral(" with confidence of ");
      interpolatedStringHandler.AppendFormatted<float>(this.Confidence);
      interpolatedStringHandler.AppendLiteral(". (BOM: ");
      interpolatedStringHandler.AppendFormatted<bool>(this.HasBOM);
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }

    internal static Encoding GetEncoding(string encodingShortName)
    {
      string str;
      string name = DetectionDetail.FixedToSupportCodepageName.TryGetValue(encodingShortName, out str) ? str : encodingShortName;
      try
      {
        return Encoding.GetEncoding(name);
      }
      catch (ArgumentException ex)
      {
        return (Encoding) null;
      }
    }
  }
}
