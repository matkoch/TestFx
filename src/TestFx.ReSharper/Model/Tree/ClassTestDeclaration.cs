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
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using TestFx.ReSharper.Model.Tree.Wrapper;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Tree
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  internal class ClassTestDeclaration : ClassDeclarationBase, ITestDeclaration
  {
    public ClassTestDeclaration (
        IIdentity identity,
        IProject project,
        IEnumerable<string> categories,
        string text,
        IEnumerable<ITestDeclaration> testDeclarations,
        IClassDeclaration classDeclaration)
        : base(classDeclaration)
    {
      Identity = identity;
      Project = project;
      Categories = categories;
      Text = text;
      TestDeclarations = testDeclarations;
    }

    public IIdentity Identity { get; }

    public IProject Project { get; }

    public IEnumerable<string> Categories { get; }

    public string Text { get; }

    public IEnumerable<ITestDeclaration> TestDeclarations { get; }

    public IEnumerable<ITestEntity> TestEntities => TestDeclarations;
  }
}