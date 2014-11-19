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
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Extensibility.Containers
{
  public interface IContainer
  {
    TService Get<TService> ();
  }

  public abstract class Container : IContainer
  {
    private readonly object[] _services;

    protected Container (object service, params object[] services)
    {
      _services = service.Concat(services).ToArray();
    }

    public T Get<T> ()
    {
      var validServices = _services.OfType<T>().ToList();
      Debug.Assert(validServices.Count == 1, "Either no service, or multiple services of type " + typeof (T).Name + " where registered.");
      return validServices.Single();
    }
  }
}