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
using Farada.TestDataGeneration.Fluent;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Utilities.Reflection;

namespace TestFx.Farada
{
  public interface ITestDataConfigurationProvider
  {
    Func<ITestDataConfigurator, ITestDataConfigurator> Configuration { get; }
  }


  [AttributeUsage (AttributeTargets.Property)]
  public class SuiteMemberDependencyAttribute : Attribute
  {
  }

  [AttributeUsage (AttributeTargets.Field)]
  [MeansImplicitUse (ImplicitUseKindFlags.Assign)]
  public class AutoAttribute : Attribute
  {
    private ISuite _currentSuite;

    public AutoAttribute ()
    {
      MaxRecursionDepth = 3;
    }

    public int MaxRecursionDepth { get; set; }

    public ISuite CurrentSuite
    {
      get { return _currentSuite; }
      internal set { _currentSuite = value; }
    }

    [CanBeNull]
    public T GetValueFromSuiteMember<T> (string memberName)
    {
      return _currentSuite.GetMemberValue<T>(memberName);
    }

    public T GetNonNullValueFromSuiteMember<T> (string memberName)
    {
      var value = GetValueFromSuiteMember<T>(memberName);
      if (value == null)
        throw new ArgumentException(
            string.Format(
                "Member '{0}' was null. If necessary adjust the descending initialization order using the AutoAttributeOrder property.",
                memberName));

      return value;
    }

    public virtual void Mutate (object auto)
    {
    }
  }

  [AttributeUsage (AttributeTargets.Class)]
  [UsedImplicitly]
  public sealed class TestDataConfigurationAttribute : Attribute
  {
    private readonly Type _configurationType;

    public TestDataConfigurationAttribute (Type configurationType)
    {
      _configurationType = configurationType;
    }

    public Type ConfigurationType
    {
      get { return _configurationType; }
    }
  }
}