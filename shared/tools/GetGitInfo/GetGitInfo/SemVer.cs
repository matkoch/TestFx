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
using System.Globalization;
using System.Text;

namespace GetGitInfo
{
  public class SemVer
  {

    public static SemVer TryParse (string value)
    {
      var values = value.Split('.');
      if (values.Length != 3)
        return null;

      int major, minor, patch;
      if (!Int32.TryParse(values[0], out major) ||
          !Int32.TryParse(values[1], out minor) ||
          !Int32.TryParse(values[2], out patch))
        return null;

      return new SemVer(major, minor, patch);
    }

    private const string c_prereleaseTagPrefix = "-Pre";

    private readonly int _major;
    private readonly int _minor;
    private readonly int _patch;
    private readonly int _commitsSince;

    public SemVer (int major, int minor, int patch)
    {
      _major = major;
      _minor = minor;
      _patch = patch;
    }

    public SemVer (SemVer lastSemVer, int commitsSince)
    {
      _major = lastSemVer.Major;
      _minor = lastSemVer.Minor;
      _patch = commitsSince > 0 ? lastSemVer.Patch + 1 : lastSemVer.Patch;
      _commitsSince = commitsSince;
    }

    public int Major
    {
      get { return _major; }
    }

    public int Minor
    {
      get { return _minor; }
    }

    public int Patch
    {
      get { return _patch; }
    }

    public string PrereleaseTag
    {
      get { return IsPrerelease ? c_prereleaseTagPrefix + _commitsSince.ToString(CultureInfo.InvariantCulture).PadLeft(5, '0') : ""; }
    }

    private bool IsPrerelease
    {
      get { return _commitsSince > 0; }
    }

    public override string ToString ()
    {
      return string.Format("{0}.{1}.{2}{3}", Major, Minor, Patch, PrereleaseTag);
    }
  }
}