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
using System.IO;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Containers;
using TestFx.SpecK.Implementation;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK
{
  #region ISuite<TSubject>

  public interface ISuite<TSubject>
  {
    void SetupOnce (Action setup, Action cleanup = null);
    void Setup (Action<ITestContext<TSubject>> setup, Action<ITestContext<TSubject>> cleanup = null);

    IIgnoreOrCase<TSubject, Dummy> Specify (Action<TSubject> action);
    IIgnoreOrCase<TSubject, TResult> Specify<TResult> (Func<TSubject, TResult> action);

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
  [TypeLoaderType(typeof(TypeLoader))]
  [OperationOrdering(
      typeof(SetupExtension),
      typeof(SetupCommon),
      typeof(SetupSubject),
      typeof(Arrange),
      typeof(BeforeAct),
      typeof(Act),
      typeof(AfterAct),
      typeof(Assert),
      typeof(CleanupCommon),
      typeof(CleanupExtension))]
  public class SubjectAttribute : SuiteAttributeBase
  {
    [UsedImplicitly]
    [DisplayFormat ("{0}.{1}")]
    public SubjectAttribute (Type type, string method)
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
          Func<ICombineOrArrangeOrAssert<TSubject, TResult, Dummy, Dummy>, IAssert> succession);
    }

    public interface IIgnore<TSubject, TResult>
    {
      ICase<TSubject, TResult> Skip (string reason);
    }

    public interface ICombine<TSubject, out TResult>
    {
      IArrangeOrAssert<TSubject, TResult, Dummy, TNewSequenceuence> WithSequences<TNewSequenceuence> (IDictionary<string, TNewSequenceuence> sequences);
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

  namespace Test
  {
    #region Spec class

    internal class StreamSuite : ISuite<FileStream>
    {
      public StreamSuite ()
      {
        ////Specify ((x, a) => x.EndRead (null), Adv.Combine()
        Specify (x => x.EndRead (null))
            .Case ("bla", _ => _
                .GivenVars(x => new { A = "bla", B = 2 })
                .GivenSubject ("file stream", x => File.OpenRead ("bla"))
                .Given ("", x => x.Subject.Close ())
                .ItForStream ())
            .Skip ("reason")
            .Case ("case2", _ => _
                .GivenVars(x => new { A = "bla", B = 2 })
                .Given ("", x => { })
                .Given (MyContext (1, 2))
                .Given (MyContext2 (1, 2))
                .It (MyBehavior3 (1, 2)));
      }

      private Context MyContext (int a, int b)
      {
        Context myContext = s => s.Given("muh", x => { });
        return myContext;
      }

      private Context<FileStream> MyContext2 (int a, int b)
      {
        return s => s.Given("muh", x => { });
      }

      private Behavior<FileStream, int> MyBehavior3 (int a, int b)
      {
        return s => s.It("muh", x => { });
      }

      #region NotImplemented

      public void SetupOnce (Action setup, Action cleanup = null)
      {
        throw new NotImplementedException();
      }

      public void Setup (Action<ITestContext<FileStream>> setup, Action<ITestContext<FileStream>> cleanup = null)
      {
        throw new NotImplementedException();
      }

      public IIgnoreOrCase<FileStream, Dummy> Specify (Action<FileStream> action)
      {
        throw new NotImplementedException();
      }

      public IIgnoreOrCase<FileStream, TResult> Specify<TResult> (Func<FileStream, TResult> action)
      {
        throw new NotImplementedException();
      }

      public FileStream CreateSubject ()
      {
        throw new NotImplementedException();
      }

      #endregion
    }

    #endregion

    #region Fluent extension

    internal static class Extensions
    {
      public static IAssert<TSubject, TResult, TVars, TSequence> ItForStream<TSubject, TResult, TVars, TSequence> (
          this IAssert<TSubject, TResult, TVars, TSequence> assert)
          where TSubject : Stream
      {
        throw new NotImplementedException();
      }
    }

    #endregion
  }
}