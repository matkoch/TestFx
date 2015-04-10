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
using System.Globalization;
using System.Reflection;

namespace TestFx.Evaluation.Utilities
{
  public interface IAppDomain : IDisposable
  {
    T CreateProxy<T> (Type proxyType, params object[] proxyArgs);
  }

  public sealed class AppDomain : IAppDomain
  {
    private const BindingFlags c_bindingFlags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance;

    private readonly System.AppDomain _appDomain;
    private readonly bool _unloadOnDispose;

    public AppDomain (System.AppDomain appDomain, bool unloadOnDispose = true)
    {
      _appDomain = appDomain;
      _unloadOnDispose = unloadOnDispose;
    }

    public T CreateProxy<T> (Type proxyType, params object[] proxyArgs)
    {
      var instance = _appDomain.CreateInstanceAndUnwrap(
          proxyType.Assembly.GetName().Name,
          proxyType.FullName,
          ignoreCase: false,
          bindingAttr: c_bindingFlags,
          binder: null,
          args: proxyArgs,
          culture: CultureInfo.InvariantCulture,
          activationAttributes: null);

      return (T) instance;
    }

    public void Dispose ()
    {
      if (_unloadOnDispose)
        System.AppDomain.Unload(_appDomain);
    }
  }
}