// Decompiled with JetBrains decompiler
// Type: AstrumIT.Argos.NHibernateSqlLogfileEvaluator.PageAndStatementTuple
// Assembly: AstrumIT.Argos.NHibernateSqlLogfileEvaluator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D97E7A68-CDEE-43E7-8E40-A752249DB11E
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\AstrumIT.Argos.NHibernateSqlLogfileEvaluator.exe

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
  public class PageAndStatementTuple
  {
    public string Page { get; private set; }

    public string Statement { get; private set; }

    public PageAndStatementTuple(string page, string statement)
    {
      this.Page = page;
      this.Statement = statement;
    }
  }
}
