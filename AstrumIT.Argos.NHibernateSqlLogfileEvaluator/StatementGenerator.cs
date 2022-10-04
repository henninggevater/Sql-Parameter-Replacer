// Decompiled with JetBrains decompiler
// Type: AstrumIT.Argos.NHibernateSqlLogfileEvaluator.StatementGenerator
// Assembly: AstrumIT.Argos.NHibernateSqlLogfileEvaluator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D97E7A68-CDEE-43E7-8E40-A752249DB11E
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\AstrumIT.Argos.NHibernateSqlLogfileEvaluator.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
  public class StatementGenerator
  {
    private static readonly Regex varNameReg = new Regex("^\\s*(?<varName>" + Statement.ParameterScheme + ") = (?<varValue>.+)\\s*\\[Type:\\s*(?<varType>\\w+)\\s*\\(\\S+\\)\\s*\\]\\s*$");

    public static Statement GenerateStatement(string statementWithArguments)
    {
      if (statementWithArguments == null)
        return (Statement) null;
      List<string> list = ((IEnumerable<string>) statementWithArguments.Replace(":p", "@p").Split(';')).ToList<string>();
      if (list.Count > 2)
        return (Statement) null;
      return list.Count == 1 ? new Statement(statementWithArguments) : new Statement(list[0], StatementGenerator.ExtractParamsAndValues(list[1]));
    }

    private static Dictionary<string, string> ExtractParamsAndValues(string argStr)
    {
      Dictionary<string, string> paramsAndValues = new Dictionary<string, string>();
      string str = argStr;
      char[] chArray = new char[1]{ ',' };
      foreach (string singleArg in ((IEnumerable<string>) str.Split(chArray)).ToList<string>())
        StatementGenerator.AddParamValueEntry(paramsAndValues, singleArg);
      return paramsAndValues;
    }

    private static void AddParamValueEntry(
      Dictionary<string, string> paramsAndValues,
      string singleArg)
    {
      Match match = StatementGenerator.varNameReg.Match(singleArg);
      string key = match.Groups["varName"].Value.Trim();
      string type = match.Groups["varType"].Value.Trim();
      string str = match.Groups["varValue"].Value.Trim();
      if (!(key != "") || !(type != "") || !(str != "") || paramsAndValues.ContainsKey(key))
        return;
      paramsAndValues.Add(key, StatementGenerator.FormatArgument(type, str));
    }

    private protected static string FormatArgument(string type, string value)
    {
      if (type == typeof (DateTime).Name)
        return StatementGenerator.FormatDateTime(value);
      if (type == typeof (bool).Name)
        return StatementGenerator.FormatBoolean(value);
      return type == typeof (Guid).Name ? StatementGenerator.QuoteArgument(value) : value;
    }

    private static string QuoteArgument(string input) => string.Format("'{0}'", (object) input);

    private static string FormatDateTime(string input)
    {
      DateTime result;
      if (DateTime.TryParseExact(input, "dd.MM.yyyy HH:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) || DateTime.TryParseExact(input, "MM/dd/yyyy hh:mm:ss tt", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) || DateTime.TryParse(input, out result))
        return result.ToString("\\'yyyy-MM-ddTHH:mm:ss\\'");
      throw new FormatException();
    }

    private static string FormatBoolean(string input) => !bool.Parse(input) ? "0" : "1";
  }
}
