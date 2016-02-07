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
using TestFx.SpecK.Implementation.Controllers;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK.Implementation.Containers
{
  public static class ContainerExtensions
  {
    public static ITestController<TSubject, TResult, TVars, TSequence> GetTestController<TSubject, TResult, TVars, TSequence> (
        this IArrange<TSubject, TResult, TVars, TSequence> arrange)
    {
      return arrange.Get<ITestController<TSubject, TResult, TVars, TSequence>>();
    }

    public static ITestController<TSubject, TResult, TVars, TSequence> GetTestController<TSubject, TResult, TVars, TSequence> (
        this IAssert<TSubject, TResult, TVars, TSequence> assert)
    {
      return assert.Get<ITestController<TSubject, TResult, TVars, TSequence>>();
    }
  }
}