// Decompiled with JetBrains decompiler
// Type: AstrumIT.Argos.NHibernateSqlLogfileEvaluator.Statement
// Assembly: AstrumIT.Argos.NHibernateSqlLogfileEvaluator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D97E7A68-CDEE-43E7-8E40-A752249DB11E
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\AstrumIT.Argos.NHibernateSqlLogfileEvaluator.exe

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
  public class Statement
  {
    public static string ParameterScheme = "@p\\d+";
    private readonly string statementBase;
    private readonly Dictionary<string, string> parametersAndValues;
    private readonly string executable;

    public Statement(string statementBase, Dictionary<string, string> parametersAndValues)
   {
      this.parametersAndValues = parametersAndValues;
      this.statementBase = statementBase;
      this.executable = Regex.Replace(this.StatementBase, Statement.ParameterScheme, (MatchEvaluator) (match => Statement.SelectValue(this.ParametersAndValues, match)));
    }

    public Statement(string statementBase)
      : this(statementBase, new Dictionary<string, string>())
    {
    }

    public Dictionary<string, string> ParametersAndValues => this.parametersAndValues;

    public string Executable => this.executable;

    public string StatementBase => this.statementBase;

    private static string SelectValue(Dictionary<string, string> parameterValuePairs, Match param)
    {
      string str;
      if (parameterValuePairs.TryGetValue(param.Value, out str))
        return str;
      throw new Exception(string.Format("{0} not found.", (object) param.Value));
    }

    public override bool Equals(object o) => o is Statement statement && this.StatementBase == statement.StatementBase;

    public override int GetHashCode() => this.StatementBase.GetHashCode();
  }
}
