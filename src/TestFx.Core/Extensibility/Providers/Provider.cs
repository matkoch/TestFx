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
using System.Diagnostics;
using TestFx.Utilities;

namespace TestFx.Extensibility.Providers
{
  public interface IProvider : IIdentifiable
  {
    string Text { get; }
    bool Ignored { get; }
  }

  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public abstract class Provider : IProvider
  {
    private readonly IIdentity _identity;
    private readonly string _text;
    private readonly bool _ignored;

    protected Provider (IIdentity identity, string text, bool ignored)
    {
      _identity = identity;
      _text = text;
      _ignored = ignored;
    }

    public IIdentity Identity
    {
      get { return _identity; }
    }

    public string Text
    {
      get { return _text; }
    }

    public bool Ignored
    {
      get { return _ignored; }
    }
  }
}