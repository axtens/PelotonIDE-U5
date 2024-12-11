// Decompiled with JetBrains decompiler
// Type: UtfUnknown.Core.Analyzers.Japanese.EUCJPDistributionAnalyser
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll


#nullable enable
namespace UtfUnknown.Core.Analyzers.Japanese
{
  public class EUCJPDistributionAnalyser : SJISDistributionAnalyser
  {
    public override int GetOrder(byte[] buf, int offset) => buf[offset] >= (byte) 160 ? 94 * ((int) buf[offset] - 161) + (int) buf[offset + 1] - 161 : -1;
  }
}
