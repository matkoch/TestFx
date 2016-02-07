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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Resolve.Managed;
using JetBrains.ReSharper.Psi.Tree;
// ReSharper disable All

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract partial class InvocationExpressionBase : TreeNodeBase, IInvocationExpression
  {
    private readonly IInvocationExpression _invocationExpression;

    protected InvocationExpressionBase (IInvocationExpression invocationExpression)
        : base(invocationExpression)
    {
      _invocationExpression = invocationExpression;
    }

    public string Dump ()
    {
      return _invocationExpression.Dump();
    }

    TreeNodeCollection<ICSharpArgument> ICSharpArgumentsOwner.Arguments
    {
      get { return _invocationExpression.Arguments; }
    }

    public TreeNodeEnumerable<ICSharpArgument> ArgumentsEnumerable
    {
      get { return _invocationExpression.ArgumentsEnumerable; }
    }

    public ICSharpArgument AddArgumentBefore (ICSharpArgument param, ICSharpArgument anchor)
    {
      return _invocationExpression.AddArgumentBefore(param, anchor);
    }

    public ICSharpArgument AddArgumentAfter (ICSharpArgument param, ICSharpArgument anchor)
    {
      return _invocationExpression.AddArgumentAfter(param, anchor);
    }

    public void RemoveArgument (ICSharpArgument param)
    {
      _invocationExpression.RemoveArgument(param);
    }

    public IList<ITokenNode> Delimeters
    {
      get { return _invocationExpression.Delimeters; }
    }

    public ITokenNode LBound
    {
      get { return _invocationExpression.LBound; }
    }

    public ITokenNode RBound
    {
      get { return _invocationExpression.RBound; }
    }

    IList<ICSharpArgumentInfo> ICSharpInvocationInfo.Arguments
    {
      get { return ((ICSharpInvocationInfo) _invocationExpression).Arguments; }
    }

    public ICSharpArgumentInfo ExtensionQualifier
    {
      get { return _invocationExpression.ExtensionQualifier; }
    }

    public ICSharpInvocationReference Reference
    {
      get { return _invocationExpression.Reference; }
    }

    public IList<IType> TypeArguments
    {
      get { return _invocationExpression.TypeArguments; }
    }

    IList<IArgument> IArgumentsOwner.Arguments
    {
      get { return ((IArgumentsOwner) _invocationExpression).Arguments; }
    }

    IArgumentInfo IInvocationInfo.ExtensionQualifier
    {
      get { return _invocationExpression.ExtensionQualifier; }
    }

    IManagedReference IInvocationInfo.Reference
    {
      get { return _invocationExpression.Reference; }
    }

    public IPsiModule PsiModule
    {
      get { return _invocationExpression.PsiModule; }
    }

    IList<IArgumentInfo> IInvocationInfo.Arguments
    {
      get { return ((IInvocationInfo) _invocationExpression).Arguments; }
    }

    public ConstantValue ConstantValue
    {
      get { return _invocationExpression.ConstantValue; }
    }

    public ExpressionAccessType GetAccessType ()
    {
      return _invocationExpression.GetAccessType();
    }

    public bool IsConstantValue ()
    {
      return _invocationExpression.IsConstantValue();
    }

    public IType Type ()
    {
      return _invocationExpression.Type();
    }

    public IExpressionType GetExpressionType ()
    {
      return _invocationExpression.GetExpressionType();
    }

    public IType GetImplicitlyConvertedTo ()
    {
      return _invocationExpression.GetImplicitlyConvertedTo();
    }

    public IType Type (IResolveContext resolveContext)
    {
      return _invocationExpression.Type(resolveContext);
    }

    public IType GetImplicitlyConvertedTo (IResolveContext resolveContext)
    {
      return _invocationExpression.GetImplicitlyConvertedTo(resolveContext);
    }

    public bool IsConstantValue (IResolveContext resolveContext)
    {
      return _invocationExpression.IsConstantValue(resolveContext);
    }

    public ICSharpExpression GetContainingExpression ()
    {
      return _invocationExpression.GetContainingExpression();
    }

    public ICSharpStatement GetContainingStatement ()
    {
      return _invocationExpression.GetContainingStatement();
    }

    public TExpression ReplaceBy<TExpression> (TExpression expr) where TExpression : class, ICSharpExpression
    {
      return _invocationExpression.ReplaceBy(expr);
    }

    public ExpressionAccessType GetAccessType (IResolveContext resolveContext)
    {
      return _invocationExpression.GetAccessType(resolveContext);
    }

    public bool IsClassifiedAsVariable
    {
      get { return _invocationExpression.IsClassifiedAsVariable; }
    }

    public bool IsLValue
    {
      get { return _invocationExpression.IsLValue; }
    }

    public IArgumentList SetArgumentList (IArgumentList param)
    {
      return _invocationExpression.SetArgumentList(param);
    }

    public IPrimaryExpression SetInvokedExpression (IPrimaryExpression param)
    {
      return _invocationExpression.SetInvokedExpression(param);
    }

    public IInvocationExpressionReference InvocationExpressionReference
    {
      get { return _invocationExpression.InvocationExpressionReference; }
    }

    public IArgumentList ArgumentList
    {
      get { return _invocationExpression.ArgumentList; }
    }

    public IPrimaryExpression InvokedExpression
    {
      get { return _invocationExpression.InvokedExpression; }
    }

    public ITokenNode LPar
    {
      get { return _invocationExpression.LPar; }
    }

    public ITokenNode RPar
    {
      get { return _invocationExpression.RPar; }
    }

    public ICSharpExpression SetConditionalQualifier (ICSharpExpression expression)
    {
      return _invocationExpression.SetConditionalQualifier(expression);
    }

    public void SetConditionalAccessSign (bool value)
    {
      _invocationExpression.SetConditionalAccessSign(value);
    }

    public IExpressionType UnliftedExpressionType ()
    {
      return _invocationExpression.UnliftedExpressionType();
    }

    public bool HasConditionalAccessSign
    {
      get { return _invocationExpression.HasConditionalAccessSign; }
    }

    public ICSharpExpression ConditionalQualifier
    {
      get { return _invocationExpression.ConditionalQualifier; }
    }

    public IConditionalAccessSign ConditionalAccessSign
    {
      get { return _invocationExpression.ConditionalAccessSign; }
    }

    public bool IsNameofOperator ()
    {
      return _invocationExpression.IsNameofOperator();
    }

    public bool IsNameofOperator (IResolveContext resolveContext)
    {
      return _invocationExpression.IsNameofOperator(resolveContext);
    }
  }
}