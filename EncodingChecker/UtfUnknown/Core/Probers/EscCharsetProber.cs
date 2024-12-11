// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.EscCharsetProber
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.Text;
using UtfUnknown.Core.Models;
using UtfUnknown.Core.Models.MultiByte.Chinese;
using UtfUnknown.Core.Models.MultiByte.Japanese;
using UtfUnknown.Core.Models.MultiByte.Korean;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public class EscCharsetProber : CharsetProber
  {
    private const int CHARSETS_NUM = 4;
    private string detectedCharset;
    private CodingStateMachine[] codingSM;
    private int activeSM;

    public EscCharsetProber()
    {
      this.codingSM = new CodingStateMachine[4];
      this.codingSM[0] = new CodingStateMachine((StateMachineModel) new HZ_GB_2312_SMModel());
      this.codingSM[1] = new CodingStateMachine((StateMachineModel) new Iso_2022_CN_SMModel());
      this.codingSM[2] = new CodingStateMachine((StateMachineModel) new Iso_2022_JP_SMModel());
      this.codingSM[3] = new CodingStateMachine((StateMachineModel) new Iso_2022_KR_SMModel());
      this.Reset();
    }

    public override void Reset()
    {
      this.state = ProbingState.Detecting;
      for (int index = 0; index < 4; ++index)
        this.codingSM[index].Reset();
      this.activeSM = 4;
      this.detectedCharset = (string) null;
    }

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      int num = offset + len;
      for (int index1 = offset; index1 < num && this.state == ProbingState.Detecting; ++index1)
      {
        for (int index2 = this.activeSM - 1; index2 >= 0; --index2)
        {
          switch (this.codingSM[index2].NextState(buf[index1]))
          {
            case 1:
              --this.activeSM;
              if (this.activeSM == 0)
              {
                this.state = ProbingState.NotMe;
                return this.state;
              }
              if (index2 != this.activeSM)
              {
                CodingStateMachine codingStateMachine = this.codingSM[this.activeSM];
                this.codingSM[this.activeSM] = this.codingSM[index2];
                this.codingSM[index2] = codingStateMachine;
                break;
              }
              break;
            case 2:
              this.state = ProbingState.FoundIt;
              this.detectedCharset = this.codingSM[index2].ModelName;
              return this.state;
          }
        }
      }
      return this.state;
    }

    public override string GetCharsetName() => this.detectedCharset;

    public override float GetConfidence(StringBuilder status = null) => 0.99f;
  }
}
