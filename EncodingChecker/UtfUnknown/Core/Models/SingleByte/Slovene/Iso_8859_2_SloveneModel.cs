﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.Slovene.Iso_8859_2_SloveneModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.Slovene
{
  public class Iso_8859_2_SloveneModel : SloveneModel
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
      (byte) 18,
      (byte) 19,
      (byte) 13,
      (byte) 1,
      (byte) 24,
      (byte) 17,
      (byte) 20,
      (byte) 2,
      (byte) 8,
      (byte) 12,
      (byte) 9,
      (byte) 14,
      (byte) 4,
      (byte) 3,
      (byte) 11,
      (byte) 28,
      (byte) 5,
      (byte) 6,
      (byte) 7,
      (byte) 16,
      (byte) 10,
      (byte) 27,
      (byte) 25,
      (byte) 26,
      (byte) 15,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 0,
      (byte) 18,
      (byte) 19,
      (byte) 13,
      (byte) 1,
      (byte) 24,
      (byte) 17,
      (byte) 20,
      (byte) 2,
      (byte) 8,
      (byte) 12,
      (byte) 9,
      (byte) 14,
      (byte) 4,
      (byte) 3,
      (byte) 11,
      (byte) 28,
      (byte) 5,
      (byte) 6,
      (byte) 7,
      (byte) 16,
      (byte) 10,
      (byte) 27,
      (byte) 25,
      (byte) 26,
      (byte) 15,
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
      (byte) 41,
      (byte) 253,
      (byte) 42,
      (byte) 253,
      (byte) 43,
      (byte) 44,
      (byte) 253,
      (byte) 253,
      (byte) 22,
      (byte) 45,
      (byte) 46,
      (byte) 47,
      (byte) 253,
      (byte) 23,
      (byte) 48,
      (byte) 253,
      (byte) 49,
      (byte) 253,
      (byte) 50,
      (byte) 253,
      (byte) 51,
      (byte) 52,
      (byte) 253,
      (byte) 253,
      (byte) 22,
      (byte) 53,
      (byte) 54,
      (byte) 55,
      (byte) 253,
      (byte) 23,
      (byte) 56,
      (byte) 57,
      (byte) 32,
      (byte) 58,
      (byte) 59,
      (byte) 60,
      (byte) 61,
      (byte) 37,
      (byte) 34,
      (byte) 21,
      (byte) 29,
      (byte) 62,
      (byte) 36,
      (byte) 63,
      (byte) 30,
      (byte) 64,
      (byte) 65,
      (byte) 66,
      (byte) 67,
      (byte) 68,
      (byte) 31,
      (byte) 35,
      (byte) 69,
      (byte) 70,
      (byte) 253,
      (byte) 71,
      (byte) 72,
      (byte) 39,
      (byte) 73,
      (byte) 74,
      (byte) 40,
      (byte) 75,
      (byte) 76,
      (byte) 77,
      (byte) 32,
      (byte) 78,
      (byte) 79,
      (byte) 80,
      (byte) 81,
      (byte) 37,
      (byte) 34,
      (byte) 21,
      (byte) 29,
      (byte) 82,
      (byte) 36,
      (byte) 83,
      (byte) 30,
      (byte) 84,
      (byte) 85,
      (byte) 86,
      (byte) 87,
      (byte) 88,
      (byte) 31,
      (byte) 35,
      (byte) 89,
      (byte) 90,
      (byte) 253,
      (byte) 91,
      (byte) 92,
      (byte) 39,
      (byte) 93,
      (byte) 94,
      (byte) 40,
      (byte) 95,
      (byte) 253
    };

    public Iso_8859_2_SloveneModel()
      : base(Iso_8859_2_SloveneModel.CHAR_TO_ORDER_MAP, "iso-8859-2")
    {
    }
  }
}