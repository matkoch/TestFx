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
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy.Core;
using FluentAssertions;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Async
{
  internal class AsyncTest : TestBase<AsyncTest.DomainSpec>
  {
    [Subject (typeof (AsyncTest))]
    internal class DomainSpec : Spec
    {
      public DomainSpec ()
      {
        SpecifyAsync (async x => await Task.Delay (20))
            .Case ("Action", _ => _
                .It ("waits", x => x.Duration.Milliseconds.Should ().BeGreaterOrEqualTo (20)));

        SpecifyAsync (async x => await Task.Delay (20).ContinueWith (task => 10))
            .Case ("Func", _ => _
                .ItReturns (x => 10));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.HasPassed ();
    }
  }
}