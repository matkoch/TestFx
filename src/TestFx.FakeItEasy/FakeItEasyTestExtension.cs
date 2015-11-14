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
using System.Linq;
using System.Reflection;
using FakeItEasy;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Utilities.Reflection;

namespace TestFx.FakeItEasy
{
  public class FakeItEasyTestExtension : ITestExtension
  {
    public int Priority
    {
      get { return 0; }
    }

    public void Extend (ITestController testController, object suite)
    {
      CreateFakes(testController, suite);

      SetupFakes(testController, suite);
    }

    private void CreateFakes (ITestController testController, object suite)
    {
      var fieldsWithAttribute = suite.GetType().GetFieldsWithAttribute<FakeBaseAttribute>().ToList();
      if (fieldsWithAttribute.Count == 0)
        return;

      testController.AddAction<SetupExtension>("<Create_Fakes>", x => fieldsWithAttribute.ForEach(t => CreateAndAssignFake(suite, t.Item2, t.Item1)));
    }

    private void SetupFakes (ITestController testController, object suite)
    {
      var fieldsWithAttribute = suite.GetType().GetFieldsWithAttribute<ReturnedFromAttribute>().ToList();
      if (fieldsWithAttribute.Count == 0)
        return;

      testController.AddAction<SetupExtension>("<Setup_Fakes>", x => fieldsWithAttribute.ForEach(t => SetupFakeReturnValue(suite, t.Item2, t.Item1)));
    }

    private void CreateAndAssignFake(object suite, FakeBaseAttribute attribute, FieldInfo field)
    {
      var fake = this.InvokeGenericMethod("CreateFake", new object[] { attribute }, new[] { field.FieldType });
      field.SetValue(suite, fake);
    }

    private void SetupFakeReturnValue(object suite, ReturnedFromAttribute attribute, FieldInfo field)
    {
      var fake = suite.GetMemberValue<object>(attribute.FakeField);
      var returnValue = field.GetValue(suite);

      this.InvokeGenericMethod("SetupFakeCall", new[] { fake, returnValue }, new[] { field.FieldType });
    }

    [UsedImplicitly]
    private object CreateFake<T> (FakeBaseAttribute attribute)
    {
      if (attribute is FakedAttribute)
        return A.Fake<T>();
      if (attribute is DummyAttribute)
        return A.Dummy<T>();

      throw new Exception(string.Format("Attribute {0} is not known for fake creation.", attribute.GetType().Name));
    }

    [UsedImplicitly]
    private void SetupFakeCall<T> (object fake, object returnValue)
    {
      A.CallTo(fake).WithReturnType<T>().Returns((T) returnValue);
    }
  }
}