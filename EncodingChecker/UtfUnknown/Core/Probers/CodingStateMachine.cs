// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.CodingStateMachine
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using UtfUnknown.Core.Models;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public class CodingStateMachine
  {
    private int currentState;
    private StateMachineModel model;
    private int currentCharLen;

    public CodingStateMachine(StateMachineModel model)
    {
      this.currentState = 0;
      this.model = model;
    }

    public int NextState(byte b)
    {
      int index = this.model.GetClass(b);
      if (this.currentState == 0)
        this.currentCharLen = this.model.charLenTable[index];
      this.currentState = this.model.stateTable.Unpack(this.currentState * this.model.ClassFactor + index);
      return this.currentState;
    }

    public void Reset() => this.currentState = 0;

    public int CurrentCharLen => this.currentCharLen;

    public string ModelName => this.model.Name;
  }
}
