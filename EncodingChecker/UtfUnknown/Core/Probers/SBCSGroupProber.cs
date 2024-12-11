// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Probers.SBCSGroupProber
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System.Text;
using UtfUnknown.Core.Models;
using UtfUnknown.Core.Models.SingleByte.Arabic;
using UtfUnknown.Core.Models.SingleByte.Bulgarian;
using UtfUnknown.Core.Models.SingleByte.Croatian;
using UtfUnknown.Core.Models.SingleByte.Czech;
using UtfUnknown.Core.Models.SingleByte.Danish;
using UtfUnknown.Core.Models.SingleByte.Esperanto;
using UtfUnknown.Core.Models.SingleByte.Estonian;
using UtfUnknown.Core.Models.SingleByte.Finnish;
using UtfUnknown.Core.Models.SingleByte.French;
using UtfUnknown.Core.Models.SingleByte.German;
using UtfUnknown.Core.Models.SingleByte.Greek;
using UtfUnknown.Core.Models.SingleByte.Hebrew;
using UtfUnknown.Core.Models.SingleByte.Hungarian;
using UtfUnknown.Core.Models.SingleByte.Irish;
using UtfUnknown.Core.Models.SingleByte.Italian;
using UtfUnknown.Core.Models.SingleByte.Latvian;
using UtfUnknown.Core.Models.SingleByte.Lithuanian;
using UtfUnknown.Core.Models.SingleByte.Maltese;
using UtfUnknown.Core.Models.SingleByte.Polish;
using UtfUnknown.Core.Models.SingleByte.Portuguese;
using UtfUnknown.Core.Models.SingleByte.Romanian;
using UtfUnknown.Core.Models.SingleByte.Russian;
using UtfUnknown.Core.Models.SingleByte.Slovak;
using UtfUnknown.Core.Models.SingleByte.Slovene;
using UtfUnknown.Core.Models.SingleByte.Spanish;
using UtfUnknown.Core.Models.SingleByte.Swedish;
using UtfUnknown.Core.Models.SingleByte.Thai;
using UtfUnknown.Core.Models.SingleByte.Turkish;
using UtfUnknown.Core.Models.SingleByte.Vietnamese;


#nullable enable
namespace UtfUnknown.Core.Probers
{
  public class SBCSGroupProber : CharsetProber
  {
    private const int PROBERS_NUM = 100;
    private CharsetProber[] probers = new CharsetProber[100];
    private bool[] isActive = new bool[100];
    private int bestGuess;
    private int activeNum;

