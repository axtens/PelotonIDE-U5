// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.MultiByte.UTF8Prober
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.Text;
using UtfUnknown.Core.Models;
using UtfUnknown.Core.Models.MultiByte;


#nullable enable
namespace UtfUnknown.Core.Probers.MultiByte
{
  public class UTF8Prober : CharsetProber
  {
    private static float ONE_CHAR_PROB = 0.5f;
    private CodingStateMachine codingSM;
    private int numOfMBChar;

    public UTF8Prober()
    {
      this.numOfMBChar = 0;
      this.codingSM = new CodingStateMachine((StateMachineModel) new UTF8_SMModel());
      this.Reset();
    }

    public override string GetCharsetName() => "utf-8";

    public override void Reset()
    {
      this.codingSM.Reset();
      this.numOfMBChar = 0;
      this.state = ProbingState.Detecting;
    }

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      int num = offset + len;
      for (int index = offset; index < num; ++index)
      {
        switch (this.codingSM.NextState(buf[index]))
        {
          case 0:
            if (this.codingSM.CurrentCharLen >= 2)
              ++this.numOfMBChar;
            break;
          case 1:
            this.state = ProbingState.NotMe;
            goto label_9;
          case 2:
            this.state = ProbingState.FoundIt;
            goto label_9;
        }
      }
label_9:
      if (this.state == ProbingState.Detecting && (double) this.GetConfidence((StringBuilder) null) > 0.949999988079071)
        this.state = ProbingState.FoundIt;
      return this.state;
    }

    public override float GetConfidence(StringBuilder status = null)
    {
      float num = 0.99f;
      float confidence;
      if (this.numOfMBChar < 6)
      {
        for (int index = 0; index < this.numOfMBChar; ++index)
          num *= UTF8Prober.ONE_CHAR_PROB;
        confidence = 1f - num;
      }
      else
        confidence = 0.99f;
      return confidence;
    }
  }
}
