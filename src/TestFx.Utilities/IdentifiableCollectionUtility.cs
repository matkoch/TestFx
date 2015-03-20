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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace TestFx.Utilities
{
  public interface IIdentifiableCollectionUtility
  {
    [CanBeNull]
    T SearchNode<T> (IEnumerable<T> identifiables, IIdentity identity, Func<T, IEnumerable<T>> childrenSelector) where T : class, IIdentifiable;

    [CanBeNull]
    T SearchNode<T> (IEnumerable<T> identifiables, IIdentity identity) where T : class, IIdentifiable;
  }

  public class IdentifiableCollectionUtility : IIdentifiableCollectionUtility
  {
    public static IIdentifiableCollectionUtility Instance = new IdentifiableCollectionUtility();

    [CanBeNull]
    public T SearchNode<T> (IEnumerable<T> identifiables, IIdentity identity, Func<T, IEnumerable<T>> childrenSelector) where T : class, IIdentifiable
    {
      var itemList = identifiables.ToList();

      var commonParentIdentity = itemList.Select(x => x.Identity.Parent).Distinct().Single();
      var identityChain = identity.DescendantsAndSelf(x => x.Parent).TakeWhile(x => !x.Equals(commonParentIdentity));
      var identityStack = new Stack<IIdentity>(identityChain);

      T identifiable;
      IEnumerable<T> children = itemList;
      do
      {
        identifiable = children.Search(identityStack.Pop());
        children = childrenSelector(identifiable);
      } while (identifiable != null && identityStack.Count > 0);

      return identifiable;
    }

    [CanBeNull]
    public T SearchNode<T> (IEnumerable<T> identifiables, IIdentity identity) where T : class, IIdentifiable
    {
      Trace.Assert(identity.Parent != null, "Parent != null");

      // TODO: overwrite == operator?
      return identifiables.SingleOrDefault(x => x.Identity.Equals(identity));
    }
  }
}