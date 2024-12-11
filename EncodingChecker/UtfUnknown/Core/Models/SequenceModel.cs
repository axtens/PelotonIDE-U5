// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Models.SequenceModel
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Models
{
  public abstract class SequenceModel
  {
    public const byte ILL = 255;
    public const byte CTR = 254;
    public const byte SYM = 253;
    public const byte RET = 252;
    public const byte NUM = 251;
    protected byte[] charToOrderMap;
    protected byte[] precedenceMatrix;
    protected int freqCharCount;
    protected float typicalPositiveRatio;
    protected bool keepEnglishLetter;
    protected string charsetName;

    public int FreqCharCount => this.freqCharCount;

    public float TypicalPositiveRatio => this.typicalPositiveRatio;

    public bool KeepEnglishLetter => this.keepEnglishLetter;

    public string CharsetName => this.charsetName;

    public SequenceModel(
      byte[] charToOrderMap,
      byte[] precedenceMatrix,
      int freqCharCount,
      float typicalPositiveRatio,
      bool keepEnglishLetter,
      string charsetName)
    {
      this.charToOrderMap = charToOrderMap;
      this.precedenceMatrix = precedenceMatrix;
      this.freqCharCount = freqCharCount;
      this.typicalPositiveRatio = typicalPositiveRatio;
      this.keepEnglishLetter = keepEnglishLetter;
      this.charsetName = charsetName;
    }

    public byte GetOrder(byte b) => this.charToOrderMap[(int) b];

    public byte GetPrecedence(int pos) => this.precedenceMatrix[pos];
  }
}
