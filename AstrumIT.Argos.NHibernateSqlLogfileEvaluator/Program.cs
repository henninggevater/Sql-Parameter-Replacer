// Decompiled with JetBrains decompiler
// Type: AstrumIT.Argos.NHibernateSqlLogfileEvaluator.Program
// Assembly: AstrumIT.Argos.NHibernateSqlLogfileEvaluator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D97E7A68-CDEE-43E7-8E40-A752249DB11E
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\AstrumIT.Argos.NHibernateSqlLogfileEvaluator.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
  public class Program
  {
    public static void Main(string[] args)
    {
      string str = args[0];
      string pathToLogfile = args[1];
      if (str == "listIndices")
        Program.ListIndices(pathToLogfile);
      else if (str == "listDistinct")
      {
        Program.ListDistinctStatements(pathToLogfile);
      }
      else
      {
        if (!(str == "missingIndices"))
          return;
        Program.ListStatementsWithMissingIndexes(pathToLogfile);
      }
    }

    private static void ListIndices(string pathToLogfile)
    {
      List<Statement> statements = Program.ExtractStatements(pathToLogfile);
      HashSet<string> source = new HashSet<string>();
      using (ExecutionPlanGatherer executionPlanGatherer = new ExecutionPlanGatherer())
      {
        executionPlanGatherer.Open();
        foreach (Statement statement in statements)
          source.UnionWith((IEnumerable<string>) ExecutionPlanEvaluator.ListIndices(executionPlanGatherer.GetExecutionPlan(statement.Executable)));
      }
      foreach (string str in (IEnumerable<string>) source.OrderBy<string, string>((Func<string, string>) (l => l)))
        Console.WriteLine(str);
    }

    private static void ListDistinctStatements(string pathToLogfile)
    {
      foreach (Statement statement in Program.ExtractStatements(pathToLogfile).Distinct<Statement>().ToList<Statement>())
        Console.WriteLine(statement.Executable);
    }

    private static void ListStatementsWithMissingIndexes(string pathToLogfile)
    {
      List<Statement> statements = Program.ExtractStatements(pathToLogfile);
      using (ExecutionPlanGatherer executionPlanGatherer = new ExecutionPlanGatherer())
      {
        executionPlanGatherer.Open();
        foreach (Statement statement in statements)
        {
          if (ExecutionPlanEvaluator.ContainsMissingIndexes(executionPlanGatherer.GetExecutionPlan(statement.Executable)))
            Console.WriteLine(statement.Executable);
        }
      }
    }

    private static List<Statement> ExtractStatements(string pathToLogfile)
    {
      StatementParser statementParser = new StatementParser(pathToLogfile);
      List<Statement> statements = new List<Statement>();
      foreach (string statement1 in statementParser.GetStatements())
      {
        Statement statement2 = StatementGenerator.GenerateStatement(statement1);
        if (statement2 != null)
          statements.Add(statement2);
      }
      return statements;
    }
  }
}
