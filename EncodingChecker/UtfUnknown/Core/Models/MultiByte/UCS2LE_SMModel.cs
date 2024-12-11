// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.MultiByte.UCS2LE_SMModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.MultiByte
{
  public class UCS2LE_SMModel : StateMachineModel
  {
    private static readonly int[] UCS2LE_cls = new int[32]
    {
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 1, 0, 0, 2, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 3, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 3, 3, 3, 3, 3, 0, 0),
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
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),
      BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 4, 5)
    };
    private static readonly int[] UCS2LE_st = new int[7]
    {
      BitPackage.Pack4bits(6, 6, 7, 6, 4, 3, 1, 1),
      BitPackage.Pack4bits(1, 1, 1, 1, 2, 2, 2, 2),
      BitPackage.Pack4bits(2, 2, 5, 5, 5, 1, 2, 1),
      BitPackage.Pack4bits(5, 5, 5, 1, 5, 1, 6, 6),
      BitPackage.Pack4bits(7, 6, 8, 8, 5, 5, 5, 1),
      BitPackage.Pack4bits(5, 5, 5, 1, 1, 1, 5, 5),
      BitPackage.Pack4bits(5, 5, 5, 1, 5, 1, 0, 0)
    };
    private static readonly int[] UCS2LECharLenTable = new int[6]
    {
      2,
      2,
      2,
      2,
      2,
      2
    };

    public UCS2LE_SMModel()
      : base(new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, UCS2LE_SMModel.UCS2LE_cls), 6, new BitPackage(BitPackage.INDEX_SHIFT_4BITS, BitPackage.SHIFT_MASK_4BITS, BitPackage.BIT_SHIFT_4BITS, BitPackage.UNIT_MASK_4BITS, UCS2LE_SMModel.UCS2LE_st), UCS2LE_SMModel.UCS2LECharLenTable, "utf-16le")
    {
    }
  }
}
