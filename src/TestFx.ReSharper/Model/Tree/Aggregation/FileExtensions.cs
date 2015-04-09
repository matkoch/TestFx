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
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Tree.Aggregation
{
  public static class FileExtensions
  {
    private static readonly Key<ISuiteFile> s_suiteFileKey = new Key<ISuiteFile>("SuiteFile");

    [CanBeNull]
    public static ISuiteFile ToSuiteFile (this IFile file, Func<bool> notInterrupted = null)
    {
      var project = file.GetProject();
      if (project == null)
        return null;

      var csharpFile = file as ICSharpFile;
      if (csharpFile == null)
        return null;

      return FileAggregatorFactory.Instance.Aggregate(project, notInterrupted).GetSuiteFile(csharpFile);
    }
  }
}