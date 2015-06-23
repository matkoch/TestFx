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
using System.Linq;
using System.Management.Automation;
using LibGit2Sharp;

namespace GetGitInfo
{
  [Cmdlet (VerbsCommon.Get, "Version")]
  public class GetGitInfoCmdlet : PSCmdlet
  {
    private string _directory;

    protected override void EndProcessing ()
    {
      using (var repository = new Repository(_directory))
      {
        SemVer semVer;

        var lastSemVerTag = GetLastSemVerTag(repository);
        if (lastSemVerTag == null)
        {
          semVer = new SemVer(new SemVer(0, 0, 0), repository.Commits.Count());
        }
        else
        {
          var commitsSinceTag = repository.Head.Commits.TakeWhile(x => x != lastSemVerTag.Item1.Target).Count();
          semVer = new SemVer(lastSemVerTag.Item2, commitsSinceTag);
        }

        WriteObject(new VersionInformation(repository.Head.Name, repository.Head.Tip.Sha, semVer));
      }
    }

    [Parameter (Position = 0, Mandatory = true)]
    public string Directory
    {
      get { return _directory; }
      set { _directory = value; }
    }

    private Tuple<Tag, SemVer> GetLastSemVerTag (IRepository repository)
    {
      var relatedTags = repository.Head.Commits.SelectMany(x => repository.Tags.Where(y => y.Target == x)).ToList();
      return relatedTags.Select(x => Tuple.Create(x, SemVer.TryParse(x.Name))).FirstOrDefault(x => x.Item2 != null);
    }
  }
}