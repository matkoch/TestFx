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
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Utilities.Reflection;

namespace TestFx
{
  public class ResetInstanceFieldsTestExtensions : ITestExtension
  {
    public int Priority
    {
      get { return int.MaxValue; }
    }

    public void Extend (ITestController testController, object suite)
    {
      var fields = suite.GetType().GetFields(MemberBindings.Instance).ToList();
      if (fields.Count == 0)
        return;

      testController.AddAction<SetupExtension>("<Reset_Instance_Fields>", x => fields.ForEach(f => f.SetValue(suite, f.FieldType.GetDefaultValue())));
    }
  }
}