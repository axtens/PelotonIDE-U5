// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.MultiByte.Korean.EUCKRProber
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.Text;
using UtfUnknown.Core.Analyzers.Korean;
using UtfUnknown.Core.Models;
using UtfUnknown.Core.Models.MultiByte.Korean;


#nullable enable
namespace UtfUnknown.Core.Probers.MultiByte.Korean
{
  public class EUCKRProber : CharsetProber
  {
    private CodingStateMachine codingSM;
    private EUCKRDistributionAnalyser distributionAnalyser;
    private byte[] lastChar = new byte[2];

    public EUCKRProber()
    {
      this.codingSM = new CodingStateMachine((StateMachineModel) new EUCKRSMModel());
      this.distributionAnalyser = new EUCKRDistributionAnalyser();
      this.Reset();
    }

    public override string GetCharsetName() => "euc-kr";

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      int num = offset + len;
      for (int index = offset; index < num; ++index)
      {
        switch (this.codingSM.NextState(buf[index]))
        {
          case 0:
            int currentCharLen = this.codingSM.CurrentCharLen;
            if (index == offset)
            {
              this.lastChar[1] = buf[offset];
              this.distributionAnalyser.HandleOneChar(this.lastChar, 0, currentCharLen);
            }
            else
              this.distributionAnalyser.HandleOneChar(buf, index - 1, currentCharLen);
            break;
          case 1:
            this.state = ProbingState.NotMe;
            goto label_10;
          case 2:
            this.state = ProbingState.FoundIt;
            goto label_10;
        }
      }
label_10:
      this.lastChar[0] = buf[num - 1];
      if (this.state == ProbingState.Detecting && this.distributionAnalyser.GotEnoughData() && (double) this.GetConfidence((StringBuilder) null) > 0.949999988079071)
        this.state = ProbingState.FoundIt;
      return this.state;
    }

    public override float GetConfidence(StringBuilder status = null) => this.distributionAnalyser.GetConfidence();

    public override void Reset()
    {
      this.codingSM.Reset();
      this.state = ProbingState.Detecting;
      this.distributionAnalyser.Reset();
    }
  }
}
