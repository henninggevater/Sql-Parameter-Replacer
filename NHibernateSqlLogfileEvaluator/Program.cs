// Decompiled with JetBrains decompiler
// Type: NHibernateSqlLogfileEvaluator.App.Program
// Assembly: NHibernateSqlLogfileEvaluator.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AF64C7C9-B58A-48DF-861C-15C0CC87C274
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\NHibernateSqlLogfileEvaluator.App.exe

using System;
using System.Windows.Forms;

namespace NHibernateSqlLogfileEvaluator.App
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainForm());
    }
  }
}
