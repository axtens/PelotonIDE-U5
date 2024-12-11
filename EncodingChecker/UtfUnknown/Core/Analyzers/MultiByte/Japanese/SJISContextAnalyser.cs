// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Analyzers.Japanese.SJISContextAnalyser
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Analyzers.Japanese
{
  public class SJISContextAnalyser : JapaneseContextAnalyser
  {
    private const byte HIRAGANA_FIRST_BYTE = 130;

    protected override int GetOrder(byte[] buf, int offset, out int charLen)
    {
      int num1 = buf[offset] < (byte) 129 || buf[offset] > (byte) 159 ? (buf[offset] < (byte) 224 ? 0 : (buf[offset] <= (byte) 252 ? 1 : 0)) : 1;
      charLen = num1 == 0 ? 1 : 2;
      if (buf[offset] == (byte) 130)
      {
        byte num2 = buf[offset + 1];
        if (num2 >= (byte) 159 && num2 <= (byte) 241)
          return (int) num2 - 159;
      }
      return -1;
    }

    protected override int GetOrder(byte[] buf, int offset)
    {
      if (buf[offset] == (byte) 130)
      {
        byte num = buf[offset + 1];
        if (num >= (byte) 159 && num <= (byte) 241)
          return (int) num - 159;
      }
      return -1;
    }
  }
}
