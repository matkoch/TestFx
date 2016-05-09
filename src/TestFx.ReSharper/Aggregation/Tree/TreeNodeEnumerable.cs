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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Tree;

namespace TestFx.ReSharper.Model.Tree.Aggregation
{
  public static class TreeNodeEnumerable
  {
    public static IEnumerable<T> Create<T> (Func<IEnumerable<T>> treeNodeProvider)
        where T : ITreeNode
    {
      return new TreeNodeEnumerable<T>(treeNodeProvider);
    }
  }

  internal class TreeNodeEnumerable<T> : IEnumerable<T>
      where T : ITreeNode
  {
    private readonly Func<IEnumerable<T>> _treeNodeProvider;
    private IList<T> _treeNodes;

    public TreeNodeEnumerable (Func<IEnumerable<T>> treeNodeProvider)
    {
      _treeNodeProvider = treeNodeProvider;
    }

    public IEnumerator<T> GetEnumerator ()
    {
      if (_treeNodes == null || _treeNodes.Any(x => !x.IsValid()))
        _treeNodes = _treeNodeProvider().ToList();

      return _treeNodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator();
    }
  }
}