﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.German.Iso_8859_1_GermanModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.German
{
  public class Iso_8859_1_GermanModel : GermanModel
  {
    private static byte[] CHAR_TO_ORDER_MAP = new byte[256]
    {
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 252,
      (byte) 254,
      (byte) 254,
      (byte) 252,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 251,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 5,
      (byte) 15,
      (byte) 12,
      (byte) 8,
      (byte) 0,
      (byte) 17,
      (byte) 14,
      (byte) 7,
      (byte) 3,
      (byte) 23,
      (byte) 16,
      (byte) 9,
      (byte) 13,
      (byte) 2,
      (byte) 11,
      (byte) 18,
      (byte) 30,
      (byte) 1,
      (byte) 4,
      (byte) 6,
      (byte) 10,
      (byte) 21,
      (byte) 19,
      (byte) 28,
      (byte) 25,
      (byte) 20,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 5,
      (byte) 15,
      (byte) 12,
      (byte) 8,
      (byte) 0,
      (byte) 17,
      (byte) 14,
      (byte) 7,
      (byte) 3,
      (byte) 23,
      (byte) 16,
      (byte) 9,
      (byte) 13,
      (byte) 2,
      (byte) 11,
      (byte) 18,
      (byte) 30,
      (byte) 1,
      (byte) 4,
      (byte) 6,
      (byte) 10,
      (byte) 21,
      (byte) 19,
      (byte) 28,
      (byte) 25,
      (byte) 20,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 254,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 65,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 41,
      (byte) 31,
      (byte) 37,
      (byte) 44,
      (byte) 22,
      (byte) 49,
      (byte) 50,
      (byte) 35,
      (byte) 32,
      (byte) 29,
      (byte) 48,
      (byte) 43,
      (byte) 57,
      (byte) 33,
      (byte) 47,
      (byte) 52,
      (byte) 53,
      (byte) 39,
      (byte) 51,
      (byte) 34,
      (byte) 40,
      (byte) 55,
      (byte) 26,
      (byte) 253,
      (byte) 38,
      (byte) 58,
      (byte) 46,
      (byte) 66,
      (byte) 24,
      (byte) 45,
      (byte) 67,
      (byte) 27,
      (byte) 41,
      (byte) 31,
      (byte) 37,
      (byte) 44,
      (byte) 22,
      (byte) 49,
      (byte) 50,
      (byte) 35,
      (byte) 32,
      (byte) 29,
      (byte) 48,
      (byte) 43,
      (byte) 57,
      (byte) 33,
      (byte) 47,
      (byte) 52,
      (byte) 53,
      (byte) 39,
      (byte) 51,
      (byte) 34,
      (byte) 40,
      (byte) 55,
      (byte) 26,
      (byte) 253,
      (byte) 38,
      (byte) 58,
      (byte) 46,
      (byte) 68,
      (byte) 24,
      (byte) 45,
      (byte) 69,
      (byte) 56
    };

    public Iso_8859_1_GermanModel()
      : base(Iso_8859_1_GermanModel.CHAR_TO_ORDER_MAP, "iso-8859-1")
    {
    }
  }
}