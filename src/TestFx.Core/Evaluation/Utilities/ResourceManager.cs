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
using System.Threading;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Evaluation.Utilities
{
  public interface IResourceManager
  {
    IDisposable Acquire (IEnumerable<string> resources);
  }

  internal class ResourceManager : MarshalByRefObject, IResourceManager
  {
    private const int c_retryTimeout = 500;

    private static readonly IDisposable s_dummyDisposable = new CrossAppDomainDisposable(() => { });

    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(initialState: false);
    private readonly List<string> _resourcesAllocated = new List<string>();
    private readonly object _lockObject = new object();

    public IDisposable Acquire (IEnumerable<string> resources)
    {
      var resourcesList = resources.ToList();
      if (resourcesList.Count == 0)
        return s_dummyDisposable;

      IDisposable resourceAllocation;
      while (!TryAllocateResources(resourcesList, out resourceAllocation))
        _autoResetEvent.WaitOne(c_retryTimeout);

      return resourceAllocation.NotNull();
    }

    private bool TryAllocateResources (ICollection<string> resourcesList, [CanBeNull] out IDisposable resourceAllocation)
    {
      resourceAllocation = null;

      lock (_lockObject)
      {
        if (resourcesList.Any(x => _resourcesAllocated.Contains(x)))
          return false;

        _resourcesAllocated.AddRange(resourcesList);
      }

      resourceAllocation = new CrossAppDomainDisposable(() => ReleaseResources(resourcesList));
      return true;
    }

    private void ReleaseResources (ICollection<string> resourcesList)
    {
      lock (_lockObject)
        _resourcesAllocated.RemoveAll(resourcesList.Contains);

      _autoResetEvent.Set();
    }
  }
}