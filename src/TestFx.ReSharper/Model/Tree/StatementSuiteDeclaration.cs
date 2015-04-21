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
using System.Diagnostics;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using TestFx.ReSharper.Model.Tree.Wrapper;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Tree
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public class StatementSuiteDeclaration : ExpressionStatementBase, ISuiteDeclaration
  {
    private readonly IIdentity _identity;
    private readonly IProject _project;
    private readonly string _text;
    private readonly IEnumerable<ISuiteDeclaration> _suiteDeclarations;
    private readonly IEnumerable<ITestDeclaration> _testDeclarations;

    public StatementSuiteDeclaration (
        IIdentity identity,
        IProject project,
        string text,
        IEnumerable<ISuiteDeclaration> suiteDeclarations,
        IEnumerable<ITestDeclaration> testDeclarations,
        IExpressionStatement statement)
        : base(statement)
    {
      _identity = identity;
      _project = project;
      _text = text;
      _suiteDeclarations = suiteDeclarations;
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

    public string Text
    {
      get { return _text; }
    }

    public IEnumerable<ISuiteDeclaration> SuiteDeclarations
    {
      get { return _suiteDeclarations; }
    }

    public IEnumerable<ITestDeclaration> TestDeclarations
    {
      get { return _testDeclarations; }
    }

    public IEnumerable<ISuiteEntity> SuiteEntities
    {
      get { return _suiteDeclarations.Cast<ISuiteEntity>(); }
    }

    public IEnumerable<ITestEntity> TestEntities
    {
      get { return _testDeclarations.Cast<ITestEntity>(); }
    }
  }
}