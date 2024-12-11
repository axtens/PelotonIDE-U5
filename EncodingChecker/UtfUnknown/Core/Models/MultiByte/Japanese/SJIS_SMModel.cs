// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Japanese.SJIS_SMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Japanese
{
  public class SJIS_SMModel : StateMachineModel
  {
    private static readonly int[] SJIS_cls = new int[32]
    {
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 0, 0),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 0, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 1),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),
      BitPackage.Pack4bits(3, 3, 3, 3, 3, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 0, 0, 0)
    };
    private static readonly int[] SJIS_st = new int[3]
    {
      BitPackage.Pack4bits(1, 0, 0, 3, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 1, 1, 0, 0, 0, 0)
    };
    private static readonly int[] SJISCharLenTable = new int[6]
    {
      0,
      1,
      1,
      2,
      0,
      0
    };

    public SJIS_SMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, SJIS_SMModel.SJIS_cls), 6, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, SJIS_SMModel.SJIS_st), SJIS_SMModel.SJISCharLenTable, "shift-jis")
    {
    }
  }
}
