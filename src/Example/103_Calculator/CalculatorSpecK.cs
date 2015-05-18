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
using TestFx.Specifications;
using TestFx.Specifications.InferredApi;

namespace Example._103_Calculator
{
  /// <summary>
  /// 
  /// This example shows how the fluent API can be extended
  /// on purpose depending on subject and result type.
  /// 
  /// </summary>
  /// <remarks>
  /// 
  /// Throughout the complete Specify call, various generic
  /// types are passed to the fluent methods, which can be
  /// used to build your own test DSL using self-defined
  /// extension methods. Compared to the standard approach,
  /// this offers even more precision and readability.
  /// 
  /// </remarks>
  [Subject (typeof (Calculator), "PressEquals")]
  public class CalculatorSpecK : SpecK<Calculator>
  {
    CalculatorSpecK ()
    {
      Specify (x => x.PressEquals ())
          .DefaultCase (_ => _
              .ItDisplays (0))
          .Case ("Addition", _ => _
              .GivenEnter (5)
              .GivenPressPlus ()
              .GivenEnter (2)
              .ItDisplays (7))
          .Case ("Substraction", _ => _
              .GivenEnter (2)
              .GivenPressMinus ()
              .GivenEnter (5)
              .ItDisplays (-3))
          .Case ("Adjust arithmetic", _ => _
              .GivenEnter (10)
              .GivenPressPlus ()
              .GivenPressMinus ()
              .GivenEnter (2)
              .ItDisplays (8))
          .Case ("Addition without extension methods", _ => _
              .Given ("Enter 5", x => x.Subject.Enter (5))
              .Given ("Press plus", x => x.Subject.PressPlus ())
              .Given ("Enter 2", x => x.Subject.Enter (2))
              .It ("displays 7", x => x.Result.Should ().Be (7)));
    }
  }

  public static class CalculatorSpecKExtensions
  {
    public static IArrangeOrAssert<Calculator, int, TVars, TCombi> GivenPressPlus<TVars, TCombi> (this IArrange<Calculator, int, TVars, TCombi> arrange)
    {
      return arrange.Given ("Press plus", x => x.Subject.PressPlus ());
    }

    public static IArrangeOrAssert<Calculator, int, TVars, TCombi> GivenPressMinus<TVars, TCombi> (this IArrange<Calculator, int, TVars, TCombi> arrange)
    {
      return arrange.Given ("Press minus", x => x.Subject.PressMinus ());
    }

    public static IArrangeOrAssert<Calculator, int, TVars, TCombi> GivenEnter<TVars, TCombi> (
        this IArrange<Calculator, int, TVars, TCombi> arrange,
        ushort value)
    {
      return arrange.Given ("Enter " + value, x => x.Subject.Enter (value));
    }

    public static IAssert<Calculator, int, TVars, TCombi> ItDisplays<TVars, TCombi> (
        this IAssert<Calculator, int, TVars, TCombi> arrange,
        int value)
    {
      return arrange.It ("Displays " + value, x => x.Result.Should ().Be (value));
    }
  }
}