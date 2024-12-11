// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Japanese.EUCJPSMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Japanese
{
  public class EUCJPSMModel : StateMachineModel
  {
    private static readonly int[] EUCJP_cls = new int[32]
    {
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 5, 5),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 5, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),
      BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 1, 3),
      BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),
      BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),
      BitPackage.Pack4bits(5, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 5)
    };
    private static readonly int[] EUCJP_st = new int[5]
    {
      BitPackage.Pack4bits(3, 4, 3, 5, 0, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 0, 1, 0, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 0, 1, 1, 1, 3, 1),
      BitPackage.Pack4bits(3, 1, 1, 1, 0, 0, 0, 0)
    };
    private static readonly int[] EUCJPCharLenTable = new int[6]
    {
      2,
      2,
      2,
      3,
      1,
      0
    };

    public EUCJPSMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, EUCJPSMModel.EUCJP_cls), 6, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, EUCJPSMModel.EUCJP_st), EUCJPSMModel.EUCJPCharLenTable, "euc-jp")
    {
    }
  }
}
