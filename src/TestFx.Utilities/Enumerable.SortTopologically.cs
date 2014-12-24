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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestFx.Utilities
{
  public static class TopologicalSort
  {
    public static IEnumerable<T> SortTopologically<T> (
        this IEnumerable<T> items,
        Func<T, T, bool> isDepending,
        bool throwIfOrderIsUndefined = false)
        where T : class
    {
      var vertexes = items.Select(x => new Vertex<T>(x)).ToList();
      foreach (var vertex in vertexes)
        vertex.Dependencies = vertexes.Where(x => isDepending(vertex.Value, x.Value)).ToList();
      return SortTopologically(vertexes, throwIfOrderIsUndefined).Select(x => x.Value);
    }

    private static IEnumerable<Vertex<T>> SortTopologically<T> (IEnumerable<Vertex<T>> graph, bool throwIfOrderIsUndefined = false)
    {
      var graphAsList = graph.ToList();
      var result = new List<Vertex<T>>(graphAsList.Count);

      while (graphAsList.Any())
      {
        var independents = graphAsList
            .Where(x => !result.Contains(x) && !graphAsList.Any(y => y.Dependencies.Contains(x)))
            .ToList();

        if (throwIfOrderIsUndefined && independents.Count > 1)
          throw new UndefinedOrderException(independents.Select(x => x.Value));

        var independent = independents.FirstOrDefault();
        if (independent == null)
        {
          var scc = new StronglyConnectedComponentFinder<T>();
          var enumerable = scc.DetectCycle(graphAsList).Select(x => x.Select(y => y.Value));
          throw new CircularDependencyException(enumerable.Cast<IEnumerable>());
        }

        graphAsList.Remove(independent);
        result.Add(independent);
      }

      return result;
    }

    internal class StronglyConnectedComponent<T> : IEnumerable<Vertex<T>>
    {
      private readonly LinkedList<Vertex<T>> _list;

      public StronglyConnectedComponent ()
      {
        _list = new LinkedList<Vertex<T>>();
      }

      public StronglyConnectedComponent (IEnumerable<Vertex<T>> collection)
      {
        _list = new LinkedList<Vertex<T>>(collection);
      }

      public void Add (Vertex<T> vertex)
      {
        _list.AddLast(vertex);
      }

      public IEnumerator<Vertex<T>> GetEnumerator ()
      {
        return _list.GetEnumerator();
      }

      public int Count
      {
        get { return _list.Count; }
      }

      IEnumerator IEnumerable.GetEnumerator ()
      {
        return _list.GetEnumerator();
      }

      public bool IsCycle
      {
        get { return _list.Count > 1; }
      }
    }

    /// <summary>
    /// Implementation of the Tarjan stronly connected components algorithm.
    /// </summary>
    /// <seealso cref="http://en.wikipedia.org/wiki/Tarjan's_strongly_connected_components_algorithm"/>
    /// <seealso cref="http://stackoverflow.com/questions/261573/best-algorithm-for-detecting-cycles-in-a-directed-graph"/>
    internal class StronglyConnectedComponentFinder<T>
    {
      private StronglyConnectedComponentList<T> _stronglyConnectedComponents;
      private Stack<Vertex<T>> _stack;
      private int _index;

      /// <summary>
      /// Calculates the sets of strongly connected vertices.
      /// </summary>
      /// <param name="graph">Graph to detect cycles within.</param>
      /// <returns>Set of strongly connected components (sets of vertices)</returns>
      public StronglyConnectedComponentList<T> DetectCycle (IEnumerable<Vertex<T>> graph)
      {
        _stronglyConnectedComponents = new StronglyConnectedComponentList<T>();
        _index = 0;
        _stack = new Stack<Vertex<T>>();
        foreach (var v in graph)
        {
          if (v.Index < 0)
            StrongConnect(v);
        }
        return _stronglyConnectedComponents;
      }

      private void StrongConnect (Vertex<T> v)
      {
        v.Index = _index;
        v.LowLink = _index;
        _index++;
        _stack.Push(v);

        foreach (var w1 in v.Dependencies)
        {
          if (w1.Index < 0)
          {
            StrongConnect(w1);
            v.LowLink = Math.Min(v.LowLink, w1.LowLink);
          }
          else if (_stack.Contains(w1))
            v.LowLink = Math.Min(v.LowLink, w1.Index);
        }

        if (v.LowLink != v.Index)
          return;

        var scc = new StronglyConnectedComponent<T>();
        Vertex<T> w2;
        do
        {
          w2 = _stack.Pop();
          scc.Add(w2);
        } while (w2 != v);
        _stronglyConnectedComponents.Add(scc);
      }
    }

    internal class StronglyConnectedComponentList<T> : IEnumerable<StronglyConnectedComponent<T>>
    {
      private readonly LinkedList<StronglyConnectedComponent<T>> _collection;

      public StronglyConnectedComponentList ()
      {
        _collection = new LinkedList<StronglyConnectedComponent<T>>();
      }

      public StronglyConnectedComponentList (IEnumerable<StronglyConnectedComponent<T>> collection)
      {
        _collection = new LinkedList<StronglyConnectedComponent<T>>(collection);
      }

      public void Add (StronglyConnectedComponent<T> scc)
      {
        _collection.AddLast(scc);
      }

      public int Count
      {
        get { return _collection.Count; }
      }

      public IEnumerator<StronglyConnectedComponent<T>> GetEnumerator ()
      {
        return _collection.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator ()
      {
        return _collection.GetEnumerator();
      }

      public IEnumerable<StronglyConnectedComponent<T>> IndependentComponents ()
      {
        return this.Where(c => !c.IsCycle);
      }

      public IEnumerable<StronglyConnectedComponent<T>> Cycles ()
      {
        return this.Where(c => c.IsCycle);
      }
    }

    internal class Vertex<T>
    {
      public Vertex ()
      {
        Index = -1;
        Dependencies = new List<Vertex<T>>();
      }

      public Vertex (T value)
          : this()
      {
        Value = value;
      }

      public Vertex (IEnumerable<Vertex<T>> dependencies)
      {
        Index = -1;
        Dependencies = dependencies.ToList();
      }

      public Vertex (T value, IEnumerable<Vertex<T>> dependencies)
          : this(dependencies)
      {
        Value = value;
      }

      internal int Index { get; set; }

      internal int LowLink { get; set; }

      public T Value { get; set; }

      public ICollection<Vertex<T>> Dependencies { get; set; }
    }
  }
}