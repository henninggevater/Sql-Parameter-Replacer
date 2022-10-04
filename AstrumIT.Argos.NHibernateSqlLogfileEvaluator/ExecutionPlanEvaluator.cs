// Decompiled with JetBrains decompiler
// Type: AstrumIT.Argos.NHibernateSqlLogfileEvaluator.ExecutionPlanEvaluator
// Assembly: AstrumIT.Argos.NHibernateSqlLogfileEvaluator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D97E7A68-CDEE-43E7-8E40-A752249DB11E
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\AstrumIT.Argos.NHibernateSqlLogfileEvaluator.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AstrumIT.Argos.NHibernateSqlLogfileEvaluator
{
  public class ExecutionPlanEvaluator
  {
    public static List<string> ListIndices(string xmlString) => XDocument.Parse(xmlString).Descendants().Attributes((XName) "Index").Select<XAttribute, string>((Func<XAttribute, string>) (x => x.Value)).ToList<string>();

    public static bool ContainsMissingIndexes(string xmlString) => XDocument.Parse(xmlString).Descendants().Elements<XElement>().Any<XElement>((Func<XElement, bool>) (elem => elem.Name.LocalName == "MissingIndexes"));

    public static bool ContainsClusteredIndexScan(string xmlString) => XDocument.Parse(xmlString).Descendants().Elements<XElement>().Where<XElement>((Func<XElement, bool>) (elem => elem.Name.LocalName == "RelOp")).Attributes((XName) "PhysicalOp").Any<XAttribute>((Func<XAttribute, bool>) (attr => attr.Value == "Clustered Index Scan"));
  }
}
