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
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using TestFx.Evaluation;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.SpecK.Implementation.Utilities
{
  /// <summary>
  ///   Factory that creates an instance of the subject based on the fields marked with <see cref="InjectedAttribute" />
  ///   available in the <see cref="ISuite{TSubject}" /> instance.
  /// </summary>
  public interface ISubjectFactory
  {
    T CreateFor<T> (ISuite<T> suiteInstance);
  }

  [UsedImplicitly]
  internal class SubjectFactory : ISubjectFactory
  {
    public T CreateFor<T> (ISuite<T> suiteInstance)
    {
      var suiteType = suiteInstance.GetType();
      var subjectType = typeof (T);

      var constructors = subjectType.GetConstructors(MemberBindings.Instance);
      if (constructors.Length != 1)
        throw new EvaluationException(string.Format("Missing default constructor for subject type '{0}'.", subjectType.Name));

      var constructor = constructors.Single();
      var constructorParameters = constructor.GetParameters();

      var suiteFields = suiteType.GetFieldsWithAttribute<InjectedAttribute>().Select(x => x.Item1).ToList();
      var arguments = constructorParameters.Select(x => GetArgumentValue(x, suiteInstance, suiteFields)).ToArray();
      if (arguments.Any(x => x == null))
      {
        var missingParameters = constructorParameters.Select((x, i) => Tuple.Create(x, arguments[i]))
            .Where(x => x.Item2 == null)
            .Select(x => x.Item1.NotNull().Name);

        throw new EvaluationException(
            string.Format(
                "Missing constructor arguments for subject type '{0}': {1}",
                subjectType.Name,
                string.Join(", ", missingParameters.ToArray())));
      }

      return (T) constructor.Invoke(arguments);
    }

    [CanBeNull]
    private object GetArgumentValue (ParameterInfo parameter, object suiteInstance, IList<FieldInfo> suiteFields)
    {
      var argumentField = suiteFields.SingleOrDefault(x => x.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
      if (argumentField == null)
        return null;

      return argumentField.IsStatic ? argumentField.GetValue(null) : suiteInstance.GetMemberValue<object>(argumentField.Name);
    }
  }
}