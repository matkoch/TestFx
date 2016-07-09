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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace TestFx.Evaluation.Utilities
{
  public static class AppDomainExtensions
  {
    private const BindingFlags c_bindingFlags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance;

    public static T CreateProxy<T> (this System.AppDomain appDomain, Type proxyType, params object[] proxyArgs)
    {
      Debug.Assert(proxyType.IsSubclassOf(typeof(MarshalByRefObject)), "proxyType.IsSubclassOf(typeof(MarshalByRefObject))");

      var instance = appDomain.CreateInstanceAndUnwrap(
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
  }
}