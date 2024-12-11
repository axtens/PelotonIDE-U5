// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.HebrewProber
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.Text;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public class HebrewProber : CharsetProber
  {
    private const byte FINAL_KAF = 234;
    private const byte NORMAL_KAF = 235;
    private const byte FINAL_MEM = 237;
    private const byte NORMAL_MEM = 238;
    private const byte FINAL_NUN = 239;
    private const byte NORMAL_NUN = 240;
    private const byte FINAL_PE = 243;
    private const byte NORMAL_PE = 244;
    private const byte FINAL_TSADI = 245;
    private const byte NORMAL_TSADI = 246;
    private const int MIN_FINAL_CHAR_DISTANCE = 5;
    private const float MIN_MODEL_DISTANCE = 0.01f;
    protected const string VISUAL_NAME = "iso-8859-8";
    protected const string LOGICAL_NAME = "windows-1255";
    protected CharsetProber logicalProber;
    protected CharsetProber visualProber;
    protected int finalCharLogicalScore;
    protected int finalCharVisualScore;
    protected byte prev;
    protected byte beforePrev;

    public HebrewProber() => this.Reset();

    public void SetModelProbers(CharsetProber logical, CharsetProber visual)
    {
      this.logicalProber = logical;
      this.visualProber = visual;
    }

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      if (this.GetState() == ProbingState.NotMe)
        return ProbingState.NotMe;
      int num1 = offset + len;
      for (int index = offset; index < num1; ++index)
      {
        byte num2 = buf[index];
        if (num2 == (byte) 32)
        {
          if (this.beforePrev != (byte) 32)
          {
            if (HebrewProber.IsFinal(this.prev))
              ++this.finalCharLogicalScore;
            else if (HebrewProber.IsNonFinal(this.prev))
              ++this.finalCharVisualScore;
          }
        }
        else if (this.beforePrev == (byte) 32 && HebrewProber.IsFinal(this.prev) && num2 != (byte) 32)
          ++this.finalCharVisualScore;
        this.beforePrev = this.prev;
        this.prev = num2;
      }
      return ProbingState.Detecting;
    }

    public override string GetCharsetName()
    {
      int num1 = this.finalCharLogicalScore - this.finalCharVisualScore;
      if (num1 >= 5)
        return "windows-1255";
      if (num1 <= -5)
        return "iso-8859-8";
      float num2 = this.logicalProber.GetConfidence() - this.visualProber.GetConfidence();
      return (double) num2 > 0.0099999997764825821 || (double) num2 >= -0.0099999997764825821 && num1 >= 0 ? "windows-1255" : "iso-8859-8";
    }

    public override void Reset()
    {
      this.finalCharLogicalScore = 0;
      this.finalCharVisualScore = 0;
      this.prev = (byte) 32;
      this.beforePrev = (byte) 32;
    }

    public override ProbingState GetState() => this.logicalProber.GetState() == ProbingState.NotMe && this.visualProber.GetState() == ProbingState.NotMe ? ProbingState.NotMe : ProbingState.Detecting;

    public override string DumpStatus()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(33, 2, stringBuilder2);
      interpolatedStringHandler.AppendLiteral("  HEB: ");
      interpolatedStringHandler.AppendFormatted<int>(this.finalCharLogicalScore);
      interpolatedStringHandler.AppendLiteral(" - ");
      interpolatedStringHandler.AppendFormatted<int>(this.finalCharVisualScore);
      interpolatedStringHandler.AppendLiteral(" [Logical-Visual score]");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.AppendLine(ref local);
      return stringBuilder1.ToString();
    }

    public override float GetConfidence(StringBuilder status = null) => 0.0f;

    protected static bool IsFinal(byte b) => b == (byte) 234 || b == (byte) 237 || b == (byte) 239 || b == (byte) 243 || b == (byte) 245;

    protected static bool IsNonFinal(byte b) => b == (byte) 235 || b == (byte) 238 || b == (byte) 240 || b == (byte) 244;
  }
}
