﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.Slovak.Windows_1250_SlovakModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.Slovak
{
  public class Windows_1250_SlovakModel : SlovakModel
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
      (byte) 20,
      (byte) 15,
      (byte) 11,
      (byte) 2,
      (byte) 29,
      (byte) 30,
      (byte) 17,
      (byte) 4,
      (byte) 18,
      (byte) 7,
      (byte) 10,
      (byte) 12,
      (byte) 3,
      (byte) 0,
      (byte) 13,
      (byte) 40,
      (byte) 6,
      (byte) 8,
      (byte) 5,
      (byte) 14,
      (byte) 9,
      (byte) 37,
      (byte) 34,
      (byte) 19,
      (byte) 16,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 1,
      (byte) 20,
      (byte) 15,
      (byte) 11,
      (byte) 2,
      (byte) 29,
      (byte) 30,
      (byte) 17,
      (byte) 4,
      (byte) 18,
      (byte) 7,
      (byte) 10,
      (byte) 12,
      (byte) 3,
      (byte) 0,
      (byte) 13,
      (byte) 40,
      (byte) 6,
      (byte) 8,
      (byte) 5,
      (byte) 14,
      (byte) 9,
      (byte) 37,
      (byte) 34,
      (byte) 19,
      (byte) 16,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 254,
      (byte) 253,
      byte.MaxValue,
      (byte) 253,
      byte.MaxValue,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      byte.MaxValue,
      (byte) 253,
      (byte) 28,
      (byte) 253,
      (byte) 122,
      (byte) 31,
      (byte) 26,
      (byte) 123,
      byte.MaxValue,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      byte.MaxValue,
      (byte) 253,
      (byte) 28,
      (byte) 253,
      (byte) 124,
      (byte) 31,
      (byte) 26,
      (byte) 125,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 49,
      (byte) 253,
      (byte) 126,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 59,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 61,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 49,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 127,
      (byte) 59,
      (byte) 253,
      (byte) 33,
      (byte) 253,
      (byte) 33,
      (byte) 61,
      (byte) 44,
      (byte) 21,
      (byte) 128,
      (byte) 56,
      (byte) 38,
      (byte) 42,
      (byte) 47,
      (byte) 51,
      (byte) 24,
      (byte) 25,
      (byte) 129,
      (byte) 54,
      (byte) 41,
      (byte) 23,
      (byte) 130,
      (byte) 39,
      (byte) 55,
      (byte) 52,
      (byte) 36,
      (byte) 35,
      (byte) 32,
      (byte) 50,
      (byte) 43,
      (byte) 253,
      (byte) 45,
      (byte) 48,
      (byte) 27,
      (byte) 60,
      (byte) 46,
      (byte) 22,
      (byte) 131,
      (byte) 58,
      (byte) 44,
      (byte) 21,
      (byte) 132,
      (byte) 56,
      (byte) 38,
      (byte) 42,
      (byte) 47,
      (byte) 51,
      (byte) 24,
      (byte) 25,
      (byte) 133,
      (byte) 54,
      (byte) 41,
      (byte) 23,
      (byte) 134,
      (byte) 39,
      (byte) 55,
      (byte) 52,
      (byte) 36,
      (byte) 35,
      (byte) 32,
      (byte) 50,
      (byte) 43,
      (byte) 253,
      (byte) 45,
      (byte) 48,
      (byte) 27,
      (byte) 60,
      (byte) 46,
      (byte) 22,
      (byte) 135,
      (byte) 253
    };

    public Windows_1250_SlovakModel()
      : base(Windows_1250_SlovakModel.CHAR_TO_ORDER_MAP, "windows-1250")
    {
    }
  }
}
