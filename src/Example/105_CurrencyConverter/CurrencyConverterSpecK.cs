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

namespace Example._105_CurrencyConverter
{
  [Subject (typeof (CurrencyConverter), "Convert")]
  public class CurrencyConverterSpecK : SpecK<CurrencyConverter>
  {
    string OriginalCurrency;
    decimal Amount;
    string ConvertedCurrency;
    decimal ExpectedResult;

    CurrencyConverterSpecK ()
    {
      Specify (x => x.Convert (OriginalCurrency, Amount, ConvertedCurrency))
          .DefaultCase (_ => _
              .WithSequences (
                  "EUR->USD", new { OriginalCurrency = "EUR", Amount = 15m, ConvertedCurrency = "USD", ExpectedResult = 16.79m },
                  "GBP->EUR", new { OriginalCurrency = "GBP", Amount = 5m, ConvertedCurrency = "EUR", ExpectedResult = 8.32m })
              .Given (x => OriginalCurrency = x.Combi.OriginalCurrency)
              .Given (x => Amount = x.Combi.Amount)
              .Given (x => ConvertedCurrency = x.Combi.ConvertedCurrency)
              .Given (x => ExpectedResult = x.Combi.ExpectedResult)
              .It ("converts", x => x.Result.Should ().Be (ExpectedResult)));
    }
  }
}