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
using System.Linq;
using System.Linq.Expressions;
using TestFx.Utilities.Introspection;
using TestFx.Utilities.Reflection;

namespace TestFx.Utilities.Expressions
{
  public interface IIntrospectionUtility
  {
    CommonExpressionProvider GetCommonExpressionProvider (Expression expression);
  }

  public class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonExpressionProvider GetCommonExpressionProvider (Expression expression)
    {
      return new CommonExpressionProvider(() => Visit(expression));
    }

    private CommonExpression Visit (Expression expression)
    {
      if (expression == null)
        return null;

      if (expression is LambdaExpression)
        return VisitLambda(expression.To<LambdaExpression>());
      if (expression is ConstantExpression)
        return VisitConstant(expression.To<ConstantExpression>());
      if (expression is MethodCallExpression)
        return VisitMethodCall(expression.To<MethodCallExpression>());
      if (expression is MemberExpression)
        return VisitMember(expression.To<MemberExpression>());
      if (expression is ParameterExpression)
        return VisitParameter(expression.To<ParameterExpression>());

      throw new Exception(string.Format("Expressions of type {0} are not supported.", expression.GetType()));
    }

    private CommonExpression VisitParameter (ParameterExpression expression)
    {
      return new CommonParameterExpression(expression.Type.ToCommon(), expression.Name);
    }

    private CommonExpression VisitMember (MemberExpression expression)
    {
      return new CommonMemberAccessExpression(Visit(expression.Expression), expression.Member.ToCommon(), new CommonExpression[0]);
    }

    private CommonExpression VisitMethodCall (MethodCallExpression expression)
    {
      var instance = Visit(expression.Object);
      var arguments = expression.Arguments.Select(Visit);

      var propertyInfo = expression.Method.GetPropertyInfo();
      if (propertyInfo != null)
        return new CommonMemberAccessExpression(instance, propertyInfo.ToCommon(), arguments);

      var eventInfo = expression.Method.GetEventInfo();
      Trace.Assert(eventInfo == null, "Expressions accessing events are not supported.");

      return new CommonInvocationExpression(instance, expression.Method.ToCommon(), arguments);
    }

    private CommonExpression VisitConstant (ConstantExpression expression)
    {
      var value = expression.Type != typeof (Type) ? expression.Value : expression.Value.To<Type>().ToCommon();

      return new CommonConstantExpression(expression.Type.ToCommon(), value);
    }

    private CommonExpression VisitLambda (LambdaExpression expression)
    {
      return Visit(expression.Body);
    }
  }
}