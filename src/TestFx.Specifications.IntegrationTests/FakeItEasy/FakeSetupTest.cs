// Copyright 2015, 2014 Matthias Koch
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
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.FakeItEasy;

namespace TestFx.Specifications.IntegrationTests.FakeItEasy
{
  public class FakeSetupTest : TestBase<FakeSetupTest.DomainSpecK>
  {
    [Subject (typeof (FakeSetupTest), "Test")]
    public class DomainSpecK : SpecK<DomainType>
    {
      [Faked] IServiceProvider ServiceProvider;
      [Dummy] [ReturnedFrom ("ServiceProvider")] object Service;

      public DomainSpecK ()
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

    [Test]
    public override void Test ()
    {
      AssertTest (Default, State.Passed)
          .WithOperations (
              Reset_Instance_Fields,
              Create_Fakes,
              Setup_Fakes,
              Create_Subject,
              Action,
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