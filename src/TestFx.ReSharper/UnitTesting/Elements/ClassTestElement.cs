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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.Model.Tree;
using TestFx.ReSharper.Model.Tree.Aggregation;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Utilities;
using TestFx.ReSharper.Utilities.ProjectModel;
using TestFx.ReSharper.Utilities.Psi.Modules;
using TestFx.Utilities;
#if R91
using JetBrains.Metadata.Reader.Impl;
#endif

namespace TestFx.ReSharper.UnitTesting.Elements
{
  public class ClassTestElement : TestElementBase
  {
    private readonly ClrTypeName _testTypeName;

    public ClassTestElement (ITestIdentity identity, IList<Task> tasks)
        : base(identity, tasks)
    {
      _testTypeName = new ClrTypeName(identity.Relative);
    }

    public override string ShortName
    {
      get { return _testTypeName.ShortName; }
    }

    [CanBeNull]
    public override IEnumerable<IProjectFile> GetProjectFiles ()
    {
      var declaredElement = GetDeclaredElement();
      if (declaredElement == null)
        return null;

      return declaredElement.GetSourceFiles().Select(x => x.ToProjectFile());
    }

    public override UnitTestElementNamespace GetNamespace()
    {
      return UnitTestElementNamespaceFactory.Create(_testTypeName.NamespaceNames);
    }

    [CanBeNull]
    public override IDeclaredElement GetDeclaredElement ()
    {
      var project = GetProject();
      if (project == null)
        return null;

      var psiModule = project.GetPrimaryPsiModule();
      return psiModule.GetTypeElement(_testTypeName);
    }

    internal override IEnumerable<ITestFile> GetTestFiles ()
    {
      var declaredElement = GetDeclaredElement();
      var project = GetProject();
      if (declaredElement == null || project == null)
        return Enumerable.Empty<ITestFile>();

      return declaredElement.GetDeclarations()
          .Select(x => x.GetSourceFile().AssertNotNull())
          .SelectMany(x => x.GetPsiFiles<CSharpLanguage>())
          .Select(x => x.ToTestFile());
    }
  }
}