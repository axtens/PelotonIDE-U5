// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Korean.Iso_2022_KR_SMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Korean
{
  public class Iso_2022_KR_SMModel : StateMachineModel
  {
    private static readonly int[] ISO2022KR_cls = new int[32]
    {
      BitPackage.Pack4bits(2, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 1, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 3, 0, 0, 0),
      BitPackage.Pack4bits(0, 4, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 5, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
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
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2)
    };
    private static readonly int[] ISO2022KR_st = new int[5]
    {
      BitPackage.Pack4bits(0, 3, 1, 0, 0, 0, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 1, 1, 1, 4, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 5, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 2, 0, 0, 0, 0)
    };
    private static readonly int[] ISO2022KRCharLenTable = new int[6];

    public Iso_2022_KR_SMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, Iso_2022_KR_SMModel.ISO2022KR_cls), 6, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, Iso_2022_KR_SMModel.ISO2022KR_st), Iso_2022_KR_SMModel.ISO2022KRCharLenTable, "iso-2022-kr")
    {
    }
  }
}
