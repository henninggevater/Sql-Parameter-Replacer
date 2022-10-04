// Decompiled with JetBrains decompiler
// Type: AstrumIT.Argos.NHibernateSqlLogfileEvaluator.StatementParser
// Assembly: AstrumIT.Argos.NHibernateSqlLogfileEvaluator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D97E7A68-CDEE-43E7-8E40-A752249DB11E
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\AstrumIT.Argos.NHibernateSqlLogfileEvaluator.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
  public class StatementParser
  {
    private readonly string path;

    public List<PageAndStatementTuple> PagesAndStatements { get; private set; }

    public StatementParser(string path)
    {
      this.path = path;
            Console.WriteLine(path);
      this.PagesAndStatements = new List<PageAndStatementTuple>();
      this.ParseStatements();
    }

    public List<string> GetStatements() => this.PagesAndStatements.Select<PageAndStatementTuple, string>((Func<PageAndStatementTuple, string>) (T => T.Statement)).ToList<string>();

    private void ParseStatements()
    {
      List<string> list = ((IEnumerable<string>) File.ReadAllLines(this.path)).ToList<string>();
      int num = 0;
      while (num < list.Count)
      {
        string upper = list[num].ToUpper();
        if (upper.Contains("BATCH COMMANDS:"))
        {
          num = this.ParseBatchCommands((IList<string>) list, num);
        }
        else
        {
          if (upper.Contains(" SELECT "))
            this.UpdateList(this.GetTuple(list[num]));
          ++num;
        }
      }
    }

    private PageAndStatementTuple GetTuple(string rawStatement)
    {
            Console.WriteLine(rawStatement);
      string[] strArray = rawStatement.Split('|');
      string statement = strArray[2].Trim();
      return new PageAndStatementTuple(strArray[1].Trim(), statement);
    }

    private void UpdateList(PageAndStatementTuple tuple) => this.PagesAndStatements.Add(tuple);

    private int ParseBatchCommands(IList<string> rawStatements, int iterator)
    {
      string page = rawStatements[iterator].Split('|')[1].Trim();
      for (++iterator; iterator < rawStatements.Count && rawStatements[iterator].ToUpper().Contains("COMMAND "); ++iterator)
      {
        string rawStatement = rawStatements[iterator];
        if (rawStatement.ToUpper().Contains(":SELECT"))
          this.UpdateList(new PageAndStatementTuple(page, string.Join("", ((IEnumerable<string>) rawStatement.Split(':')).Skip<string>(1)).Trim()));
      }
      return iterator;
    }
  }
}
