﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.Lithuanian.Iso_8859_4_LithuanianModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.Lithuanian
{
  public class Iso_8859_4_LithuanianModel : LithuanianModel
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
      (byte) 1,
      (byte) 18,
      (byte) 23,
      (byte) 12,
      (byte) 4,
      (byte) 25,
      (byte) 16,
      (byte) 26,
      (byte) 0,
      (byte) 14,
      (byte) 9,
      (byte) 10,
      (byte) 11,
      (byte) 6,
      (byte) 3,
      (byte) 15,
      (byte) 37,
      (byte) 5,
      (byte) 2,
      (byte) 7,
      (byte) 8,
      (byte) 13,
      (byte) 33,
      (byte) 32,
      (byte) 19,
      (byte) 27,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 1,
      (byte) 18,
      (byte) 23,
      (byte) 12,
      (byte) 4,
      (byte) 25,
      (byte) 16,
      (byte) 26,
      (byte) 0,
      (byte) 14,
      (byte) 9,
      (byte) 10,
      (byte) 11,
      (byte) 6,
      (byte) 3,
      (byte) 15,
      (byte) 37,
      (byte) 5,
      (byte) 2,
      (byte) 7,
      (byte) 8,
      (byte) 13,
      (byte) 33,
      (byte) 32,
      (byte) 19,
      (byte) 27,
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
      (byte) 29,
      (byte) 92,
      (byte) 93,
      (byte) 253,
      (byte) 94,
      (byte) 56,
      (byte) 253,
      (byte) 253,
      (byte) 21,
      (byte) 50,
      (byte) 95,
      (byte) 96,
      (byte) 253,
      (byte) 22,
      (byte) 253,
      (byte) 253,
      (byte) 29,
      (byte) 253,
      (byte) 97,
      (byte) 253,
      (byte) 98,
      (byte) 56,
      (byte) 253,
      (byte) 253,
      (byte) 21,
      (byte) 50,
      (byte) 99,
      (byte) 100,
      (byte) 101,
      (byte) 22,
      (byte) 102,
      (byte) 41,
      (byte) 39,
      (byte) 103,
      (byte) 53,
      (byte) 38,
      (byte) 43,
      (byte) 104,
      (byte) 30,
      (byte) 24,
      (byte) 36,
      (byte) 31,
      (byte) 105,
      (byte) 17,
      (byte) 40,
      (byte) 106,
      (byte) 47,
      (byte) 55,
      (byte) 57,
      (byte) 34,
      (byte) 107,
      (byte) 59,
      (byte) 108,
      (byte) 35,
      (byte) 253,
      (byte) 48,
      (byte) 20,
      (byte) 54,
      (byte) 109,
      (byte) 45,
      (byte) 110,
      (byte) 28,
      (byte) 52,
      (byte) 41,
      (byte) 39,
      (byte) 111,
      (byte) 53,
      (byte) 38,
      (byte) 43,
      (byte) 112,
      (byte) 30,
      (byte) 24,
      (byte) 36,
      (byte) 31,
      (byte) 113,
      (byte) 17,
      (byte) 40,
      (byte) 114,
      (byte) 47,
      (byte) 55,
      (byte) 57,
      (byte) 34,
      (byte) 115,
      (byte) 59,
      (byte) 116,
      (byte) 35,
      (byte) 253,
      (byte) 48,
      (byte) 20,
      (byte) 54,
      (byte) 117,
      (byte) 45,
      (byte) 118,
      (byte) 28,
      (byte) 253
    };

    public Iso_8859_4_LithuanianModel()
      : base(Iso_8859_4_LithuanianModel.CHAR_TO_ORDER_MAP, "iso-8859-4")
    {
    }
  }
}
