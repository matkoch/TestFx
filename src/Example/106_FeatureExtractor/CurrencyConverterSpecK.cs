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

namespace Example._106_FeatureExtractor
{
  [Subject (typeof (IFeatureExtractor), "Calculate")]
  public class CurrencyConverterSpecK : SpecK
  {
    IFeatureExtractor FeatureExtractor;
    int A;
    int B;

    CurrencyConverterSpecK ()
    {
      Specify (x => FeatureExtractor.Calculate (A, B))
          .DefaultCase (_ => _
              .WithPermutations (
                  new
                  {
                      FeatureExtractor = default(IFeatureExtractor),
                      A = default(int),
                      B = default(int)
                  },
                  x => x.FeatureExtractor,
                  new IFeatureExtractor[]
                  {
                      new SimpleFeatureExtractor (),
                      new AdvancedFeatureExtractor ()
                  },
                  x => x.A,
                  new[]
                  {
                      int.MinValue,
                      int.MinValue / 2,
                      0,
                      int.MaxValue / 2,
                      int.MaxValue
                  },
                  x => x.B,
                  new[]
                  {
                      0,
                      1,
                      2,
                      3,
                      4
                  })
              .Given (x => FeatureExtractor = x.Combi.FeatureExtractor)
              .Given (x => A = x.Combi.A)
              .Given (x => B = x.Combi.B)
              .It ("normalizes", x => x.Result.Should ().BeInRange (0, 1)));
    }
  }
}