// Decompiled with JetBrains decompiler
// Type: EncodingChecker.Utf16Detector
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System;
using System.IO;
using System.Text;


#nullable enable
namespace EncodingChecker
{
  public static class Utf16Detector
  {
    public static Encoding DetectFromBytes(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Utf16Detector.DetectFromStream((Stream) memoryStream);
    }

    public static Encoding DetectFromBytes(byte[] bytes, int offset, int len)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (len < 0)
        throw new ArgumentOutOfRangeException(nameof (len));
      if (bytes.Length < offset + len)
        throw new ArgumentException("len is greater than the number of bytes from offset to the end of the array.");
      using (MemoryStream memoryStream = new MemoryStream(bytes, offset, len))
        return Utf16Detector.DetectFromStream((Stream) memoryStream);
    }

    public static Encoding DetectFromStream(Stream stream) => stream != null ? Utf16Detector.DetectFromStream(stream, new int?()) : throw new ArgumentNullException(nameof (stream));

    public static Encoding DetectFromStream(Stream stream, int? maxBytesToRead)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      int? nullable1 = maxBytesToRead;
      int num1 = 0;
      if (nullable1.GetValueOrDefault() <= num1 & nullable1.HasValue)
        throw new ArgumentOutOfRangeException(nameof (maxBytesToRead));
      int num2;
      if (maxBytesToRead.HasValue)
      {
        nullable1 = maxBytesToRead;
        long? nullable2 = nullable1.HasValue ? new long?((long) nullable1.GetValueOrDefault()) : new long?();
        long length = stream.Length;
        num2 = nullable2.GetValueOrDefault() > length & nullable2.HasValue ? 1 : 0;
      }
      else
        num2 = 1;
      if (num2 != 0)
        maxBytesToRead = new int?((int) stream.Length);
      byte[] buffer = new byte[maxBytesToRead.Value];
      int num3 = stream.Read(buffer, 0, Math.Min(maxBytesToRead.Value, 4));
      if (Utf16Detector.CheckUtfSignature(buffer, num3) != null)
        return (Encoding) null;
      int num4 = num3;
      nullable1 = maxBytesToRead;
      int valueOrDefault = nullable1.GetValueOrDefault();
      if (num4 < valueOrDefault & nullable1.HasValue)
        num3 += stream.Read(buffer, num3, maxBytesToRead.Value - num3);
      return Utf16Detector.CheckUtf16Ascii(buffer, num3) ?? Utf16Detector.CheckUtf16ControlChars(buffer, num3);
    }

    public static Encoding DetectFromFile(string filePath)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof (filePath));
      using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        return Utf16Detector.DetectFromStream((Stream) fileStream);
    }

    public static Encoding DetectFromFile(string filePath, int? maxBytesToRead)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof (filePath));
      int? nullable = maxBytesToRead;
      int num = 0;
      if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
        throw new ArgumentOutOfRangeException(nameof (maxBytesToRead));
      using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        return Utf16Detector.DetectFromStream((Stream) fileStream, maxBytesToRead);
    }

    private static Encoding CheckUtfSignature(byte[] buffer, int size)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (size < 0)
        throw new ArgumentOutOfRangeException(nameof (size));
      if (size < 2)
        return (Encoding) null;
      if (buffer[0] == byte.MaxValue && buffer[1] == (byte) 254 && (size < 4 || buffer[2] != (byte) 0 || buffer[3] > (byte) 0))
        return Encoding.Unicode;
      if (buffer[0] == (byte) 254 && buffer[1] == byte.MaxValue && (size < 4 || buffer[2] != (byte) 0 || buffer[3] > (byte) 0))
        return Encoding.BigEndianUnicode;
      if (size >= 3 && buffer[0] == (byte) 239 && buffer[1] == (byte) 187 && buffer[2] == (byte) 191)
        return Encoding.UTF8;
      if (size >= 4 && buffer[0] == byte.MaxValue && buffer[1] == (byte) 254 && buffer[2] == (byte) 0 && buffer[3] == (byte) 0)
        return Encoding.UTF32;
      return size >= 4 && buffer[0] == (byte) 0 && buffer[1] == (byte) 0 && buffer[2] == (byte) 254 && buffer[3] == byte.MaxValue ? Encoding.GetEncoding("utf-32BE") : (Encoding) null;
    }

    private static Encoding CheckUtf16Ascii(byte[] buffer, int size)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (size < 0)
        throw new ArgumentOutOfRangeException(nameof (size));
      if (size < 2)
        return (Encoding) null;
      --size;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      while (num3 < size)
      {
        byte[] numArray1 = buffer;
        int index1 = num3;
        int num4 = index1 + 1;
        byte num5 = numArray1[index1];
        byte[] numArray2 = buffer;
        int index2 = num4;
        num3 = index2 + 1;
        byte num6 = numArray2[index2];
        if (num5 == (byte) 0 && num6 > (byte) 0)
          ++num2;
        if (num5 != (byte) 0 && num6 == (byte) 0)
          ++num1;
      }
      ++size;
      double num7 = (double) num1 * 2.0 / (double) size;
      double num8 = (double) num2 * 2.0 / (double) size;
      if (num7 > 0.5 && num8 < 0.1)
        return Encoding.Unicode;
      return num8 > 0.5 && num7 < 0.1 ? Encoding.BigEndianUnicode : (Encoding) null;
    }

    private static Encoding? CheckUtf16ControlChars(byte[] buffer, int size)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (size < 0)
        throw new ArgumentOutOfRangeException(nameof (size));
      if (size < 2)
        return (Encoding) null;
      --size;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      while (num3 < size)
      {
        byte[] numArray1 = buffer;
        int index1 = num3;
        int num4 = index1 + 1;
        byte num5 = numArray1[index1];
        byte[] numArray2 = buffer;
        int index2 = num4;
        num3 = index2 + 1;
        byte num6 = numArray2[index2];
        if (num5 == (byte) 0)
        {
          if (num6 == (byte) 13 || num6 == (byte) 10 || num6 == (byte) 32 || num6 == (byte) 9)
            ++num2;
        }
        else if (num6 == (byte) 0 && (num5 == (byte) 13 || num5 == (byte) 10 || num5 == (byte) 32 || num5 == (byte) 9))
          ++num1;
        if (num1 > 0 && num2 > 0)
          return (Encoding) null;
      }
      return num1 > 0 ? Encoding.Unicode : (num2 > 0 ? Encoding.BigEndianUnicode : (Encoding) null);
    }
  }
}
