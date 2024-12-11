﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.Estonian.Iso_8859_15_EstonianModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.Estonian
{
  public class Iso_8859_15_EstonianModel : EstonianModel
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
      (byte) 0,
      (byte) 19,
      (byte) 23,
      (byte) 10,
      (byte) 2,
      (byte) 22,
      (byte) 15,
      (byte) 16,
      (byte) 1,
      (byte) 17,
      (byte) 8,
      (byte) 5,
      (byte) 12,
      (byte) 7,
      (byte) 9,
      (byte) 14,
      (byte) 28,
      (byte) 11,
      (byte) 3,
      (byte) 4,
      (byte) 6,
      (byte) 13,
      (byte) 27,
      (byte) 26,
      (byte) 25,
      (byte) 30,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 0,
      (byte) 19,
      (byte) 23,
      (byte) 10,
      (byte) 2,
      (byte) 22,
      (byte) 15,
      (byte) 16,
      (byte) 1,
      (byte) 17,
      (byte) 8,
      (byte) 5,
      (byte) 12,
      (byte) 7,
      (byte) 9,
      (byte) 14,
      (byte) 28,
      (byte) 11,
      (byte) 3,
      (byte) 4,
      (byte) 6,
      (byte) 13,
      (byte) 27,
      (byte) 26,
      (byte) 25,
      (byte) 30,
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
      (byte) 29,
      (byte) 253,
      (byte) 29,
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
      (byte) 32,
      (byte) 50,
      (byte) 253,
      (byte) 253,
      (byte) 32,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 129,
      (byte) 130,
      (byte) 131,
      (byte) 253,
      (byte) 40,
      (byte) 43,
      (byte) 132,
      (byte) 133,
      (byte) 18,
      (byte) 44,
      (byte) 47,
      (byte) 48,
      (byte) 41,
      (byte) 33,
      (byte) 134,
      (byte) 135,
      (byte) 35,
      (byte) 36,
      (byte) 136,
      (byte) 137,
      (byte) 46,
      (byte) 138,
      (byte) 53,
      (byte) 42,
      (byte) 139,
      (byte) 20,
      (byte) 24,
      (byte) 253,
      (byte) 38,
      (byte) 54,
      (byte) 52,
      (byte) 140,
      (byte) 21,
      (byte) 141,
      (byte) 142,
      (byte) 143,
      (byte) 40,
      (byte) 43,
      (byte) 144,
      (byte) 145,
      (byte) 18,
      (byte) 44,
      (byte) 47,
      (byte) 48,
      (byte) 41,
      (byte) 33,
      (byte) 146,
      (byte) 147,
      (byte) 35,
      (byte) 36,
      (byte) 148,
      (byte) 149,
      (byte) 46,
      (byte) 150,
      (byte) 53,
      (byte) 42,
      (byte) 151,
      (byte) 20,
      (byte) 24,
      (byte) 253,
      (byte) 38,
      (byte) 54,
      (byte) 52,
      (byte) 152,
      (byte) 21,
      (byte) 153,
      (byte) 154,
      (byte) 155
    };

    public Iso_8859_15_EstonianModel()
      : base(Iso_8859_15_EstonianModel.CHAR_TO_ORDER_MAP, "iso-8859-15")
    {
    }
  }
}
