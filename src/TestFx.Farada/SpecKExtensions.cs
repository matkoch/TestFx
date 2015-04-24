using System;
using Farada.TestDataGeneration;
using TestFx.Specifications;

namespace TestFx.Farada
{
  public static class SpecKExtensions
  {
    public static void SetupTestDomain (
        this SpecK specK,
        TestDataDomainConfiguration domainConfiguration)
    {
      specK.Setup(context => context[FaradaTestExtensions.ConfigurationKey] = domainConfiguration);
    }
  }
}
