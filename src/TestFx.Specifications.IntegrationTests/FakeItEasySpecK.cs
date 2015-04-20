// Copyright 2014, 2013 Matthias Koch
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
using FakeItEasy;
using FluentAssertions;
using TestFx.Evaluation.Results;
using TestFx.FakeItEasy;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (FakeItEasySpecK), "Method")]
  public class FakeItEasySpecK : SpecK<FakeItEasySpecK.DomainType>
  {
    [Faked] [Injected] protected IDisposable Disposable;
    [Faked] [Injected] protected IServiceProvider ServiceProvider;
    [Dummy] [ReturnedFrom ("ServiceProvider")] protected object Service;
    [Dummy] protected object OtherService;

    public FakeItEasySpecK ()
    {
      Specify (x => x.DoSomething ())
          .DefaultCase (_ => _
              .It ("calls disposable", x => A.CallTo (() => Disposable.Dispose ()).MustHaveHappened ())
              .It ("resolves services", x => A.CallTo (() => ServiceProvider.GetService (typeof (IFormatProvider))).MustHaveHappened ())
              .It ("returns formatter", x => x.Result.Should ().BeSameAs (Service)))
          .Case ("adjusting fake setup", _ => _
              .Given ("service provider returns other service",
                  x => A.CallTo (() => ServiceProvider.GetService (typeof (IFormatProvider))).Returns (OtherService))
              .It ("returns other service", x => x.Result.Should ().BeSameAs (OtherService)));
    }

    public class DomainType
    {
      readonly IDisposable _disposable;
      readonly IServiceProvider _serviceProvider;

      public DomainType (IDisposable disposable, IServiceProvider serviceProvider)
      {
        _disposable = disposable;
        _serviceProvider = serviceProvider;
      }

      public object DoSomething ()
      {
        _disposable.Dispose ();
        return _serviceProvider.GetService (typeof (IFormatProvider));
      }
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using FluentAssertions;
  using NUnit.Framework;

  public class FakeItEasyTest : TestBase<FakeItEasySpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);
    }
  }
}

#endif