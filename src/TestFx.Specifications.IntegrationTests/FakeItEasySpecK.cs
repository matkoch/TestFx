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
using TestFx.FakeItEasy;

namespace TestFx.Specifications.IntegrationTests
{
  public abstract class FakeItEasySpecK : SpecK<FakeItEasySpecK.DomainType>
  {
    [Faked] [Injected] protected IDisposable Disposable;
    [Faked] [Injected] protected IServiceProvider ServiceProvider;
    [Dummy] [ReturnedFrom ("ServiceProvider")] protected object Service;
    [Dummy] protected object OtherService;

    public class DomainType
    {
      readonly IDisposable _disposable;
      readonly IServiceProvider _serviceProvider;

      public DomainType (IDisposable disposable, IServiceProvider serviceProvider)
      {
        _disposable = disposable;
        _serviceProvider = serviceProvider;
      }

      public object DoSomething2 ()
      {
        _disposable.Dispose ();
        return _serviceProvider.GetService (typeof (int));
      }
    }
  }
}