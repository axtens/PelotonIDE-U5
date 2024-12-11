// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Chinese.EUCTWSMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Chinese
{
  public class EUCTWSMModel : StateMachineModel
  {
    private static readonly int[] EUCTW_cls = new int[32]
    {
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 0, 0),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 0, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 6, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 3, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(5, 5, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 3, 1, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 0)
    };
    private static readonly int[] EUCTW_st = new int[6]
    {
      BitPackage.Pack4bits(1, 1, 0, 3, 3, 3, 4, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 1, 0, 1),
      BitPackage.Pack4bits(0, 0, 0, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(5, 1, 1, 1, 0, 1, 0, 0),
      BitPackage.Pack4bits(0, 1, 0, 0, 0, 0, 0, 0)
    };
    private static readonly int[] EUCTWCharLenTable = new int[7]
    {
      0,
      0,
      1,
      2,
      2,
      2,
      3
    };

    public EUCTWSMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, EUCTWSMModel.EUCTW_cls), 7, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, EUCTWSMModel.EUCTW_st), EUCTWSMModel.EUCTWCharLenTable, "euc-tw")
    {
    }
  }
}
