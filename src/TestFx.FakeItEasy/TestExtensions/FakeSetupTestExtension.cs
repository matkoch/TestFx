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
using System.Linq;
using System.Reflection;
using FakeItEasy;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Utilities.Reflection;

namespace TestFx.FakeItEasy.TestExtensions
{
  // NOTE: currently called by FakeCreationTestExtension
  public class FakeSetupTestExtension : ITestExtension
  {
    public void Extend (ITestController testController, ISuite suite)
    {
      var fieldsWithAttribute = suite.GetType().GetFieldsWithAttribute<ReturnedFromAttribute>().ToList();
      if (fieldsWithAttribute.Count == 0)
        return;

      testController.AddAction<SetupExtension>("setup fakes", x => fieldsWithAttribute.ForEach(t => SetupFakeReturnValue(suite, t.Item2, t.Item1)));
    }

    private void SetupFakeReturnValue (ISuite suite, ReturnedFromAttribute attribute, FieldInfo field)
    {
      if (attribute.FakeField == null)
        return;

      var fake = suite.GetMemberValue<object>(attribute.FakeField);
      var returnValue = field.GetValue(suite);

      this.InvokeGenericMethod("SetupFakeCall", new[] { fake, returnValue }, new[] { field.FieldType });
    }

    private void SetupFakeCall<T> (object fake, object returnValue)
    {
      A.CallTo(fake).WithReturnType<T>().Returns((T) returnValue);
    }
  }
}