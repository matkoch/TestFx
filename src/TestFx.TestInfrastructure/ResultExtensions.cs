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
using FluentAssertions;
using JetBrains.Annotations;
using TestFx.Evaluation.Results;

namespace TestFx.TestInfrastructure
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class ResultExtensions
  {
    public static T HasRelativeId<T> (this T result, string relativeId)
        where T : IResult
    {
      result.Identity.Relative.Should().Be(relativeId);
      return result;
    }

    public static T HasText<T> (this T result, string text)
        where T : IResult
    {
      result.Text.Should().Be(text);
      return result;
    }

    public static T HasPassed<T> (this T result)
        where T : IResult
    {
      result.State.Should().Be(State.Passed);
      return result;
    }

    public static T HasFailed<T> (this T result)
        where T : IResult
    {
      result.State.Should().Be(State.Failed);
      return result;
    }

    public static T HasBeenIgnored<T> (this T result)
        where T : IResult
    {
      result.State.Should().Be(State.Ignored);
      return result;
    }

    public static T WasInconclusive<T> (this T result)
        where T : IResult
    {
      result.State.Should().Be(State.Inconclusive);
      return result;
    }
  }
}