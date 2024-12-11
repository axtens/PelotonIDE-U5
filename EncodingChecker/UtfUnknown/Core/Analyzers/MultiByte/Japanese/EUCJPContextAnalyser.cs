// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Analyzers.Japanese.EUCJPContextAnalyser
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Analyzers.Japanese
{
  public class EUCJPContextAnalyser : JapaneseContextAnalyser
  {
    private const byte HIRAGANA_FIRST_BYTE = 164;

    protected override int GetOrder(byte[] buf, int offset, out int charLen)
    {
      byte num1 = buf[offset];
      int num2 = num1 == (byte) 142 ? 1 : (num1 < (byte) 161 ? 0 : (num1 <= (byte) 254 ? 1 : 0));
      charLen = num2 == 0 ? (num1 != (byte) 191 ? 1 : 3) : 2;
      if (num1 == (byte) 164)
      {
        byte num3 = buf[offset + 1];
        if (num3 >= (byte) 161 && num3 <= (byte) 243)
          return (int) num3 - 161;
      }
      return -1;
    }

    protected override int GetOrder(byte[] buf, int offset)
    {
      if (buf[offset] == (byte) 164)
      {
        byte num = buf[offset + 1];
        if (num >= (byte) 161 && num <= (byte) 243)
          return (int) num - 161;
      }
      return -1;
    }
  }
}
