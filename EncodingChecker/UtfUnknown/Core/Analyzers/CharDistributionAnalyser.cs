// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Analyzers.CharDistributionAnalyser
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Analyzers
{
  public abstract class CharDistributionAnalyser
  {
    protected const float SURE_YES = 0.99f;
    protected const float SURE_NO = 0.01f;
    protected const int MINIMUM_DATA_THRESHOLD = 4;
    protected const int ENOUGH_DATA_THRESHOLD = 1024;
    protected bool done;
    protected int freqChars;
    protected int totalChars;
    protected int[] charToFreqOrder;
    protected float typicalDistributionRatio;

    public CharDistributionAnalyser() => this.Reset();

    public abstract int GetOrder(byte[] buf, int offset);

    public void HandleOneChar(byte[] buf, int offset, int charLen)
    {
      int index = charLen == 2 ? this.GetOrder(buf, offset) : -1;
      if (index < 0)
        return;
      ++this.totalChars;
      if (index < this.charToFreqOrder.Length && 512 > this.charToFreqOrder[index])
        ++this.freqChars;
    }

    public virtual void Reset()
    {
      this.done = false;
      this.totalChars = 0;
      this.freqChars = 0;
    }

    public virtual float GetConfidence()
    {
      if (this.totalChars <= 0 || this.freqChars <= 4)
        return 0.01f;
      if (this.totalChars != this.freqChars)
      {
        float confidence = (float) this.freqChars / ((float) (this.totalChars - this.freqChars) * this.typicalDistributionRatio);
        if ((double) confidence < 0.99000000953674316)
          return confidence;
      }
      return 0.99f;
    }

    public bool GotEnoughData() => this.totalChars > 1024;
  }
}
