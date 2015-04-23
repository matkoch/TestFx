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
using System.Security.Cryptography.X509Certificates;
using TestFx.Specifications;

namespace Example
{
  public class Calculator
  {
    public int Divide (int a, int b)
    {
      if (b == 0)
        throw new ArgumentException ("Divisor must not be zero.");

      return a / b;
    }
  }

  [Subject (typeof (Calculator), "Divide")]
  public class CalculatorSpecK : SpecK<Calculator>
  {
    int Dividend;
    int Divisor;

    CalculatorSpecK ()
    {
      Specify (x => x.Divide (Dividend, Divisor))
          .DefaultCase (_ => _
              .Given ("Dividend is 10", x => Dividend = 10)
              .Given ("Divisor is 5", x => Divisor = 5)
              .ItReturns (x => 2))
          .Case ("Division by Zero", _ => _
              .It ("", x => { }));
      //.ItThrows (typeof (ArgumentException), "Divisor must not be zero."));
    }
  }
}