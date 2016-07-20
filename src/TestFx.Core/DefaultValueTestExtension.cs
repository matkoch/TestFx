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
using System.Linq;
using System.Reflection;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Utilities.Reflection;

namespace TestFx
{
  public class DefaultValueTestExtension : ITestExtension
  {
    public int Priority => 0;

    public void Extend (ITestController testController, object suite)
    {
      var fieldsWithAttribute = suite.GetType().GetFieldsWithAttribute<DefaultAttribute>()
          .Where(x => x.Item1.FieldType.IsInstantiatable<object>()).ToList();
      if (fieldsWithAttribute.Count == 0)
        return;

      testController.AddAction<SetupExtension>("<Create_DefaultValues>", x => fieldsWithAttribute.ForEach(t => CreateDefaultValue(suite, t.Item1)));
    }

    private void CreateDefaultValue (object suite, FieldInfo field)
    {
      var defaultValue = field.FieldType.CreateInstance<object>();
      field.SetValue(suite, defaultValue);
    }
  }
}