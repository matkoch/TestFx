﻿// Copyright 2014, 2013 Matthias Koch
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
using JetBrains.ProjectModel;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting
{
  public partial class UnitTestProviderEx
  {
    public IUnitTestElement GetDynamicElement (RemoteTask remoteTask, Dictionary<RemoteTask, IUnitTestElement> elements)
    {
      return GetDynamicElement(
          remoteTask,
          absoluteId =>
          {
            var task = new ComparableTask(absoluteId);
            IUnitTestElement element;
            return elements.TryGetValue(task, out element) ? element.To<IUnitTestElementEx>() : null;
          });
    }

    public void ExploreExternal (UnitTestElementConsumer consumer)
    {
    }

    public void ExploreSolution (ISolution solution, UnitTestElementConsumer consumer)
    {
    }
  }
}