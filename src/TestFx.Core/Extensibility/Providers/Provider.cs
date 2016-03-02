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
using System.Diagnostics;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Extensibility.Providers
{
  public interface IProvider : IIdentifiable
  {
    string Text { get; }

    bool Ignored { get; }

    [CanBeNull]
    string IgnoreReason { get; }
  }

  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public abstract class Provider : IProvider
  {
    protected Provider (IIdentity identity, string text, [CanBeNull] string ignoreReason)
    {
      Identity = identity;
      Text = text;
      Ignored = ignoreReason != null;
      IgnoreReason = ignoreReason;
    }

    public IIdentity Identity { get; }

    public string Text { get; }

    public bool Ignored { get; }

    [CanBeNull]
    public string IgnoreReason { get; }
  }
}