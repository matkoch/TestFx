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
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace TestFx.VisualStudio.TestAdapter
{
  [FileExtension (".dll")]
  [FileExtension (".exe")]
  [DefaultExecutorUri (ExecutorUri)]
  [ExtensionUri (ExecutorUri)]
  public class VisualStudioRunner : ITestExecutor, ITestDiscoverer
  {
    public const string ExecutorUri = "executor://TestFx/VisualStudioRunner";

    #region ITestDiscoverer

    public void DiscoverTests (
      [NotNull] IEnumerable<string> sources,
      [NotNull] IDiscoveryContext discoveryContext,
      [NotNull] IMessageLogger logger,
      [NotNull] ITestCaseDiscoverySink discoverySink)
    {
      Debugger.Launch();
    }

    #endregion

    #region ITestExecutor

    public void RunTests ([NotNull] IEnumerable<TestCase> tests, [NotNull] IRunContext runContext, [NotNull] IFrameworkHandle frameworkHandle)
    {
      Debugger.Launch();
    }

    public void RunTests ([NotNull] IEnumerable<string> sources, [NotNull] IRunContext runContext, [NotNull] IFrameworkHandle frameworkHandle)
    {
      Debugger.Launch();
    }

    public void Cancel ()
    {
      Debugger.Launch();
    }

    #endregion
  }
}