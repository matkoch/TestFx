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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Containers;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK
{
  #region ISuite<TSubject>

  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public interface ISuite<TSubject>
  {
    void SetupOnce (Action setup, Action cleanup = null);
    void Setup (Action<ITestContext<TSubject>> setup, Action<ITestContext<TSubject>> cleanup = null);

    IIgnoreOrCase<TSubject, Dummy> Specify (Action<TSubject> action);
    IIgnoreOrCase<TSubject, TResult> Specify<TResult> (Func<TSubject, TResult> action);

    IIgnoreOrCase<TSubject, IReadOnlyList<TItem>> Specify<TItem> (Func<TSubject, IEnumerable<TItem>> action);

    IIgnoreOrCase<TSubject, Dummy> SpecifyAsync (Func<TSubject, Task> action);
    IIgnoreOrCase<TSubject, TResult> SpecifyAsync<TResult> (Func<TSubject, Task<TResult>> action);

    TSubject CreateSubject ();
  }

  #endregion

  #region ITestContext<TSubject, TResult> / ITestContext<TSubject> interfaces

  public interface ITestContext : Extensibility.Contexts.ITestContext
  {
    Exception Exception { get; }
    TimeSpan Duration { get; }
  }

  public interface ITestContext<out TSubject> : ITestContext
  {
    TSubject Subject { get; }
  }

  public interface ITestContext<out TSubject, out TResult, out TVars, out TSequence> : ITestContext<TSubject>
  {
    TResult Result { get; }
    TVars Vars { get; }
    TSequence Sequence { get; }
  }

  #endregion

  #region Attributes

  [MeansImplicitUse]
  [BaseTypeRequired (typeof (ISuite<>))]
  public class SubjectAttribute : SuiteAttributeBase
  {
    [UsedImplicitly]
    [DisplayFormat ("{0}")]
    public SubjectAttribute (Type type)
    {
    }

    [UsedImplicitly]
    [DisplayFormat ("{0}.{1}")]
    public SubjectAttribute (Type type, string member)
    {
    }
  }

  [MeansImplicitUse (ImplicitUseKindFlags.Access)]
  [AttributeUsage (AttributeTargets.Field)]
  public class InjectedAttribute : Attribute
  {
  }

  #endregion

  #region Context / Behavior delegates

  public delegate IArrange Context (IArrange<Dummy, Dummy, Dummy, Dummy> arrange);

  public delegate IArrange Context<TSubject> (IArrange<TSubject, Dummy, Dummy, Dummy> arrange);

  public delegate IAssert Behavior (IAssert<Dummy, Dummy, Dummy, Dummy> assert);

  public delegate IAssert Behavior<in TResult> (IAssert<Dummy, TResult, Dummy, Dummy> assert);

  public delegate IAssert Behavior<TSubject, in TResult> (IAssert<TSubject, TResult, Dummy, Dummy> assert);

  #endregion

  namespace InferredApi
  {

    #region Arrangement / Assertion delegates

    // TODO: only ITestContext<TSubject>
    public delegate void Arrangement<in TSubject, in TResult, in TVars, in TSequence> (ITestContext<TSubject, TResult, TVars, TSequence> context);

    public delegate void Assertion<in TSubject, in TResult, in TVars, in TSequence> (ITestContext<TSubject, TResult, TVars, TSequence> context);

    #endregion

    #region Fluent API

    public interface IIgnoreOrCase<TSubject, TResult>
        : IIgnore<TSubject, TResult>,
            ICase<TSubject, TResult>
    {
    }

    public interface ICombineOrArrangeOrAssert<TSubject, out TResult, TVars, TSequence>
        : ICombine<TSubject, TResult>,
            IArrangeOrAssert<TSubject, TResult, TVars, TSequence>
    {
    }

    public interface IArrangeOrAssert<TSubject, out TResult, TVars, TSequence>
        : IArrange<TSubject, TResult, TVars, TSequence>,
            IAssert<TSubject, TResult, TVars, TSequence>
    {
    }

    public interface ICase<TSubject, TResult>
    {
      [DisplayFormat ("{0}")]
      IIgnoreOrCase<TSubject, TResult> Case (
          string description,
          Func<ICombineOrArrangeOrAssert<TSubject, TResult, Dummy, Dummy>, IAssert> succession,
          [CallerFilePath] string filePath = null,
          [CallerLineNumber] int lineNumber = -1);
    }

    public interface IIgnore<TSubject, TResult>
    {
      ICase<TSubject, TResult> Skip (string reason);
    }

    public interface ICombine<TSubject, out TResult>
    {
      IArrangeOrAssert<TSubject, TResult, Dummy, TNewSequenceuence> WithSequences<TNewSequenceuence> (
          IDictionary<string, TNewSequenceuence> sequences);
    }

    public interface IArrange<TSubject, out TResult, TVars, TSequence> : IArrange
    {
      IArrangeOrAssert<TSubject, TResult, TNewVars, TSequence> GivenVars<TNewVars> (Func<Dummy, TNewVars> variablesProvider);

      // TODO: should check whether there is a setup that requires the subject... then possibly throw exception
      IArrangeOrAssert<TSubject, TResult, TVars, TSequence> GivenSubject (string description, Func<Dummy, TSubject> subjectFactory);

      IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given (string description, Arrangement<TSubject, TResult, TVars, TSequence> arrangement);
      IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given (Context context);
      IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given (Context<TSubject> context);
    }

    public interface IAssert<TSubject, out TResult, out TVars, out TSequence> : IAssert
    {
      IAssert<TSubject, TResult, TVars, TSequence> It (string description, Assertion<TSubject, TResult, TVars, TSequence> assertion);
      IAssert<TSubject, TResult, TVars, TSequence> It (Behavior behavior);
      IAssert<TSubject, TResult, TVars, TSequence> It (Behavior<TResult> behavior);
      IAssert<TSubject, TResult, TVars, TSequence> It (Behavior<TSubject, TResult> behavior);
    }

    public interface IAssert : IContainer
    {
    }

    public interface IArrange : IContainer
    {
    }

    #endregion

    #region Dummy

    public sealed class Dummy
    {
      public override bool Equals ([CanBeNull] object obj)
      {
        throw new NotSupportedException();
      }

      public override int GetHashCode ()
      {
        throw new NotSupportedException();
      }

      public override string ToString ()
      {
        throw new NotSupportedException();
      }
    }

    #endregion
  }
}