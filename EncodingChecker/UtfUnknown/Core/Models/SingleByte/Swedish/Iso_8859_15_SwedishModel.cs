﻿// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SingleByte.Swedish.Iso_8859_15_SwedishModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models.SingleByte.Swedish
{
  public class Iso_8859_15_SwedishModel : SwedishModel
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
      (byte) 22,
      (byte) 20,
      (byte) 9,
      (byte) 1,
      (byte) 14,
      (byte) 12,
      (byte) 18,
      (byte) 6,
      (byte) 23,
      (byte) 10,
      (byte) 7,
      (byte) 11,
      (byte) 3,
      (byte) 8,
      (byte) 15,
      (byte) 30,
      (byte) 2,
      (byte) 5,
      (byte) 4,
      (byte) 16,
      (byte) 13,
      (byte) 26,
      (byte) 25,
      (byte) 24,
      (byte) 27,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 0,
      (byte) 22,
      (byte) 20,
      (byte) 9,
      (byte) 1,
      (byte) 14,
      (byte) 12,
      (byte) 18,
      (byte) 6,
      (byte) 23,
      (byte) 10,
      (byte) 7,
      (byte) 11,
      (byte) 3,
      (byte) 8,
      (byte) 15,
      (byte) 30,
      (byte) 2,
      (byte) 5,
      (byte) 4,
      (byte) 16,
      (byte) 13,
      (byte) 26,
      (byte) 25,
      (byte) 24,
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
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 213,
      (byte) 253,
      (byte) 214,
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
      (byte) 215,
      (byte) 216,
      (byte) 253,
      (byte) 253,
      (byte) 217,
      (byte) 253,
      (byte) 253,
      (byte) 253,
      (byte) 218,
      (byte) 219,
      (byte) 220,
      (byte) 253,
      (byte) 221,
      (byte) 44,
      (byte) 222,
      (byte) 223,
      (byte) 17,
      (byte) 19,
      (byte) 38,
      (byte) 40,
      (byte) 32,
      (byte) 28,
      (byte) 45,
      (byte) 224,
      (byte) 225,
      (byte) 226,
      (byte) 47,
      (byte) 227,
      (byte) 228,
      (byte) 229,
      (byte) 230,
      (byte) 231,
      (byte) 35,
      (byte) 232,
      (byte) 21,
      (byte) 253,
      (byte) 37,
      (byte) 233,
      (byte) 234,
      (byte) 235,
      (byte) 31,
      (byte) 236,
      (byte) 237,
      (byte) 238,
      (byte) 239,
      (byte) 44,
      (byte) 240,
      (byte) 241,
      (byte) 17,
      (byte) 19,
      (byte) 38,
      (byte) 40,
      (byte) 32,
      (byte) 28,
      (byte) 45,
      (byte) 242,
      (byte) 243,
      (byte) 244,
      (byte) 47,
      (byte) 245,
      (byte) 246,
      (byte) 247,
      (byte) 248,
      (byte) 249,
      (byte) 35,
      (byte) 249,
      (byte) 21,
      (byte) 253,
      (byte) 37,
      (byte) 249,
      (byte) 249,
      (byte) 249,
      (byte) 31,
      (byte) 249,
      (byte) 249,
      (byte) 249
    };

    public Iso_8859_15_SwedishModel()
      : base(Iso_8859_15_SwedishModel.CHAR_TO_ORDER_MAP, "iso-8859-15")
    {
    }
  }
}