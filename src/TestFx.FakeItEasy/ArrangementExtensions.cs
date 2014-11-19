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
using System.Linq.Expressions;
using FakeItEasy;
using FakeItEasy.Configuration;
using TestFx.Extensibility;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.InferredApi;
using TestFx.Utilities;
using TestFx.Utilities.Expressions;

namespace TestFx.FakeItEasy
{
  public static class ArrangementExtensions
  {
    public static IArrangeOrAssert<TSubject, TResult, TVars> GivenACallTo<TSubject, TResult, TVars> (
        this IArrange<TSubject, TResult, TVars> arrange,
        Expression<Action<Dummy>> callExpression,
        Expression<Action<IVoidConfiguration, TVars>> callConfigurator)
    {
      var controller = arrange.Get<ITestController<TSubject, TResult, TVars>>();
      var text = string.Format(
          "ACallTo {0}.{1}",
          callExpression.ParseExcept(new[] { typeof (ISuite) }),
          callConfigurator.ParseExcept(new[] { typeof (Dummy), typeof(IAssertConfiguration) }));

      controller.AddArrangement(
          text,
          x =>
          {
            var configuration = A.CallTo(Expression.Lambda<Action>(callExpression.Body)).WithAnyArguments();
            callConfigurator.Compile()(configuration, x.Vars);
          });
      return arrange.To<IArrangeOrAssert<TSubject, TResult, TVars>>();
    }

    public static IArrangeOrAssert<TSubject, TResult, TVars> GivenACallTo<TSubject, TResult, TVars, TCallResult> (
        this IArrange<TSubject, TResult, TVars> arrange,
        Expression<Func<Dummy, TCallResult>> callExpression,
        Expression<Action<IReturnValueConfiguration<TCallResult>, TVars>> callConfigurator)
    {
      var controller = arrange.Get<ITestController<TSubject, TResult, TVars>>();
      var text = string.Format(
          "ACallTo {0}.{1}",
          callExpression.ParseExcept(new[] { typeof (ISuite) }),
          callConfigurator.ParseExcept(new[] { typeof (TVars), typeof(IReturnValueConfiguration<TCallResult>) }));

      controller.AddArrangement(
          text,
          x =>
          {
            var configuration = A.CallTo(Expression.Lambda<Func<TCallResult>>(callExpression.Body)).WithAnyArguments();
            callConfigurator.Compile()(configuration, x.Vars);
          });
      return arrange.To<IArrangeOrAssert<TSubject, TResult, TVars>>();
    }
  }
}