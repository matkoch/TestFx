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
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Strategy;
using JetBrains.Util;
using TestFx.ReSharper.Model.Tree;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Utilities;
using TestFx.ReSharper.Utilities.Psi.Tree;
using TestFx.Utilities;
using RecursiveRemoteTaskRunner = TestFx.ReSharper.Runner.RecursiveRemoteTaskRunner;

namespace TestFx.ReSharper.UnitTesting.Elements
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public abstract partial class TestElementBase : ITestElement
  {
    private static readonly IUnitTestRunStrategy RunStrategy =
        new OutOfProcessUnitTestRunStrategy(new RemoteTaskRunnerInfo(RecursiveRemoteTaskRunner.ID, typeof (RecursiveRemoteTaskRunner)));

    private readonly ITestIdentity _identity;
    private readonly IList<Task> _tasks;
    private readonly HashSet<IUnitTestElement> _children;

    private UnitTestElementState _state;
    private TestElementBase _parent;
    private string _text;

    protected TestElementBase (ITestIdentity identity, IList<Task> tasks)
    {
      _identity = identity;
      _tasks = tasks;
      _children = new HashSet<IUnitTestElement>();
    }

    [CanBeNull]
    public abstract IEnumerable<IProjectFile> GetProjectFiles ();

    public abstract UnitTestElementNamespace GetNamespace();

    [CanBeNull]
    public abstract IDeclaredElement GetDeclaredElement ();

    internal abstract IEnumerable<ITestFile> GetTestFiles ();

    public IIdentity Identity => _identity;

    [CanBeNull]
    public IProject GetProject ()
    {
      return Id.Project;
    }

    public string Kind => "Test";

    public UnitTestElementKind ElementKind => _children.Any() ? UnitTestElementKind.TestContainer : UnitTestElementKind.Test;

    public virtual string ShortName => Identity.Relative;

    public void Update (string text, [CanBeNull] string explicitReason, IEnumerable<UnitTestElementCategory> categories)
    {
      _text = text;
      ExplicitReason = explicitReason;
      
      Categories = categories;
      _state = UnitTestElementState.None;
    }

    public IEnumerable<UnitTestElementCategory> Categories { get; private set; }

    public bool Explicit => ExplicitReason != null;

    [CanBeNull]
    public string ExplicitReason { get; private set; }

    public UnitTestElementState State
    {
      get { return _state; }
      set
      {
        _state = value;

        if (value == UnitTestElementState.Invalid)
          _children.ForEach(x => x.State = UnitTestElementState.Invalid);
      }
    }

    //private UnitTestElementState GetState (UnitTestElementState currentState, UnitTestElementState newState)
    //{
    //  if (newState == UnitTestElementState.Valid && currentState == UnitTestElementState.PendingDynamic)
    //    return UnitTestElementState.Dynamic;

    //  if ((currentState == UnitTestElementState.Dynamic || currentState == UnitTestElementState.PendingDynamic) &&
    //      (newState == UnitTestElementState.Valid || newState == UnitTestElementState.Pending))
    //  {
    //    throw new Exception($"Current state {currentState} unable to turn into {newState}");
    //  }

    //  return newState;
    //}

    [CanBeNull]
    public IUnitTestElement Parent
    {
      get { return _parent; }
      set
      {
        if (ReferenceEquals(value, _parent))
          return;

        if (_parent != null)
          _parent.Children.Remove(this);

        _parent = value as TestElementBase;

        if (_parent != null)
          _parent.Children.Add(this);
      }
    }

    public ICollection<IUnitTestElement> Children => _children;

    public IUnitTestRunStrategy GetRunStrategy ([NotNull] IHostProvider hostProvider)
    {
      return RunStrategy;
    }

    public IList<UnitTestTask> GetTaskSequence (ICollection<IUnitTestElement> explicitElements, bool init)
    {
      if (init && explicitElements.Count > 0 && !explicitElements.Contains(this))
        return new List<UnitTestTask>();

      var parentTasks = _parent != null ? _parent.GetTaskSequence(explicitElements, init: false) : new List<UnitTestTask>();
      return parentTasks.Concat(_tasks.Select(x => new UnitTestTask(this, x))).ToList();
    }

    public UnitTestElementDisposition GetDisposition ()
    {
      return GetDispositionFromFiles(GetTestFiles().ToArray());
    }

    public UnitTestElementDisposition GetDispositionFromFiles (params ITestFile[] testFiles)
    {
      var declarations = testFiles
          .Select(x => x.TestDeclarations.Search(Identity, y => y.TestDeclarations))
          .WhereNotNull().ToList();

      var locations = declarations.Select(GetUnitTestElementLocation).ToList();
      if (locations.Count != 0)
        return new UnitTestElementDisposition(locations, this);

      if (_state == UnitTestElementState.Dynamic)
        return UnitTestElementDisposition.NotYetClear(this);

      _state = UnitTestElementState.Invalid;
      return UnitTestElementDisposition.InvalidDisposition;
    }

    private UnitTestElementLocation GetUnitTestElementLocation (ITestDeclaration declaration)
    {
      var ranges = declaration.GetRanges();
      return new UnitTestElementLocation(
          declaration.GetSourceFile().ToProjectFile(),
          ranges.NavigationRange.TextRange,
          ranges.ContainingRange.TextRange);
    }
  }
}