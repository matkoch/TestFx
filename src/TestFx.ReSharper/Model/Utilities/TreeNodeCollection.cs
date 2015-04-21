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
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Tree;

namespace TestFx.ReSharper.Model.Utilities
{
  public static class TreeNodeCollection
  {
    public static IEnumerable<TDestination> Create<TSource, TDestination> (
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> converter,
        Func<bool> notInterrupted)
        where TSource : ITreeNode
        where TDestination : class, ITreeNode
    {
      return new TreeNodeCollection<TSource, TDestination>(sources, converter, notInterrupted);
    }
  }

  public class TreeNodeCollection<TSource, TDestination> : BufferingNodeCollection<TSource, TDestination>
      where TSource : ITreeNode
      where TDestination : class, ITreeNode
  {
    public TreeNodeCollection (IEnumerable<TSource> sources, Func<TSource, TDestination> convert, Func<bool> notInterrupted)
        : base(sources, convert, notInterrupted)
    {
    }

    public override IEnumerator<TDestination> GetEnumerator ()
    {
      try
      {
        return base.GetEnumerator();
      }
      catch (Exception)
      {
        // TODO: log exception
        return Enumerable.Empty<TDestination>().GetEnumerator();
      }
    }

    protected override bool IsInvalid (TDestination item)
    {
      return !item.IsValid();
    }
  }
}