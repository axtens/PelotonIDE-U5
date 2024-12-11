// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.Korean.CP949SMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte.Korean
{
  public class CP949SMModel : StateMachineModel
  {
    private static readonly int[] CP949_cls = new int[32]
    {
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 0, 0),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 0, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),
      BitPackage.Pack4bits(4, 4, 5, 5, 5, 5, 5, 5),
      BitPackage.Pack4bits(5, 5, 5, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 5, 5, 5, 5, 5, 5, 5),
      BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),
      BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),
      BitPackage.Pack4bits(5, 5, 5, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(0, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),
      BitPackage.Pack4bits(6, 7, 7, 7, 7, 7, 7, 7),
      BitPackage.Pack4bits(7, 7, 7, 7, 7, 8, 8, 8),
      BitPackage.Pack4bits(7, 7, 7, 7, 7, 7, 7, 7),
      BitPackage.Pack4bits(7, 7, 7, 7, 7, 7, 7, 7),
      BitPackage.Pack4bits(7, 7, 7, 7, 7, 7, 9, 2),
      BitPackage.Pack4bits(2, 3, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 3, 0)
    };
    private static readonly int[] CP949_st = new int[9]
    {
      BitPackage.Pack4bits(1, 0, 6, 1, 0, 0, 3, 3),
      BitPackage.Pack4bits(4, 5, 1, 1, 1, 1, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 1, 1),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(1, 1, 1, 1, 0, 0, 0, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 0, 0, 0, 1),
      BitPackage.Pack4bits(1, 0, 0, 0, 1, 1, 0, 0),
      BitPackage.Pack4bits(1, 1, 1, 0, 0, 0, 0, 0)
    };
    private static readonly int[] CP949CharLenTable = new int[10]
    {
      0,
      1,
      2,
      0,
      1,
      1,
      2,
      2,
      0,
      2
    };

    public CP949SMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, CP949SMModel.CP949_cls), 10, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, CP949SMModel.CP949_st), CP949SMModel.CP949CharLenTable, "cp949")
    {
    }
  }
}
