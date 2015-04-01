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
using System.IO;
using System.Linq.Expressions;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Containers;
using TestFx.Specifications.Implementation;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications
{
  #region ISpecK<TSubject>

  public interface ISpecK<TSubject> : ISpecK
  {
    void SetupOnce (Action setup, Action cleanup = null);
    void Setup (Action<ITestContext<TSubject>> setup, Action<ITestContext<TSubject>> cleanup = null);

    IIgnoreOrCase<TSubject, Dummy> Specify (Expression<Action<TSubject>> action);
    IIgnoreOrCase<TSubject, TResult> Specify<TResult> (Expression<Func<TSubject, TResult>> action);

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

  public interface ITestContext<out TSubject, out TResult, out TVars> : ITestContext<TSubject>
  {
    TResult Result { get; }
    TVars Vars { get; }
  }

  #endregion

  #region Attributes

  [MeansImplicitUse (ImplicitUseTargetFlags.WithMembers)]
  [BaseTypeRequired (typeof (ISpecK<>))]
  public class SubjectAttribute : SubjectAttributeBase
  {
    // ReSharper disable UnusedParameter.Local
    [DisplayFormat ("{0}.{1}")]
    public SubjectAttribute (Type type, string method)
    {
    }
    // ReSharper restore UnusedParameter.Local
  }

  [MeansImplicitUse (ImplicitUseKindFlags.Access)]
  [AttributeUsage (AttributeTargets.Field, AllowMultiple = false)]
  public class InjectedAttribute : Attribute
  {
  }

  #endregion

  #region Context / Behavior delegates

  public delegate IArrange Context (IArrange<Dummy, Dummy, Dummy> arrange);

  public delegate IArrange Context<TSubject> (IArrange<TSubject, Dummy, Dummy> arrange);

  public delegate IAssert Behavior (IAssert<Dummy, Dummy, Dummy> assert);

  public delegate IAssert Behavior<in TResult> (IAssert<Dummy, TResult, Dummy> assert);

  public delegate IAssert Behavior<TSubject, in TResult> (IAssert<TSubject, TResult, Dummy> assert);

  #endregion

  namespace InferredApi
  {
    #region Arrangement / Assertion delegates

    // TODO: only ITestContext<TSubject>
    public delegate void Arrangement<in TSubject, in TResult, in TVars> (ITestContext<TSubject, TResult, TVars> context);

    public delegate void Assertion<in TSubject, in TResult, in TVars> (ITestContext<TSubject, TResult, TVars> context);

    #endregion

    #region IAdvanced extension interface

    public interface IAdvanced
    {
    }

    #endregion

    #region Fluent API

    public interface IIgnoreOrCase<TSubject, TResult>
        : IIgnore<TSubject, TResult>,
            ICase<TSubject, TResult>
    {
    }

    public interface IDefineOrArrangeOrAssert<TSubject, out TResult, TVars>
        : IDefine<TSubject, TResult, TVars>,
            IArrangeOrAssert<TSubject, TResult, TVars>
    {
    }

    public interface IArrangeOrAssert<TSubject, out TResult, TVars>
        : IArrange<TSubject, TResult, TVars>,
            IAssert<TSubject, TResult, TVars>
    {
    }

    //public interface ISpecify<TSubject>
    //{
    //  IIgnoreOrCase<TSubject, Dummy> Specify (Expression<Action<TSubject>> voidAction);
    //  IIgnoreOrCase<TSubject, TResult> Specify<TResult> (Expression<Func<TSubject, TResult>> resultAction);
    //}

    public interface ICase<TSubject, TResult>
    {
      [DisplayFormat ("{0}")]
      IIgnoreOrCase<TSubject, TResult> Case (
          string description,
          Func<IDefineOrArrangeOrAssert<TSubject, TResult, object>, IAssert> succession);
    }

    public interface IIgnore<TSubject, TResult>
    {
      ICase<TSubject, TResult> Skip (string reason);
    }

    public interface IDefine<TSubject, out TResult, TVars>
    {
      IArrangeOrAssert<TSubject, TResult, TNewVars> Define<TNewVars> (Func<Dummy, TNewVars> variablesProvider);
    }

    public interface IArrange<TSubject, out TResult, TVars> : IArrange
    {
      // TODO: should check whether there is a setup that requires the subject... then possibly throw exception
      IArrangeOrAssert<TSubject, TResult, TVars> GivenSubject (string description, Func<Dummy, TSubject> subjectFactory);
      IArrangeOrAssert<TSubject, TResult, TVars> Given (string description, Arrangement<TSubject, TResult, TVars> arrangement);
      IArrangeOrAssert<TSubject, TResult, TVars> Given (Context context);
      IArrangeOrAssert<TSubject, TResult, TVars> Given (Context<TSubject> context);
    }

    public interface IAssert<TSubject, out TResult, TVars> : IAssert
    {
      IAssert<TSubject, TResult, TVars> It (string description, Assertion<TSubject, TResult, TVars> assertion);
      IAssert<TSubject, TResult, TVars> It (Behavior behavior);
      IAssert<TSubject, TResult, TVars> It (Behavior<TResult> behavior);
      IAssert<TSubject, TResult, TVars> It (Behavior<TSubject, TResult> behavior);
      IAssert ItThrows<T> (Func<TVars, string> messageProvider = null, Func<TVars, Exception> innerExceptionProvider = null) where T : Exception;
      // TODO: 'T : TResult' removes 'out TResult'
      IAssert ItReturns<T> ();
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
      public override bool Equals (object obj)
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
    #region SpecK class

    internal class StreamSpecK : ISpecK<FileStream>
    {
      public StreamSpecK ()
      {
        ////Specify ((x, a) => x.EndRead (null), Adv.Combine()
        Specify (x => x.EndRead (null))
            .Case ("bla", _ => _
                .Define(x => new { A = "bla", B = 2 })
                .GivenSubject ("file stream", x => File.OpenRead ("bla"))
                .Given ("", x => x.Subject.Close ())
                .ItForStream ())
            .Skip ("reason")
            .Case ("case2", _ => _
                .Define(x => new { A = "bla", B = 2 })
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

      public IIgnoreOrCase<FileStream, Dummy> Specify (Expression<Action<FileStream>> action)
      {
        throw new NotImplementedException();
      }

      public IIgnoreOrCase<FileStream, TResult> Specify<TResult> (Expression<Func<FileStream, TResult>> action)
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
      public static IAssert<TSubject, TResult, TVars> ItForStream<TSubject, TResult, TVars> (this IAssert<TSubject, TResult, TVars> assert)
          where TSubject : Stream
      {
        throw new NotImplementedException();
      }
    }

    #endregion
  }
}