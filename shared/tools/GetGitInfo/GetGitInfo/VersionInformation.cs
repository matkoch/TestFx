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

namespace GetGitInfo
{
  public class VersionInformation
  {
    private readonly string _branch;
    private readonly string _commit;
    private readonly SemVer _semVer;

    public VersionInformation (string branch, string commit, SemVer semVer)
    {
      _branch = branch;
      _commit = commit;
      _semVer = semVer;
    }

    public string Branch
    {
      get { return _branch; }
    }

    public string Commit
    {
      get { return _commit; }
    }

    public SemVer SemVer
    {
      get { return _semVer; }
    }
  }
}