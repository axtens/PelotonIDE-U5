// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.Latin1Prober
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.Text;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public class Latin1Prober : CharsetProber
  {
    private const int FREQ_CAT_NUM = 4;
    private const int UDF = 0;
    private const int OTH = 1;
    private const int ASC = 2;
    private const int ASS = 3;
    private const int ACV = 4;
    private const int ACO = 5;
    private const int ASV = 6;
    private const int ASO = 7;
    private const int CLASS_NUM = 8;
    private static readonly byte[] Latin1_CharToClass = new byte[256]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 7,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 5,
      (byte) 1,
      (byte) 5,
      (byte) 0,
      (byte) 5,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 7,
      (byte) 1,
      (byte) 7,
      (byte) 0,
      (byte) 7,
      (byte) 5,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 5,
      (byte) 5,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 5,
      (byte) 5,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 1,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 7,
      (byte) 7,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 7,
      (byte) 7,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 1,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 7,
      (byte) 7,
      (byte) 7
    };
    private static readonly byte[] Latin1ClassModel = new byte[64]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 0,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 0,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3,
      (byte) 0,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 0,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 0,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 0,
      (byte) 3,
      (byte) 1,
      (byte) 3,
      (byte) 1,
      (byte) 1,
      (byte) 3,
      (byte) 3
    };
    private byte lastCharClass;
    private int[] freqCounter = new int[4];

    public Latin1Prober() => this.Reset();

    public override string GetCharsetName() => "windows-1252";

    public override void Reset()
    {
      this.state = ProbingState.Detecting;
      this.lastCharClass = (byte) 1;
      for (int index = 0; index < 4; ++index)
        this.freqCounter[index] = 0;
    }

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      foreach (int withEnglishLetter in CharsetProber.FilterWithEnglishLetters(buf, offset, len))
      {
        byte num = Latin1Prober.Latin1_CharToClass[withEnglishLetter];
        byte index = Latin1Prober.Latin1ClassModel[(int) this.lastCharClass * 8 + (int) num];
        if (index == (byte) 0)
        {
          this.state = ProbingState.NotMe;
          break;
        }
        ++this.freqCounter[(int) index];
        this.lastCharClass = num;
      }
      return this.state;
    }

    public override float GetConfidence(StringBuilder status = null)
    {
      if (this.state == ProbingState.NotMe)
        return 0.01f;
      int num1 = 0;
      for (int index = 0; index < 4; ++index)
        num1 += this.freqCounter[index];
      float num2 = num1 > 0 ? (float) this.freqCounter[3] * 1f / (float) num1 - (float) this.freqCounter[1] * 20f / (float) num1 : 0.0f;
      return (double) num2 < 0.0 ? 0.0f : num2 * 0.5f;
    }

    public override string DumpStatus()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(18, 2, stringBuilder2);
      interpolatedStringHandler.AppendLiteral(" Latin1Prober: ");
      interpolatedStringHandler.AppendFormatted<float>(this.GetConfidence((StringBuilder) null));
      interpolatedStringHandler.AppendLiteral(" [");
      interpolatedStringHandler.AppendFormatted(this.GetCharsetName());
      interpolatedStringHandler.AppendLiteral("]");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.AppendLine(ref local);
      return stringBuilder1.ToString();
    }
  }
}