    public SBCSGroupProber()
    {
      this.probers[0] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1251_RussianModel());
      this.probers[1] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Koi8r_Model());
      this.probers[2] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_5_RussianModel());
      this.probers[3] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new X_Mac_Cyrillic_RussianModel());
      this.probers[4] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm866_RussianModel());
      this.probers[5] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm855_RussianModel());
      this.probers[6] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_7_GreekModel());
      this.probers[7] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1253_GreekModel());
      this.probers[8] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_5_BulgarianModel());
      this.probers[9] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1251_BulgarianModel());
      HebrewProber nameProber = new HebrewProber();
      this.probers[10] = (CharsetProber) nameProber;
      this.probers[11] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1255_HebrewModel(), false, (CharsetProber) nameProber);
      this.probers[12] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1255_HebrewModel(), true, (CharsetProber) nameProber);
      nameProber.SetModelProbers(this.probers[11], this.probers[12]);
      this.probers[13] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Tis_620_ThaiModel());
      this.probers[14] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_11_ThaiModel());
      this.probers[15] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_FrenchModel());
      this.probers[16] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_FrenchModel());
      this.probers[17] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_FrenchModel());
      this.probers[18] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_SpanishModel());
      this.probers[19] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_SpanishModel());
      this.probers[20] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_SpanishModel());
      this.probers[21] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_2_HungarianModel());
      this.probers[22] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1250_HungarianModel());
      this.probers[23] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_GermanModel());
      this.probers[24] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_GermanModel());
      this.probers[25] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_3_EsperantoModel());
      this.probers[26] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_3_TurkishModel());
      this.probers[27] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_9_TurkishModel());
      this.probers[28] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_6_ArabicModel());
      this.probers[29] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1256_ArabicModel());
      this.probers[30] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Viscii_VietnameseModel());
      this.probers[31] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1258_VietnameseModel());
      this.probers[32] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_DanishModel());
      this.probers[33] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_DanishModel());
      this.probers[34] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_DanishModel());
      this.probers[35] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_13_LithuanianModel());
      this.probers[36] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_10_LithuanianModel());
      this.probers[37] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_4_LithuanianModel());
      this.probers[38] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_13_LatvianModel());
      this.probers[39] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_10_LatvianModel());
      this.probers[40] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_4_LatvianModel());
      this.probers[41] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_PortugueseModel());
      this.probers[42] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_9_PortugueseModel());
      this.probers[43] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_PortugueseModel());
      this.probers[44] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_PortugueseModel());
      this.probers[45] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_3_MalteseModel());
      this.probers[46] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1250_CzechModel());
      this.probers[47] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_2_CzechModel());
      this.probers[48] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Mac_Centraleurope_CzechModel());
      this.probers[49] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm852_CzechModel());
      this.probers[50] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1250_SlovakModel());
      this.probers[51] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_2_SlovakModel());
      this.probers[52] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Mac_Centraleurope_SlovakModel());
      this.probers[53] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm852_SlovakModel());
      this.probers[54] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1250_PolishModel());
      this.probers[55] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_2_PolishModel());
      this.probers[56] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_13_PolishModel());
      this.probers[57] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_16_PolishModel());
      this.probers[58] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Mac_Centraleurope_PolishModel());
      this.probers[59] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm852_PolishModel());
      this.probers[60] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_FinnishModel());
      this.probers[61] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_4_FinnishModel());
      this.probers[62] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_9_FinnishModel());
      this.probers[63] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_13_FinnishModel());
      this.probers[64] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_FinnishModel());
      this.probers[65] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_FinnishModel());
      this.probers[66] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_ItalianModel());
      this.probers[67] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_3_ItalianModel());
      this.probers[68] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_9_ItalianModel());
      this.probers[69] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_ItalianModel());
      this.probers[70] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_ItalianModel());
      this.probers[71] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1250_CroatianModel());
      this.probers[72] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_2_CroatianModel());
      this.probers[73] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_13_CroatianModel());
      this.probers[74] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_16_CroatianModel());
      this.probers[75] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Mac_Centraleurope_CroatianModel());
      this.probers[76] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm852_CroatianModel());
      this.probers[77] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_EstonianModel());
      this.probers[78] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1257_EstonianModel());
      this.probers[79] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_4_EstonianModel());
      this.probers[80] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_13_EstonianModel());
      this.probers[81] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_EstonianModel());
      this.probers[82] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_IrishModel());
      this.probers[83] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_9_IrishModel());
      this.probers[84] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_IrishModel());
      this.probers[85] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_IrishModel());
      this.probers[86] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1250_RomanianModel());
      this.probers[87] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_2_RomanianModel());
      this.probers[88] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_16_RomanianModel());
      this.probers[89] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm852_RomanianModel());
      this.probers[90] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1250_SloveneModel());
      this.probers[91] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_2_SloveneModel());
      this.probers[92] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_16_SloveneModel());
      this.probers[93] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Mac_Centraleurope_SloveneModel());
      this.probers[94] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Ibm852_SloveneModel());
      this.probers[95] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_1_SwedishModel());
      this.probers[96] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_4_SwedishModel());
      this.probers[97] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_9_SwedishModel());
      this.probers[98] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Iso_8859_15_SwedishModel());
      this.probers[99] = (CharsetProber) new SingleByteCharSetProber((SequenceModel) new Windows_1252_SwedishModel());
      this.Reset();
    }

    public override ProbingState HandleData(byte[] buf, int offset, int len)
    {
      byte[] buf1 = CharsetProber.FilterWithoutEnglishLetters(buf, offset, len);
      if (buf1.Length == 0)
        return this.state;
      for (int index = 0; index < 100; ++index)
      {
        if (this.isActive[index])
        {
          switch (this.probers[index].HandleData(buf1, 0, buf1.Length))
          {
            case ProbingState.FoundIt:
              this.bestGuess = index;
              this.state = ProbingState.FoundIt;
              goto label_11;
            case ProbingState.NotMe:
              this.isActive[index] = false;
              --this.activeNum;
              if (this.activeNum <= 0)
              {
                this.state = ProbingState.NotMe;
                goto label_11;
              }
              else
                break;
          }
        }
      }
label_11:
      return this.state;
    }

    public override float GetConfidence(StringBuilder status = null)
    {
      float confidence1 = 0.0f;
      switch (this.state)
      {
        case ProbingState.FoundIt:
          return 0.99f;
        case ProbingState.NotMe:
          return 0.01f;
        default:
          status?.AppendLine("Get confidence:");
          for (int index = 0; index < 100; ++index)
          {
            if (this.isActive[index])
            {
              float confidence2 = this.probers[index].GetConfidence();
              if ((double) confidence1 < (double) confidence2)
              {
                confidence1 = confidence2;
                this.bestGuess = index;
                if (status != null)
                {
                  StringBuilder stringBuilder1 = status;
                  StringBuilder stringBuilder2 = stringBuilder1;
                  StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(50, 3, stringBuilder1);
                  interpolatedStringHandler.AppendLiteral("-- new match found: confidence ");
                  interpolatedStringHandler.AppendFormatted<float>(confidence1);
                  interpolatedStringHandler.AppendLiteral(", index ");
                  interpolatedStringHandler.AppendFormatted<int>(this.bestGuess);
                  interpolatedStringHandler.AppendLiteral(", charset ");
                  interpolatedStringHandler.AppendFormatted(this.probers[index].GetCharsetName());
                  interpolatedStringHandler.AppendLiteral(".");
                  ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
                  stringBuilder2.AppendLine(ref local);
                }
              }
            }
          }
          status?.AppendLine("Get confidence done.");
          return confidence1;
      }
    }

    public override string DumpStatus()
    {
      StringBuilder status = new StringBuilder();
      float confidence1 = this.GetConfidence(status);
      status.AppendLine(" SBCS Group Prober --------begin status");
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      for (int index = 0; index < 100; ++index)
      {
        if (this.probers[index] != null)
        {
          if (!this.isActive[index])
          {
            StringBuilder stringBuilder1 = status;
            StringBuilder stringBuilder2 = stringBuilder1;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(48, 1, stringBuilder1);
            interpolatedStringHandler.AppendLiteral(" SBCS inactive: [");
            interpolatedStringHandler.AppendFormatted(this.probers[index].GetCharsetName());
            interpolatedStringHandler.AppendLiteral("] (i.e. confidence is too low).");
            ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
            stringBuilder2.AppendLine(ref local);
          }
          else
          {
            float confidence2 = this.probers[index].GetConfidence();
            StringBuilder stringBuilder3 = status;
            StringBuilder stringBuilder4 = stringBuilder3;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(10, 2, stringBuilder3);
            interpolatedStringHandler.AppendLiteral(" SBCS ");
            interpolatedStringHandler.AppendFormatted<float>(confidence2);
            interpolatedStringHandler.AppendLiteral(": [");
            interpolatedStringHandler.AppendFormatted(this.probers[index].GetCharsetName());
            interpolatedStringHandler.AppendLiteral("]");
            ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
            stringBuilder4.AppendLine(ref local);
            status.AppendLine(this.probers[index].DumpStatus());
          }
        }
      }
      StringBuilder stringBuilder5 = status;
      StringBuilder stringBuilder6 = stringBuilder5;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(44, 2, stringBuilder5);
      interpolatedStringHandler.AppendLiteral(" SBCS Group found best match [");
      interpolatedStringHandler.AppendFormatted(this.probers[this.bestGuess].GetCharsetName());
      interpolatedStringHandler.AppendLiteral("] confidence ");
      interpolatedStringHandler.AppendFormatted<float>(confidence1);
      interpolatedStringHandler.AppendLiteral(".");
      ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
      stringBuilder6.AppendLine(ref local1);
      return status.ToString();
    }

    public override void Reset()
    {
      this.activeNum = 0;
      for (int index = 0; index < 100; ++index)
      {
        if (this.probers[index] != null)
        {
          this.probers[index].Reset();
          this.isActive[index] = true;
          ++this.activeNum;
        }
        else
          this.isActive[index] = false;
      }
      this.bestGuess = -1;
      this.state = ProbingState.Detecting;
    }

    public override string GetCharsetName()
    {
      if (this.bestGuess == -1)
      {
        double confidence = (double) this.GetConfidence((StringBuilder) null);
        if (this.bestGuess == -1)
          this.bestGuess = 0;
      }
      return this.probers[this.bestGuess].GetCharsetName();
    }
  }
}
