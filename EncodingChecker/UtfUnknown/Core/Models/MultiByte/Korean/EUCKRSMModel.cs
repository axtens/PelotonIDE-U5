// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Korean.EUCKRSMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Korean
{
  public class EUCKRSMModel : StateMachineModel
  {
    private static readonly int[] EUCKR_cls = new int[32]
    {
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 0, 0),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 0, 1, 1, 1, 1),
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
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 3, 3, 3),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 3, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 0)
    };
    private static readonly int[] EUCKR_st = new int[2]
    {
      BitPackage.Pack4bits(1, 0, 3, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(2, 2, 2, 2, 1, 1, 0, 0)
    };
    private static readonly int[] EUCKRCharLenTable = new int[4]
    {
      0,
      1,
      2,
      0
    };

    public EUCKRSMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, EUCKRSMModel.EUCKR_cls), 4, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, EUCKRSMModel.EUCKR_st), EUCKRSMModel.EUCKRCharLenTable, "euc-kr")
    {
    }
  }
}
