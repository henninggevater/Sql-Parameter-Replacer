using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
    public class StatemenGeneratorSqlProfiler : StatementGenerator
    {
        private static readonly Regex varNameReg = new Regex(@"(?<varName>\@p\d+) (?<varType>[\w\d\(\)]+)\@p\d+=(?<varValue>[\'\w\d \-\:]*)");
        
        public static new Statement GenerateStatement(string statementWithArguments)
        {
            if (statementWithArguments == null)
                return (Statement)null;
            List<string> list = ((IEnumerable<string>)statementWithArguments.Replace("exec sp_executesql N'", "").Replace("@P", "@p").Split(new [] { "',N'" }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
            var varValues = Regex.Match(list[1], @"(\@p\d+\=[\'\w\d\, \:\-]*)+").ToString();
            if (list.Count > 2)
                return (Statement)null;
            return list.Count == 1 ? new Statement(statementWithArguments) : new Statement(list[0].Trim(), StatemenGeneratorSqlProfiler.ExtractParamsAndValues(list[1], varValues));
        }

        private static Dictionary<string, string> ExtractParamsAndValues(string argStr, string argStr2 = "")
        {
            Dictionary<string, string> paramsAndValues = new Dictionary<string, string>();
            string[] str = argStr.Replace("'","").Split(',');
            string[] str2 = argStr2.Split(',');
            string[] args = new string[str.Length/2];

            for(int i = 0; i < str2.Length; i++)
            {
                args[i] = str[i] + str2[i];
            }

            foreach(string singleArg in args)
            {
                StatemenGeneratorSqlProfiler.AddParamValueEntry(paramsAndValues, singleArg);
            }
            return paramsAndValues;
        }

        private static void AddParamValueEntry(
          Dictionary<string, string> paramsAndValues,
          string singleArg)
        {
            Match match = StatemenGeneratorSqlProfiler.varNameReg.Match(singleArg);
            string key = match.Groups["varName"].Value.Trim();
            string type = match.Groups["varType"].Value.Trim();
            string str = match.Groups["varValue"].Value.Trim();
            if (!(key != "") || !(type != "") || !(str != "") || paramsAndValues.ContainsKey(key))
                return;
            paramsAndValues.Add(key, StatemenGeneratorSqlProfiler.FormatArgument(type, str));
        }
    }
}
