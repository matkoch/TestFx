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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.Utilities;
using TestFx.Utilities.Introspection;

namespace TestFx.ReSharper.Utilities.Psi.Tree
{
  public interface IIntrospectionUtility
  {
    CommonExpressionProvider GetCommonExpressionProvider (ICSharpExpression expression);
  }

  public class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonExpressionProvider GetCommonExpressionProvider (ICSharpExpression expression)
    {
      return new CommonExpressionProvider(() => Visit(expression));
    }

    private CommonExpression Visit (ICSharpExpression expression)
    {
      if (expression is ILambdaExpression)
        return VisitLambda(expression.To<ILambdaExpression>());
      if (expression is ILiteralExpression)
        return VisitLiteral(expression.To<ILiteralExpression>());
      if (expression is IInvocationExpression)
        return VisitInvocation(expression.To<IInvocationExpression>());
      if (expression is IReferenceExpression)
        return VisitReference(expression.To<IReferenceExpression>());

      throw new Exception(string.Format("Expressions of type {0} are not supported.", expression.GetType()));
    }

    private CommonExpression VisitDeclaredElement (IDeclaredElement declaredElement)
    {
      if (declaredElement is IParameter)
        return VisitParameter(declaredElement.To<IParameter>());
      if (declaredElement is ITypeMember)
        return VisitTypeMember(declaredElement.To<ITypeMember>());

      throw new Exception(string.Format("DeclaredElements of type {0} are not supported.", declaredElement.GetType()));
    }

    private CommonExpression VisitTypeMember (ITypeMember typeMember)
    {
      var declaringType = typeMember.GetContainingType().ToCommon();
      return new CommonMemberAccessExpression(new CommonThisExpression(declaringType), typeMember.ToCommon(), new CommonExpression[0]);
    }

    private CommonExpression VisitParameter (IParameter parameter)
    {
      return new CommonParameterExpression(parameter.Type.ToCommon(), parameter.ShortName);
    }

    private CommonExpression VisitReference (IReferenceExpression expression)
    {
      if (expression.QualifierExpression != null)
        return Visit(expression.QualifierExpression);

      // TODO: try alternative implementation
      // TODO: performance critial? (try writing 'Specify(x => ....)'
      var declaredElement = expression.Reference.GetResolveResult().DeclaredElement;
      //return VisitDeclaredElement(declaredElement);
      if (declaredElement is IParameter)
      {
        var parameter = declaredElement.To<IParameter>();
        return new CommonParameterExpression(parameter.Type.ToCommon(), parameter.ShortName);
      }
      if (declaredElement is ITypeMember)
      {
        var member = declaredElement.To<ITypeMember>();
        return new CommonMemberAccessExpression(
            new CommonThisExpression(member.GetContainingType().ToCommon()),
            member.ToCommon(),
            new CommonExpression[0]);
      }

      throw new Exception("bla");
    }

    private CommonExpression VisitInvocation (IInvocationExpression expression)
    {
      var method = expression.Reference.AssertNotNull().CurrentResolveResult.AssertNotNull().DeclaredElement.To<IMethod>();
      var instance = method.IsStatic ? null : Visit(expression.InvokedExpression);
      return new CommonInvocationExpression(instance, method.ToCommon(), expression.Arguments.Select(x => Visit(x.Value)));
    }

    private CommonExpression VisitLambda (ILambdaExpression expression)
    {
      return Visit(expression.BodyExpression);
    }

    private CommonExpression VisitLiteral (ILiteralExpression expression)
    {
      var constantValue = expression.ConstantValue;
      return new CommonConstantExpression(constantValue.Type.To<IDeclaredType>().ToCommon(), constantValue.Value);
    }
  }
}