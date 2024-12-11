// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.StateMachineModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models
{
  public abstract class StateMachineModel
  {
    public const int START = 0;
    public const int ERROR = 1;
    public const int ITSME = 2;
    public BitPackage classTable;
    public BitPackage stateTable;
    public int[] charLenTable;

    public string Name { get; }

    public int ClassFactor { get; }

    public StateMachineModel(
      BitPackage classTable,
      int classFactor,
      BitPackage stateTable,
      int[] charLenTable,
      string name)
    {
      this.classTable = classTable;
      this.ClassFactor = classFactor;
      this.stateTable = stateTable;
      this.charLenTable = charLenTable;
      this.Name = name;
    }

    public int GetClass(byte b) => this.classTable.Unpack((int) b);
  }
}
