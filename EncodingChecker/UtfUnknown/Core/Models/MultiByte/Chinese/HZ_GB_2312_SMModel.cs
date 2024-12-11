// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Chinese.HZ_GB_2312_SMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Chinese
{
  public class HZ_GB_2312_SMModel : StateMachineModel
  {
    private static readonly int[] HZ_cls = new int[32]
    {
      BitPackage.Pack4bits(1, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 1, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 4, 0, 5, 2, 0),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1)
    };
    private static readonly int[] HZ_st = new int[6]
    {
      BitPackage.Pack4bits(0, 1, 3, 0, 0, 0, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 1, 1, 0, 0, 4, 1),
      BitPackage.Pack4bits(5, 1, 6, 1, 5, 5, 4, 1),
      BitPackage.Pack4bits(4, 1, 4, 4, 4, 1, 4, 1),
      BitPackage.Pack4bits(4, 2, 0, 0, 0, 0, 0, 0)
    };
    private static readonly int[] HZCharLenTable = new int[6];

    public HZ_GB_2312_SMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, HZ_GB_2312_SMModel.HZ_cls), 6, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, HZ_GB_2312_SMModel.HZ_st), HZ_GB_2312_SMModel.HZCharLenTable, "hz-gb-2312")
    {
    }
  }
}
