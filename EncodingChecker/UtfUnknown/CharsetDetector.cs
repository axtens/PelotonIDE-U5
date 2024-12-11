// Decompiled with JetBrains decompiler
// Type: UtfUnknown.CharsetDetector
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UtfUnknown.Core;
using UtfUnknown.Core.Probers;


#nullable enable
namespace UtfUnknown
{
  public class CharsetDetector
  {
    internal InputState InputState;
    private bool _start;
    private bool _gotData;
    private bool _done;
    private byte _lastChar;
    private IList<CharsetProber> _charsetProbers;
    private IList<CharsetProber> _escCharsetProber;
    private DetectionDetail _detectionDetail;
    private const float MinimumThreshold = 0.2f;

    private IList<CharsetProber> CharsetProbers
    {
      get
      {
        switch (this.InputState)
        {
          case InputState.EscASCII:
            return this._escCharsetProber;
          case InputState.Highbyte:
            return this._charsetProbers;
          default:
            return (IList<CharsetProber>) new List<CharsetProber>();
        }
      }
    }

    private CharsetDetector()
    {
      this._start = true;
      this.InputState = InputState.PureASCII;
      this._lastChar = (byte) 0;
    }

    public static DetectionResult DetectFromBytes(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      CharsetDetector charsetDetector = new CharsetDetector();
      charsetDetector.Feed(bytes, 0, bytes.Length);
      return charsetDetector.DataEnd();
    }

    public static DetectionResult DetectFromBytes(byte[] bytes, int offset, int len)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (len < 0)
        throw new ArgumentOutOfRangeException(nameof (len));
      if (bytes.Length < offset + len)
        throw new ArgumentException("len is greater than the number of bytes from offset to the end of the array.");
      CharsetDetector charsetDetector = new CharsetDetector();
      charsetDetector.Feed(bytes, offset, len);
      return charsetDetector.DataEnd();
    }

    public static DetectionResult DetectFromStream(Stream stream) => stream != null ? CharsetDetector.DetectFromStream(stream, new long?()) : throw new ArgumentNullException(nameof (stream));

