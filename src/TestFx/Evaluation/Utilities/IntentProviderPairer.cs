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
using System.Collections.Generic;
using System.Linq;
using TestFx.Evaluation.Intents;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;

namespace TestFx.Evaluation.Utilities
{
  public interface IIntentProviderPairer
  {
    IEnumerable<Tuple<TIntent, TProvider>> Pair<TIntent, TProvider> (
        IEnumerable<TIntent> intents,
        IEnumerable<TProvider> providers,
        Func<TProvider, TIntent> intentFactory)
        where TIntent : IIntent
        where TProvider : IProvider;
  }

  public class IntentProviderPairer : IIntentProviderPairer
  {
    public IEnumerable<Tuple<TIntent, TProvider>> Pair<TIntent, TProvider> (
        IEnumerable<TIntent> intents,
        IEnumerable<TProvider> providers,
        Func<TProvider, TIntent> intentFactory)
        where TIntent : IIntent
        where TProvider : IProvider
    {
      var providersList = providers.ToList();
      var intentsList = intents.ToList();

      var pairs = intentsList.Join(
          providersList,
          x => x.Identity.Relative,
          x => x.Identity.Relative,
          (intent, provider) => new Tuple<TIntent, TProvider>(intent, provider)).ToList();

      return intentsList.Count > 0 ? pairs : providersList.Select(provider => new Tuple<TIntent, TProvider>(intentFactory(provider), provider));
    }
  }
}