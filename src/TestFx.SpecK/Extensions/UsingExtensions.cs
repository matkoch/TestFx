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
using TestFx.Extensibility;
using TestFx.SpecK.Implementation;
using TestFx.SpecK.Implementation.Containers;
using TestFx.SpecK.Implementation.Controllers;
using TestFx.SpecK.InferredApi;
using TestFx.Utilities.Reflection;

namespace TestFx.SpecK
{
  public static class UsingExtensions
  {
    public static IArrangeOrAssert<TSubject, TResult, TVars, TSequence> GivenUsing<TSubject, TResult, TVars, TSequence> (
        this IArrange<TSubject, TResult, TVars, TSequence> arrange,
        string text,
        Action<ITestContext<TSubject, TResult, TVars, TSequence>> setup,
        Action<ITestContext<TSubject, TResult, TVars, TSequence>> cleanup)
    {
      return arrange.GivenUsing(text, x => new DelegateScope(() => setup(x), () => cleanup(x)));
    }

    public static IArrangeOrAssert<TSubject, TResult, TVars, TSequence> GivenUsing<TSubject, TResult, TVars, TSequence> (
        this IArrange<TSubject, TResult, TVars, TSequence> arrange,
        Type disposableType)
    {
      return arrange.GivenUsing(disposableType.Name, x => disposableType.CreateInstance<IDisposable>());
    }

    public static IArrangeOrAssert<TSubject, TResult, TVars, TSequence> GivenUsing<TSubject, TResult, TVars, TSequence, TDisposable> (
        this IArrange<TSubject, TResult, TVars, TSequence> arrange,
        Func<ITestContext<TSubject, TResult, TVars, TSequence>, TDisposable> scopeProvider)
        where TDisposable : IDisposable
    {
      return arrange.GivenUsing(typeof (TDisposable).Name, scopeProvider);
    }

    public static IArrangeOrAssert<TSubject, TResult, TVars, TSequence> GivenUsing<TSubject, TResult, TVars, TSequence, TDisposable> (
        this IArrange<TSubject, TResult, TVars, TSequence> arrange,
        string text,
        Func<ITestContext<TSubject, TResult, TVars, TSequence>, TDisposable> scopeProvider)
        where TDisposable : IDisposable
    {
      var controller = arrange.GetTestController();

      IDisposable scope = null;
      controller.AddSetupCleanup<Arrange, CleanupCommon>(
          "Create " + text,
          x => scope = scopeProvider((ITestContext<TSubject, TResult, TVars, TSequence>) x),
          "Dispose " + text,
          x => scope.Dispose());
      return (IArrangeOrAssert<TSubject, TResult, TVars, TSequence>) arrange;
    }

    private class DelegateScope : IDisposable
    {
      private readonly Action _dispose;

      public DelegateScope (Action create, Action dispose)
      {
        create();
        _dispose = dispose;
      }

      public void Dispose ()
      {
        _dispose();
      }
    }
  }
}