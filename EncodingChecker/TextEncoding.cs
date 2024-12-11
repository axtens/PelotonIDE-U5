// Decompiled with JetBrains decompiler
// Type: EncodingChecker.TextEncoding
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System;
using System.IO;
using System.Text;
using UtfUnknown;


#nullable enable
namespace EncodingChecker
{
  public static class TextEncoding
  {
    private static readonly DecoderExceptionFallback DecoderExceptionFallback = new DecoderExceptionFallback();

    public static bool Validate(this Encoding encoding, byte[] bytes, int offset = 0, int? length = null)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      ref int? local = ref length;
      int? nullable1 = length;
      int num1 = nullable1 ?? bytes.Length;
      local = new int?(num1);
      if (offset < 0 || offset > bytes.Length)
        throw new ArgumentOutOfRangeException(nameof (offset), "Offset is out of range.");
      nullable1 = length;
      int num2 = 0;
      int num3;
      if (!(nullable1.GetValueOrDefault() < num2 & nullable1.HasValue))
      {
        nullable1 = length;
        int length1 = bytes.Length;
        num3 = nullable1.GetValueOrDefault() > length1 & nullable1.HasValue ? 1 : 0;
      }
      else
        num3 = 1;
      if (num3 != 0)
        throw new ArgumentOutOfRangeException(nameof (length), "Length is out of range.");
      int num4 = offset;
      int? nullable2 = length;
      nullable1 = nullable2.HasValue ? new int?(num4 + nullable2.GetValueOrDefault()) : new int?();
      int length2 = bytes.Length;
      if (nullable1.GetValueOrDefault() > length2 & nullable1.HasValue)
        throw new ArgumentOutOfRangeException(nameof (offset), "The specified range is outside of the specified buffer.");
      Decoder decoder = encoding.GetDecoder();
      decoder.Fallback = (DecoderFallback) TextEncoding.DecoderExceptionFallback;
      try
      {
        decoder.GetCharCount(bytes, offset, length.Value);
      }
      catch (DecoderFallbackException ex)
      {
        return false;
      }
      return true;
    }

    public static Encoding GetFileEncoding(string filePath, ref bool hasBOM) => TextEncoding.GetFileEncoding(filePath, new int?(), ref hasBOM);

    public static Encoding? GetFileEncoding(string filePath, int? maxBytesToRead, ref bool hasBOM)
    {
      hasBOM = false;
      try
      {
        using (FileStream fileStream1 = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          Encoding fileEncoding = Utf16Detector.DetectFromStream((Stream) fileStream1, maxBytesToRead);
          if (fileEncoding != null)
            return fileEncoding;
          fileStream1.Position = 0L;
          FileStream fileStream2 = fileStream1;
          int? nullable = maxBytesToRead;
          long? maxBytesToRead1 = nullable.HasValue ? new long?((long) nullable.GetValueOrDefault()) : new long?();
          DetectionResult detectionResult = CharsetDetector.DetectFromStream((Stream) fileStream2, maxBytesToRead1);
          if (detectionResult.Detected == null)
            return null;
          hasBOM = detectionResult.Detected.HasBOM;
          return detectionResult.Detected.Encoding;
        }
      }
      catch
      {
        return null;
      }
    }
  }
}
