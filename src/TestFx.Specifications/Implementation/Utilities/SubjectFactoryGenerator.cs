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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Specifications.Implementation.Utilities
{
  public interface ISubjectFactoryGenerator
  {
    Delegate GetFactory (Type suiteType);
  }

  public class SubjectFactoryGenerator : ISubjectFactoryGenerator
  {
    private const BindingFlags c_bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

    public Delegate GetFactory (Type suiteType)
    {
      var closedSpeckType = suiteType.GetClosedTypeFor(typeof (ISpecK<>));
      var subjectType = closedSpeckType.GetGenericArguments().Single();
      var delegateType = typeof (Func<,>).MakeGenericType(closedSpeckType, subjectType);

      var suiteParameter = Expression.Parameter(closedSpeckType, "suite");
      var constructingExpression = CreateConstructingExpression(suiteType, subjectType, suiteParameter);
      return Expression.Lambda(delegateType, constructingExpression, new[] { suiteParameter }).Compile();
    }

    private Expression CreateConstructingExpression (Type suiteType, Type subjectType, Expression suiteExpression)
    {
      var defaultSubject = Expression.Constant(null, subjectType);
      var constructors = subjectType.GetConstructors(MemberBindings.Instance);
      if (constructors.Length != 1)
        return defaultSubject;

      var constructor = constructors.Single();
      var constructorParameters = constructor.GetParameters();

      var suiteFields = suiteType.GetFieldsWithAttribute<InjectedAttribute>().Select(x => x.Item1).ToList();
      var castedSuiteExpression = Expression.Convert(suiteExpression, suiteType);
      var argumentAccessExpressions =
          constructorParameters.Select(x => CreateArgumentAccessExpression(x, castedSuiteExpression, suiteFields)).ToList();
      if (argumentAccessExpressions.Any(x => x == null))
        return defaultSubject;

      return Expression.New(constructor, argumentAccessExpressions);
    }

    private Expression CreateArgumentAccessExpression (ParameterInfo parameter, Expression suiteExpression, IList<FieldInfo> candidates)
    {
      var argumentField = candidates.SingleOrDefault(x => x.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
      if (argumentField != null)
        return Expression.Field(suiteExpression, argumentField);

      var elementType = GetElementType(parameter);
      if (elementType == null || !parameter.Name.EndsWith("s"))
        return null;

      var argumentPrefix = parameter.Name.Substring(0, parameter.Name.Length - 1);
      var collectionElements = candidates.Where(x => x.Name.StartsWith(argumentPrefix, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Name);
      var collectionElementAccessExpressions = collectionElements.Select(x => Expression.Field(suiteExpression, x));
      return Expression.NewArrayInit(elementType, collectionElementAccessExpressions.Cast<Expression>());
    }

    private Type GetElementType (ParameterInfo parameter)
    {
      var parameterType = parameter.ParameterType;
      if (parameterType.IsArray)
        return parameterType.GetElementType();

      if (parameterType.IsGenericType)
      {
        var genericTypeDefinition = parameterType.GetGenericTypeDefinition();
        if (new[] { typeof (IEnumerable<>), typeof (ICollection<>), typeof (IList<>) }.Contains(genericTypeDefinition))
          return parameterType.GetGenericArguments().Single();
      }

      return null;
    }
  }
}