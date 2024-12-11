// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.CharsetProber
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.IO;
using System.Text;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public abstract class CharsetProber
  {
    protected const float SHORTCUT_THRESHOLD = 0.95f;
    protected ProbingState state;
    private const byte SPACE = 32;
    private const byte CAPITAL_A = 65;
    private const byte CAPITAL_Z = 90;
    private const byte SMALL_A = 97;
    private const byte SMALL_Z = 122;
    private const byte LESS_THAN = 60;
    private const byte GREATER_THAN = 62;

    public abstract ProbingState HandleData(byte[] buf, int offset, int len);

    public abstract void Reset();

    public abstract string GetCharsetName();

    public abstract float GetConfidence(StringBuilder status = null);

    public virtual ProbingState GetState() => this.state;

    public virtual void SetOption()
    {
    }

    public virtual string DumpStatus() => string.Empty;

    protected static byte[] FilterWithoutEnglishLetters(byte[] buf, int offset, int len)
    {
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream(buf.Length))
      {
        bool flag = false;
        int num1 = offset + len;
        int offset1 = offset;
        int index;
        for (index = offset; index < num1; ++index)
        {
          byte num2 = buf[index];
          if (((uint) num2 & 128U) > 0U)
            flag = true;
          else if (num2 < (byte) 65 || num2 > (byte) 90 && num2 < (byte) 97 || num2 > (byte) 122)
          {
            if (flag && index > offset1)
            {
              memoryStream.Write(buf, offset1, index - offset1);
              memoryStream.WriteByte((byte) 32);
              flag = false;
            }
            offset1 = index + 1;
          }
        }
        if (flag && index > offset1)
          memoryStream.Write(buf, offset1, index - offset1);
        memoryStream.SetLength(memoryStream.Position);
        array = memoryStream.ToArray();
      }
      return array;
    }

    protected static byte[] FilterWithEnglishLetters(byte[] buf, int offset, int len)
    {
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream(buf.Length))
      {
        bool flag = false;
        int num1 = offset + len;
        int offset1 = offset;
        int index;
        for (index = offset; index < num1; ++index)
        {
          byte num2 = buf[index];
          switch (num2)
          {
            case 60:
              flag = true;
              break;
            case 62:
              flag = false;
              break;
          }
          if (((int) num2 & 128) == 0 && (num2 < (byte) 65 || num2 > (byte) 122 || num2 > (byte) 90 && num2 < (byte) 97))
          {
            if (index > offset1 && !flag)
            {
              memoryStream.Write(buf, offset1, index - offset1);
              memoryStream.WriteByte((byte) 32);
            }
            offset1 = index + 1;
          }
        }
        if (!flag && index > offset1)
          memoryStream.Write(buf, offset1, index - offset1);
        memoryStream.SetLength(memoryStream.Position);
        array = memoryStream.ToArray();
      }
      return array;
    }
  }
}
