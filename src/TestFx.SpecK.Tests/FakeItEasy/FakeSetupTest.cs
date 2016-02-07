// Copyright 2016, 2015, 2014 Matthias Koch
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using FakeItEasy.Core;
using FluentAssertions;
using TestFx.Evaluation.Results;
using TestFx.FakeItEasy;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.FakeItEasy
{
  public class FakeSetupTest : TestBase<FakeSetupTest.DomainSpec>
  {
    [Subject (typeof (FakeSetupTest), "Test")]
    public class DomainSpec : Spec<DomainType>
    {
      [Faked] IServiceProvider ServiceProvider;
      [Dummy] [ReturnedFrom ("ServiceProvider")] object Service;

      public DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .It ("retrieves Service from ServiceProvider", x => x.Subject.ServiceProvider.GetService (null).Should ().BeSameAs (Service)));
      }

      public override DomainType CreateSubject ()
      {
        return new DomainType (ServiceProvider);
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult ()
          .HasPassed ()
          .HasOperations (
              Constants.Reset_Instance_Fields,
              Constants.Create_Fakes,
              Constants.Setup_Fakes,
              Constants.Create_Subject,
              Constants.Action,
              "retrieves Service from ServiceProvider");
    }

    public class DomainType
    {
      public DomainType (IServiceProvider serviceProvider)
      {
        ServiceProvider = serviceProvider;
      }

      public IServiceProvider ServiceProvider { get; private set; }
    }
  }
}