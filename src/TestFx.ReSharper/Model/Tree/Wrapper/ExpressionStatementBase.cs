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
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract class ExpressionStatementBase : TreeNodeBase, IExpressionStatement
  {
    private readonly IExpressionStatement _statement;

    protected ExpressionStatementBase (IExpressionStatement statement)
        : base(statement)
    {
      _statement = statement;
    }

    public ICSharpStatement GetContainingStatement ()
    {
      return _statement.GetContainingStatement();
    }

    public TStatement ReplaceBy<TStatement> (TStatement stmt) where TStatement : class, ICSharpStatement
    {
      return _statement.ReplaceBy(stmt);
    }

    public bool IsEmbeddedStatement
    {
      get { return _statement.IsEmbeddedStatement; }
    }

    public ICSharpExpression SetExpression (ICSharpExpression param)
    {
      return _statement.SetExpression(param);
    }

    public ICSharpExpression Expression
    {
      get { return _statement.Expression; }
    }

    public ITokenNode Semicolon
    {
      get { return _statement.Semicolon; }
    }
  }
}