// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Chinese.GB18030_SMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Chinese
{
  public class GB18030_SMModel : StateMachineModel
  {
    private static readonly int[] GB18030_cls = new int[32]
    {
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 0, 0),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 0, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 4),
      BitPackage.Pack4bits(5, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 0)
    };
    private static readonly int[] GB18030_st = new int[6]
    {
      BitPackage.Pack4bits(1, 0, 0, 0, 0, 0, 3, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 1, 1, 0),
      BitPackage.Pack4bits(4, 1, 0, 0, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 5, 1, 1, 1, 2, 1),
      BitPackage.Pack4bits(1, 1, 0, 0, 0, 0, 0, 0)
    };
    private static readonly int[] GB18030CharLenTable = new int[7]
    {
      0,
      1,
      1,
      1,
      1,
      1,
      2
    };

    public GB18030_SMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, GB18030_SMModel.GB18030_cls), 7, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, GB18030_SMModel.GB18030_st), GB18030_SMModel.GB18030CharLenTable, "gb18030")
    {
    }
  }
}
