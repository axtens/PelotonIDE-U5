// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.MBCSGroupProber
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.Text;
using UtfUnknown.Core.Probers.MultiByte;
using UtfUnknown.Core.Probers.MultiByte.Chinese;
using UtfUnknown.Core.Probers.MultiByte.Japanese;
using UtfUnknown.Core.Probers.MultiByte.Korean;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public class MBCSGroupProber : CharsetProber
  {
    private const int PROBERS_NUM = 8;
    private CharsetProber[] probers = new CharsetProber[8];
    private bool[] isActive = new bool[8];
    private int bestGuess;
    private int activeNum;

    public MBCSGroupProber()
    {
      this.probers[0] = (CharsetProber) new UTF8Prober();
      this.probers[1] = (CharsetProber) new SJISProber();
      this.probers[2] = (CharsetProber) new EUCJPProber();
      this.probers[3] = (CharsetProber) new GB18030Prober();
      this.probers[4] = (CharsetProber) new EUCKRProber();
      this.probers[5] = (CharsetProber) new CP949Prober();
      this.probers[6] = (CharsetProber) new Big5Prober();
      this.probers[7] = (CharsetProber) new EUCTWProber();
      this.Reset();
    }

    public override string GetCharsetName()
    {
      if (this.bestGuess == -1)
      {
        double confidence = (double) this.GetConfidence((StringBuilder) null);
        if (this.bestGuess == -1)
          this.bestGuess = 0;
      }
      return this.probers[this.bestGuess].GetCharsetName();
    }

    public override void Reset()
    {
      this.activeNum = 0;
      for (int index = 0; index < this.probers.Length; ++index)
      {
        if (this.probers[index] != null)
        {
          this.probers[index].Reset();
          this.isActive[index] = true;
          ++this.activeNum;
        }
        else
          this.isActive[index] = false;
      }
      this.bestGuess = -1;
      this.state = ProbingState.Detecting;
    }

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      byte[] buf1 = new byte[len];
      int len1 = 0;
      bool flag = true;
      int num = offset + len;
      for (int index = offset; index < num; ++index)
      {
        if (((uint) buf[index] & 128U) > 0U)
        {
          buf1[len1++] = buf[index];
          flag = true;
        }
        else if (flag)
        {
          buf1[len1++] = buf[index];
          flag = false;
        }
      }
      for (int index = 0; index < this.probers.Length; ++index)
      {
        if (this.isActive[index])
        {
          switch (this.probers[index].HandleData(buf1, 0, len1))
          {
            case ProbingState.FoundIt:
              this.bestGuess = index;
              this.state = ProbingState.FoundIt;
              goto label_17;
            case ProbingState.NotMe:
              this.isActive[index] = false;
              --this.activeNum;
              if (this.activeNum <= 0)
              {
                this.state = ProbingState.NotMe;
                goto label_17;
              }
              else
                break;
          }
        }
      }
label_17:
      return this.state;
    }

    public override float GetConfidence(StringBuilder status = null)
    {
      float confidence1 = 0.0f;
      switch (this.state)
      {
        case ProbingState.FoundIt:
          return 0.99f;
        case ProbingState.NotMe:
          return 0.01f;
        default:
          status?.AppendLine("Get confidence:");
          for (int index = 0; index < 8; ++index)
          {
            if (this.isActive[index])
            {
              float confidence2 = this.probers[index].GetConfidence();
              if ((double) confidence1 < (double) confidence2)
              {
                confidence1 = confidence2;
                this.bestGuess = index;
                if (status != null)
                {
                  StringBuilder stringBuilder1 = status;
                  StringBuilder stringBuilder2 = stringBuilder1;
                  StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(50, 3, stringBuilder1);
                  interpolatedStringHandler.AppendLiteral("-- new match found: confidence ");
                  interpolatedStringHandler.AppendFormatted<float>(confidence1);
                  interpolatedStringHandler.AppendLiteral(", index ");
                  interpolatedStringHandler.AppendFormatted<int>(this.bestGuess);
                  interpolatedStringHandler.AppendLiteral(", charset ");
                  interpolatedStringHandler.AppendFormatted(this.probers[index].GetCharsetName());
                  interpolatedStringHandler.AppendLiteral(".");
                  ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
                  stringBuilder2.AppendLine(ref local);
                }
              }
            }
          }
          status?.AppendLine("Get confidence done.");
          return confidence1;
      }
    }

    public override string DumpStatus()
    {
      StringBuilder status = new StringBuilder();
      float confidence1 = this.GetConfidence(status);
      status.AppendLine(" MBCS Group Prober --------begin status");
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      for (int index = 0; index < 8; ++index)
      {
        if (this.probers[index] != null)
        {
          if (!this.isActive[index])
          {
            StringBuilder stringBuilder1 = status;
            StringBuilder stringBuilder2 = stringBuilder1;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(46, 1, stringBuilder1);
            interpolatedStringHandler.AppendLiteral(" MBCS inactive: ");
            interpolatedStringHandler.AppendFormatted(this.probers[index].GetCharsetName());
            interpolatedStringHandler.AppendLiteral(" (i.e. confidence is too low).");
            ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
            stringBuilder2.AppendLine(ref local);
          }
          else
          {
            float confidence2 = this.probers[index].GetConfidence();
            StringBuilder stringBuilder3 = status;
            StringBuilder stringBuilder4 = stringBuilder3;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(10, 2, stringBuilder3);
            interpolatedStringHandler.AppendLiteral(" MBCS ");
            interpolatedStringHandler.AppendFormatted<float>(confidence2);
            interpolatedStringHandler.AppendLiteral(": [");
            interpolatedStringHandler.AppendFormatted(this.probers[index].GetCharsetName());
            interpolatedStringHandler.AppendLiteral("]");
            ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
            stringBuilder4.AppendLine(ref local);
            status.AppendLine(this.probers[index].DumpStatus());
          }
        }
      }
      StringBuilder stringBuilder5 = status;
      StringBuilder stringBuilder6 = stringBuilder5;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(44, 2, stringBuilder5);
      interpolatedStringHandler.AppendLiteral(" MBCS Group found best match [");
      interpolatedStringHandler.AppendFormatted(this.probers[this.bestGuess].GetCharsetName());
      interpolatedStringHandler.AppendLiteral("] confidence ");
      interpolatedStringHandler.AppendFormatted<float>(confidence1);
      interpolatedStringHandler.AppendLiteral(".");
      ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
      stringBuilder6.AppendLine(ref local1);
      return status.ToString();
    }
  }
}
