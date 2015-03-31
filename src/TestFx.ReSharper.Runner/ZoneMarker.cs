using System;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.UnitTestFramework;

namespace TestFx.ReSharper
{
  [ZoneMarker]
  public class ZoneMarker : IRequire<IUnitTestingZone>
  {
  }
}