    public static DetectionResult DetectFromStream(Stream stream, long? maxBytesToRead)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      long? nullable = maxBytesToRead;
      long num = 0;
      if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
        throw new ArgumentOutOfRangeException(nameof (maxBytesToRead));
      CharsetDetector detector = new CharsetDetector();
      CharsetDetector.ReadStream(stream, maxBytesToRead, detector);
      return detector.DataEnd();
    }

    private static void ReadStream(Stream stream, long? maxBytes, CharsetDetector detector)
    {
      byte[] numArray = new byte[1024];
      long readTotal = 0;
      int read = CharsetDetector.CalcToRead(maxBytes, readTotal, 1024);
      int len;
      while ((len = stream.Read(numArray, 0, read)) > 0)
      {
        detector.Feed(numArray, 0, len);
        if (maxBytes.HasValue)
        {
          readTotal += (long) len;
          long num = readTotal;
          long? nullable = maxBytes;
          long valueOrDefault = nullable.GetValueOrDefault();
          if (num >= valueOrDefault & nullable.HasValue)
            break;
          read = CharsetDetector.CalcToRead(maxBytes, readTotal, 1024);
        }
        if (detector._done)
          break;
      }
    }

    private static int CalcToRead(long? maxBytes, long readTotal, int bufferSize)
    {
      long num = readTotal + (long) bufferSize;
      long? nullable = maxBytes;
      long valueOrDefault = nullable.GetValueOrDefault();
      return num > valueOrDefault & nullable.HasValue ? (int) maxBytes.Value - (int) readTotal : bufferSize;
    }

    public static DetectionResult DetectFromFile(string filePath)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof (filePath));
      using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        return CharsetDetector.DetectFromStream((Stream) fileStream);
    }

    public static DetectionResult DetectFromFile(FileInfo file)
    {
      if (file == null)
        throw new ArgumentNullException(nameof (file));
      using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        return CharsetDetector.DetectFromStream((Stream) fileStream);
    }

    protected virtual void Feed(byte[] buf, int offset, int len)
    {
      if (this._done)
        return;
      if (len > 0)
        this._gotData = true;
      if (this._start)
      {
        this._start = false;
        this._done = this.IsStartsWithBom(buf, offset, len);
        if (this._done)
          return;
      }
      this.FindInputState(buf, offset, len);
      foreach (CharsetProber charsetProber in (IEnumerable<CharsetProber>) this.CharsetProbers)
      {
        this._done = this.RunProber(buf, offset, len, charsetProber);
        if (this._done)
          break;
      }
    }

    private bool IsStartsWithBom(byte[] buf, int offset, int len)
    {
      string charSetByBom = CharsetDetector.FindCharSetByBom(buf, offset, len);
      if (charSetByBom == null)
        return false;
      this._detectionDetail = new DetectionDetail(charSetByBom, 1f)
      {
        HasBOM = true
      };
      return true;
    }

    private bool RunProber(byte[] buf, int offset, int len, CharsetProber charsetProber)
    {
      if (charsetProber.HandleData(buf, offset, len) != ProbingState.FoundIt)
        return false;
      this._detectionDetail = new DetectionDetail(charsetProber);
      return true;
    }

    private void FindInputState(byte[] buf, int offset, int len)
    {
      for (int index = offset; index < len; ++index)
      {
        if (((int) buf[index] & 128) != 0 && buf[index] != (byte) 160)
        {
          if (this.InputState != InputState.Highbyte)
          {
            this.InputState = InputState.Highbyte;
            this._escCharsetProber = (IList<CharsetProber>) null;
            this._charsetProbers = this._charsetProbers ?? this.GetNewProbers();
          }
        }
        else
        {
          if (this.InputState == InputState.PureASCII && (buf[index] == (byte) 27 || buf[index] == (byte) 123 && this._lastChar == (byte) 126))
          {
            this.InputState = InputState.EscASCII;
            this._escCharsetProber = this._escCharsetProber ?? this.GetNewProbers();
          }
          this._lastChar = buf[index];
        }
      }
    }

    private static string FindCharSetByBom(byte[] buf, int offset, int len)
    {
      if (len < 2)
        return (string) null;
      byte num1 = buf[offset];
      byte num2 = buf[offset + 1];
      if (num1 == (byte) 254 && num2 == byte.MaxValue)
        return len <= 3 || buf[offset + 2] != (byte) 0 || buf[offset + 3] != (byte) 0 ? "utf-16be" : "X-ISO-10646-UCS-4-3412";
      if (num1 == byte.MaxValue && num2 == (byte) 254)
        return len <= 3 || buf[offset + 2] != (byte) 0 || buf[offset + 3] != (byte) 0 ? "utf-16le" : "utf-32le";
      if (len < 3)
        return (string) null;
      if (num1 == (byte) 239 && num2 == (byte) 187 && buf[offset + 2] == (byte) 191)
        return "utf-8";
      if (len < 4)
        return (string) null;
      if (num1 == (byte) 0 && num2 == (byte) 0)
      {
        if (buf[offset + 2] == (byte) 254 && buf[offset + 3] == byte.MaxValue)
          return "utf-32be";
        if (buf[offset + 2] == byte.MaxValue && buf[offset + 3] == (byte) 254)
          return "X-ISO-10646-UCS-4-2143";
      }
      if (num1 == (byte) 43 && num2 == (byte) 47 && buf[offset + 2] == (byte) 118 && (buf[offset + 3] == (byte) 56 || buf[offset + 3] == (byte) 57 || buf[offset + 3] == (byte) 43 || buf[offset + 3] == (byte) 47))
        return "utf-7";
      return num1 == (byte) 132 && num2 == (byte) 49 && buf[offset + 2] == (byte) 149 && buf[offset + 3] == (byte) 51 ? "gb18030" : (string) null;
    }

    private DetectionResult DataEnd()
    {
      if (!this._gotData)
        return new DetectionResult();
      if (this._detectionDetail != null)
      {
        this._done = true;
        this._detectionDetail.Confidence = 1f;
        return new DetectionResult(this._detectionDetail);
      }
      if (this.InputState == InputState.Highbyte)
        return new DetectionResult((IList<DetectionDetail>) this._charsetProbers.Select<CharsetProber, DetectionDetail>((Func<CharsetProber, DetectionDetail>) (prober => new DetectionDetail(prober))).Where<DetectionDetail>((Func<DetectionDetail, bool>) (result => (double) result.Confidence > 0.20000000298023224)).OrderByDescending<DetectionDetail, float>((Func<DetectionDetail, float>) (result => result.Confidence)).ToList<DetectionDetail>());
      if (this.InputState == InputState.PureASCII)
        return new DetectionResult(new DetectionDetail("ascii", 1f));
      return this.InputState == InputState.EscASCII ? new DetectionResult(new DetectionDetail("ascii", 1f)) : new DetectionResult();
    }

    internal IList<CharsetProber> GetNewProbers()
    {
      switch (this.InputState)
      {
        case InputState.EscASCII:
          return (IList<CharsetProber>) new List<CharsetProber>()
          {
            (CharsetProber) new EscCharsetProber()
          };
        case InputState.Highbyte:
          return (IList<CharsetProber>) new List<CharsetProber>()
          {
            (CharsetProber) new MBCSGroupProber(),
            (CharsetProber) new SBCSGroupProber(),
            (CharsetProber) new Latin1Prober()
          };
        default:
          return (IList<CharsetProber>) new List<CharsetProber>();
      }
    }
  }
}
