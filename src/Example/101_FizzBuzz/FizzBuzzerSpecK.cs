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
using TestFx.Specifications;

namespace Example._101_FizzBuzz
{
  /// <summary>
  /// 
  /// This example shows how the SpecK dialect differs from
  /// the conservative Arrange-Act-Assert testing.
  /// 
  /// </summary>
  /// <remarks>
  /// 
  /// In SpecK the conservative Arrange-Act-Assert testing
  /// is changed to Act-[Arrange-Assert]*, which can be read as
  /// "single Act, followed by zero to n Arrange-Assert cases".
  /// 
  /// This means that you first state the action that is going
  /// to be tested. Afterwards you can add multiple test cases,
  /// each having an individual description and its own pre-
  /// and postconditions. For convenience, DefaultCase is
  /// synonymous for "&lt;Default&gt;".
  /// 
  /// Input values for the Act are defined as instance fields,
  /// and can be assigned or accessed later on.
  /// 
  /// </remarks>
  [Subject (typeof (FizzBuzzer), "Calculate")]
  public class FizzBuzzerSpecK : SpecK
  {
    int Number;

    FizzBuzzerSpecK ()
    {
      Specify (x => FizzBuzzer.Calculate (Number))
          .DefaultCase (_ => _
              .Given (x => Number = 1)
              .ItReturns (x => Number.ToString ()))
          .Case ("Dividable by 3", _ => _
              .Given (x => Number = 3)
              .ItReturns (x => "Fizz"))
          .Case ("Dividable by 5", _ => _
              .Given (x => Number = 5)
              .ItReturns (x => "Buzz"))
          .Case ("Dividable by 3 and 5", _ => _
              .Given (x => Number = 15)
              .ItReturns (x => "FizzBuzz"));
    }
  }
}