﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.Turkish.Iso_8859_9_TurkishModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.Turkish
{
  public class Iso_8859_9_TurkishModel : TurkishModel
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
      (byte) 15,
      (byte) 21,
      (byte) 7,
      (byte) 1,
      (byte) 26,
      (byte) 22,
      (byte) 19,
      (byte) 6,
      (byte) 28,
      (byte) 9,
      (byte) 5,
      (byte) 11,
      (byte) 3,
      (byte) 14,
      (byte) 23,
      (byte) 34,
      (byte) 4,
      (byte) 10,
      (byte) 8,
      (byte) 12,
      (byte) 20,
      (byte) 29,
      (byte) 32,
      (byte) 13,
      (byte) 18,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 0,
      (byte) 15,
      (byte) 21,
      (byte) 7,
      (byte) 1,
      (byte) 26,
      (byte) 22,
      (byte) 19,
      (byte) 2,
      (byte) 28,
      (byte) 9,
      (byte) 5,
      (byte) 11,
      (byte) 3,
      (byte) 14,
      (byte) 23,
      (byte) 34,
      (byte) 4,
      (byte) 10,
      (byte) 8,
      (byte) 12,
      (byte) 20,
      (byte) 29,
      (byte) 32,
      (byte) 13,
      (byte) 18,
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
      (byte) 81,
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
      (byte) 36,
      (byte) 30,
      (byte) 44,
      (byte) 39,
      (byte) 82,
      (byte) 46,
      (byte) 24,
      (byte) 42,
      (byte) 33,
      (byte) 83,
      (byte) 45,
      (byte) 84,
      (byte) 37,
      (byte) 31,
      (byte) 85,
      (byte) 25,
      (byte) 47,
      (byte) 86,
      (byte) 38,
      (byte) 87,
      (byte) 88,
      (byte) 27,
      (byte) 253,
      (byte) 43,
      (byte) 89,
      (byte) 40,
      (byte) 35,
      (byte) 16,
      (byte) 2,
      (byte) 17,
      (byte) 90,
      (byte) 41,
      (byte) 36,
      (byte) 30,
      (byte) 44,
      (byte) 39,
      (byte) 91,
      (byte) 46,
      (byte) 24,
      (byte) 42,
      (byte) 33,
      (byte) 92,
      (byte) 45,
      (byte) 93,
      (byte) 37,
      (byte) 31,
      (byte) 94,
      (byte) 25,
      (byte) 47,
      (byte) 95,
      (byte) 38,
      (byte) 96,
      (byte) 97,
      (byte) 27,
      (byte) 253,
      (byte) 43,
      (byte) 98,
      (byte) 40,
      (byte) 35,
      (byte) 16,
      (byte) 6,
      (byte) 17,
      (byte) 99
    };

    public Iso_8859_9_TurkishModel()
      : base(Iso_8859_9_TurkishModel.CHAR_TO_ORDER_MAP, "iso-8859-9")
    {
    }
  }
}