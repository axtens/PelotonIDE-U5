﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.Portuguese.Iso_8859_1_PortugueseModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.Portuguese
{
  public class Iso_8859_1_PortugueseModel : PortugueseModel
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
      (byte) 17,
      (byte) 10,
      (byte) 7,
      (byte) 1,
      (byte) 16,
      (byte) 14,
      (byte) 18,
      (byte) 4,
      (byte) 28,
      (byte) 34,
      (byte) 12,
      (byte) 9,
      (byte) 6,
      (byte) 2,
      (byte) 13,
      (byte) 21,
      (byte) 5,
      (byte) 3,
      (byte) 8,
      (byte) 11,
      (byte) 15,
      (byte) 32,
      (byte) 24,
      (byte) 31,
      (byte) 26,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 0,
      (byte) 17,
      (byte) 10,
      (byte) 7,
      (byte) 1,
      (byte) 16,
      (byte) 14,
      (byte) 18,
      (byte) 4,
      (byte) 28,
      (byte) 34,
      (byte) 12,
      (byte) 9,
      (byte) 6,
      (byte) 2,
      (byte) 13,
      (byte) 21,
      (byte) 5,
      (byte) 3,
      (byte) 8,
      (byte) 11,
      (byte) 15,
      (byte) 32,
      (byte) 24,
      (byte) 31,
      (byte) 26,
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
      (byte) 51,
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
      (byte) 36,
      (byte) 25,
      (byte) 35,
      (byte) 20,
      (byte) 41,
      (byte) 42,
      (byte) 43,
      (byte) 22,
      (byte) 39,
      (byte) 19,
      (byte) 29,
      (byte) 44,
      (byte) 52,
      (byte) 23,
      (byte) 45,
      (byte) 47,
      (byte) 48,
      (byte) 53,
      (byte) 46,
      (byte) 27,
      (byte) 37,
      (byte) 30,
      (byte) 38,
      (byte) 253,
      (byte) 54,
      (byte) 55,
      (byte) 33,
      (byte) 56,
      (byte) 40,
      (byte) 57,
      (byte) 58,
      (byte) 49,
      (byte) 36,
      (byte) 25,
      (byte) 35,
      (byte) 20,
      (byte) 41,
      (byte) 42,
      (byte) 43,
      (byte) 22,
      (byte) 39,
      (byte) 19,
      (byte) 29,
      (byte) 44,
      (byte) 59,
      (byte) 23,
      (byte) 45,
      (byte) 47,
      (byte) 48,
      (byte) 60,
      (byte) 46,
      (byte) 27,
      (byte) 37,
      (byte) 30,
      (byte) 38,
      (byte) 253,
      (byte) 61,
      (byte) 62,
      (byte) 33,
      (byte) 63,
      (byte) 40,
      (byte) 64,
      (byte) 65,
      (byte) 50
    };

    public Iso_8859_1_PortugueseModel()
      : base(Iso_8859_1_PortugueseModel.CHAR_TO_ORDER_MAP, "iso-8859-1")
    {
    }
  }
}
