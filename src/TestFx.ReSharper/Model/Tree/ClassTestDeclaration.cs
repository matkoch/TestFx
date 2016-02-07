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
  public class ClassTestDeclaration : ClassDeclarationBase, ITestDeclaration
  {
    private readonly IIdentity _identity;
    private readonly IProject _project;
    private readonly IEnumerable<string> _categories;
    private readonly string _text;
    private readonly IEnumerable<ITestDeclaration> _testDeclarations;

    public ClassTestDeclaration (
        IIdentity identity,
        IProject project,
        IEnumerable<string> categories,
        string text,
        IEnumerable<ITestDeclaration> testDeclarations,
        IClassDeclaration classDeclaration)
        : base(classDeclaration)
    {
      _identity = identity;
      _project = project;
      _categories = categories;
      _text = text;
      _testDeclarations = testDeclarations;
    }

    public IIdentity Identity
    {
      get { return _identity; }
    }

    public IProject Project
    {
      get { return _project; }
    }

    public IEnumerable<string> Categories
    {
      get { return _categories; }
    }

    public string Text
    {
      get { return _text; }
    }

    public IEnumerable<ITestDeclaration> TestDeclarations
    {
      get { return _testDeclarations; }
    }

    public IEnumerable<ITestEntity> TestEntities
    {
      get { return _testDeclarations; }
    }
  }
}