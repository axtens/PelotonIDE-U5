// Decompiled with JetBrains decompiler
// Type: UtfUnknown.DetectionResult
// Assembly: EncodingChecker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEDCF0FC-E3C6-4738-80E4-21B9D554C05E
// Assembly location: C:\Users\bugma\Downloads\P3a\P3\bin\Debug\net7.0\EncodingChecker.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


#nullable enable
namespace UtfUnknown
{
  public class DetectionResult
  {
    public DetectionResult()
    {
    }

    public DetectionResult(IList<DetectionDetail> details) => this.Details = details;

    public DetectionResult(DetectionDetail detectionDetail) => this.Details = (IList<DetectionDetail>) new List<DetectionDetail>()
    {
      detectionDetail
    };

    public DetectionDetail Detected
    {
      get
      {
        IList<DetectionDetail> details = this.Details;
        return details == null ? (DetectionDetail) null : details.FirstOrDefault<DetectionDetail>();
      }
    }

    public IList<DetectionDetail> Details { get; set; }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 4);
      interpolatedStringHandler.AppendFormatted("Detected");
      interpolatedStringHandler.AppendLiteral(": ");
      interpolatedStringHandler.AppendFormatted<DetectionDetail>(this.Detected);
      interpolatedStringHandler.AppendLiteral(", \n");
      interpolatedStringHandler.AppendFormatted("Details");
      interpolatedStringHandler.AppendLiteral(":\n - ");
      ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
      IList<DetectionDetail> details = this.Details;
      string str = string.Join("\n- ", details != null ? details.Select<DetectionDetail, string>((Func<DetectionDetail, string>) (d => d.ToString())) : (IEnumerable<string>) null);
      local.AppendFormatted(str);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
