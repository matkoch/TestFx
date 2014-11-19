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
using System.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using TestFx.ReSharper.Model.Tree.Wrapper;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Tree
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public class InvocationTestDeclaration : InvocationExpressionBase, ITestDeclaration
  {
    private readonly IIdentity _identity;
    private readonly IProject _project;
    private readonly string _text;

    public InvocationTestDeclaration (
        IIdentity identity,
        IProject project,
        string text,
        IInvocationExpression invocationExpression)
        : base(invocationExpression)
    {
      _identity = identity;
      _project = project;
      _text = text;
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
  }
}