// Decompiled with JetBrains decompiler
// Type: EncChecker.EncCheck
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UtfUnknown;


#nullable enable
namespace EncChecker
{
  public static class EncCheck
  {
    public static Encoding DetectFileAsEncoding(string filename) => CharsetDetector.DetectFromFile(filename).Detected.Encoding;

    public static string DetectFile(string filename)
    {
      DetectionDetail detected = CharsetDetector.DetectFromFile(filename).Detected;
      if (detected == null)
        return "Encoding for " + filename + " not detected";
      string[] strArray = new string[3]
      {
        "EncodingName^" + detected.EncodingName.ToUpper(),
        "",
        ""
      };
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
      interpolatedStringHandler.AppendLiteral("Confidence^");
      interpolatedStringHandler.AppendFormatted<float>(detected.Confidence);
      strArray[1] = interpolatedStringHandler.ToStringAndClear();
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
      interpolatedStringHandler.AppendLiteral("HasBOM^");
      interpolatedStringHandler.AppendFormatted<bool>(detected.HasBOM);
      interpolatedStringHandler.AppendLiteral("~");
      strArray[2] = interpolatedStringHandler.ToStringAndClear();
      return string.Join("~", strArray);
    }

    public static string DetectString(string str)
    {
      DetectionDetail detected = CharsetDetector.DetectFromBytes(((IEnumerable<char>) str.ToCharArray()).Select<char, byte>((Func<char, byte>) (character => (byte) character)).ToArray<byte>()).Detected;
      if (detected == null)
        return "Encoding not detected";
      string[] strArray = new string[3]
      {
        "EncodingName^" + detected.EncodingName.ToUpper(),
        "",
        ""
      };
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
      interpolatedStringHandler.AppendLiteral("Confidence^");
      interpolatedStringHandler.AppendFormatted<float>(detected.Confidence);
      strArray[1] = interpolatedStringHandler.ToStringAndClear();
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
      interpolatedStringHandler.AppendLiteral("HasBOM^");
      interpolatedStringHandler.AppendFormatted<bool>(detected.HasBOM);
      strArray[2] = interpolatedStringHandler.ToStringAndClear();
      return string.Join("~", strArray) + "~";
    }

        public static Encoding DetectBytesAsEncoding(byte[] bytes) => CharsetDetector.DetectFromBytes(bytes).Detected.Encoding;
    }
}
