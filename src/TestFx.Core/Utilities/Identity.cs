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
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace TestFx.Utilities
{
  public interface IIdentity
  {
    [CanBeNull]
    IIdentity Parent { get; }

    string Relative { get; }
    string Absolute { get; }
  }

  [DebuggerDisplay ("{Relative}")]
  [Serializable]
  public class Identity : IIdentity, IEquatable<IIdentity>
  {
    private const string c_separator = " » ";
    private static readonly Dictionary<string, IIdentity> s_identites = new Dictionary<string, IIdentity>();

    public static IIdentity Parse (string absoluteIdentity)
    {
      IIdentity identity;
      if (s_identites.TryGetValue(absoluteIdentity, out identity))
        return identity;

      var lastSeparator = absoluteIdentity.LastIndexOf(c_separator, StringComparison.InvariantCulture);
      if (lastSeparator != -1)
      {
        var parentIdentity = Parse(absoluteIdentity.Substring(0, lastSeparator));
        var relativeIdentity = absoluteIdentity.Substring(lastSeparator + c_separator.Length);
        identity = new Identity(relativeIdentity, parentIdentity);
      }
      else
      {
        identity = new Identity(absoluteIdentity);
      }

      s_identites[absoluteIdentity] = identity;
      return identity;
    }

    private readonly string _relative;
    private readonly IIdentity _parent;

    private string _absolute;

    public Identity (string relative, [CanBeNull] IIdentity parent = null)
    {
      _relative = relative;
      _parent = parent;
    }

    [CanBeNull]
    public IIdentity Parent
    {
      get { return _parent; }
    }

    public string Relative
    {
      get { return _relative; }
    }

    public string Absolute
    {
      get { return _absolute = _absolute ?? (Parent == null ? _relative : Concat(_parent.Absolute, _relative)); }
    }

    private string Concat (string first, string second)
    {
      return first + c_separator + second;
    }

    public bool Equals ([CanBeNull] IIdentity other)
    {
      if (ReferenceEquals(null, other))
        return false;
      if (ReferenceEquals(this, other))
        return true;
      return string.Equals(Absolute, other.Absolute);
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      return Equals(obj as IIdentity);
    }

    public override int GetHashCode ()
    {
      return Absolute.GetHashCode();
    }
  }
}