// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.SingleByteCharSetProber
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System;
using UtfUnknown.Core.Models;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public class SingleByteCharSetProber : CharsetProber
  {
    private const int SB_ENOUGH_REL_THRESHOLD = 1024;
    private const float POSITIVE_SHORTCUT_THRESHOLD = 0.95f;
    private const float NEGATIVE_SHORTCUT_THRESHOLD = 0.05f;
    private const int SYMBOL_CAT_ORDER = 250;
    private const int NUMBER_OF_SEQ_CAT = 4;
    private const int POSITIVE_CAT = 3;
    private const int PROBABLE_CAT = 2;
    private const int NEUTRAL_CAT = 1;
    private const int NEGATIVE_CAT = 0;
    protected SequenceModel model;
    private bool reversed;
    private byte lastOrder;
    private int totalSeqs;
    private int[] seqCounters = new int[4];
    private int totalChar;
    private int ctrlChar;
    private int freqChar;
    private CharsetProber nameProber;

    public SingleByteCharSetProber(SequenceModel model)
      : this(model, false, (CharsetProber) null)
    {
    }

    public SingleByteCharSetProber(SequenceModel model, bool reversed, CharsetProber nameProber)
    {
      this.model = model;
      this.reversed = reversed;
      this.nameProber = nameProber;
      this.Reset();
    }

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      int num = offset + len;
      for (int index = offset; index < num; ++index)
      {
        byte order = this.model.GetOrder(buf[index]);
        if (order < (byte) 250)
        {
          ++this.totalChar;
        }
        else
        {
          switch (order)
          {
            case 254:
              ++this.ctrlChar;
              break;
            case byte.MaxValue:
              this.state = ProbingState.NotMe;
              goto label_15;
          }
        }
        if ((int) order < this.model.FreqCharCount)
        {
          ++this.freqChar;
          if ((int) this.lastOrder < this.model.FreqCharCount)
          {
            ++this.totalSeqs;
            if (!this.reversed)
              ++this.seqCounters[(int) this.model.GetPrecedence((int) this.lastOrder * this.model.FreqCharCount + (int) order)];
            else
              ++this.seqCounters[(int) this.model.GetPrecedence((int) order * this.model.FreqCharCount + (int) this.lastOrder)];
          }
        }
        this.lastOrder = order;
      }
label_15:
      if (this.state == ProbingState.Detecting && this.totalSeqs > 1024)
      {
        float confidence = this.GetConfidence((System.Text.StringBuilder) null);
        if ((double) confidence > 0.949999988079071)
          this.state = ProbingState.FoundIt;
        else if ((double) confidence < 0.05000000074505806)
          this.state = ProbingState.NotMe;
      }
      return this.state;
    }

    public override string DumpStatus()
    {
      System.Text.StringBuilder stringBuilder1 = new System.Text.StringBuilder();
      System.Text.StringBuilder stringBuilder2 = stringBuilder1;
      System.Text.StringBuilder stringBuilder3 = stringBuilder2;
      System.Text.StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new System.Text.StringBuilder.AppendInterpolatedStringHandler(11, 2, stringBuilder2);
      interpolatedStringHandler.AppendLiteral("  SBCS: ");
      interpolatedStringHandler.AppendFormatted<float>(this.GetConfidence((System.Text.StringBuilder) null), "0.00############");
      interpolatedStringHandler.AppendLiteral(" [");
      interpolatedStringHandler.AppendFormatted(this.GetCharsetName());
      interpolatedStringHandler.AppendLiteral("]");
      ref System.Text.StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.AppendLine(ref local);
      return stringBuilder1.ToString();
    }

    private void StringBuilder(string v1, float v2, string v3) => throw new NotImplementedException();

    public override float GetConfidence(System.Text.StringBuilder status = null)
    {
      if (this.totalSeqs <= 0)
        return 0.01f;
      float confidence = 1f * (float) this.seqCounters[3] / (float) this.totalSeqs / this.model.TypicalPositiveRatio * ((float) this.seqCounters[3] + (float) this.seqCounters[2] / 4f) / (float) this.totalChar * (float) (this.totalChar - this.ctrlChar) / (float) this.totalChar * (float) this.freqChar / (float) this.totalChar;
      if ((double) confidence >= 1.0)
        confidence = 0.99f;
      return confidence;
    }

    public override void Reset()
    {
      this.state = ProbingState.Detecting;
      this.lastOrder = byte.MaxValue;
      for (int index = 0; index < 4; ++index)
        this.seqCounters[index] = 0;
      this.totalSeqs = 0;
      this.totalChar = 0;
      this.freqChar = 0;
    }

    public override string GetCharsetName() => this.nameProber == null ? this.model.CharsetName : this.nameProber.GetCharsetName();
  }
}
