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
using System.Collections.Generic;
using System.Diagnostics;
using TestFx.Utilities;

namespace TestFx.Evaluation.Intents
{
  public interface IIntent : IIdentifiable
  {
    IEnumerable<IIntent> Intents { get; }

    void AddIntent (IIntent intent);
  }

  [Serializable]
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public class Intent : IIntent
  {
    public static IIntent Create (IIdentity identity)
    {
      return new Intent(identity);
    }

    private readonly IIdentity _identity;
    private readonly List<IIntent> _intents;

    private Intent (IIdentity identity)
    {
      _identity = identity;
      _intents = new List<IIntent>();
    }

    public IIdentity Identity
    {
      get { return _identity; }
    }

    public IEnumerable<IIntent> Intents
    {
      get { return _intents; }
    }

    public void AddIntent (IIntent intent)
    {
      Trace.Assert(Identity.Equals(intent.Identity.Parent));
      _intents.Add(intent);
    }
  }
}