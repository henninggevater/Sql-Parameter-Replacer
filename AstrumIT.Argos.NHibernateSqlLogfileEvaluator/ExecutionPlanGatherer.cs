// Decompiled with JetBrains decompiler
// Type: AstrumIT.Argos.NHibernateSqlLogfileEvaluator.ExecutionPlanGatherer
// Assembly: AstrumIT.Argos.NHibernateSqlLogfileEvaluator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D97E7A68-CDEE-43E7-8E40-A752249DB11E
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\AstrumIT.Argos.NHibernateSqlLogfileEvaluator.exe

using log4net;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
  public class ExecutionPlanGatherer : IDisposable
  {
    private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private readonly SqlConnection connection;

    public ExecutionPlanGatherer() => this.connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServices"].ConnectionString);

    public void Open()
    {
      try
      {
        this.connection.Open();
        using (SqlCommand sqlCommand = new SqlCommand("SET ARITHABORT ON", this.connection))
          sqlCommand.ExecuteReader();
        using (SqlCommand sqlCommand = new SqlCommand("SET SHOWPLAN_XML ON", this.connection))
          sqlCommand.ExecuteReader();
      }
      catch (Exception ex)
      {
        ExecutionPlanGatherer.logger.Error((object) ex);
        throw;
      }
    }

    public string GetExecutionPlan(string statement)
    {
      using (SqlCommand sqlCommand = new SqlCommand(statement, this.connection))
      {
        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
        {
          sqlDataReader.Read();
          return sqlDataReader.GetSqlString(0).Value;
        }
      }
    }

    public void Dispose() => this.connection.Dispose();
  }
}
