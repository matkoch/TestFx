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
using JetBrains.Annotations;
using TestFx;
using TestFx.Extensibility;
using TestFx.MSpec.Implementation;

// ReSharper disable once CheckNamespace

namespace Machine.Specifications
{

  #region SuiteAttribute

  [TypeLoaderType (typeof (TypeLoader))]
  [OperationOrdering (typeof (Operation))]
  public class SubjectAttribute : SuiteAttributeBase
  {
    [UsedImplicitly]
    [DisplayFormat ("{0}, {type}")]
    public SubjectAttribute (Type type)
    {
    }

    [UsedImplicitly]
    [DisplayFormat ("{0}")]
    public SubjectAttribute (string text)
    {
    }

    [UsedImplicitly]
    [DisplayFormat ("{0}, {1}")]
    public SubjectAttribute (Type type, string text)
    {
    }
  }

  #endregion

  #region Delegates

  public delegate void Establish ();

  public delegate void Because ();

  public delegate void It ();

  public delegate void Cleanup ();

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once UnusedTypeParameter
  public delegate void Behaves_like<T> ();

  #endregion

  #region AssemblyContext

  [UsedImplicitly]
  // ReSharper disable once InconsistentNaming
  public abstract class IAssemblyContext : IAssemblySetup
  {
    public void Setup ()
    {
      OnAssemblyStart();
    }

    public void Cleanup ()
    {
      OnAssemblyComplete();
    }

    public abstract void OnAssemblyStart ();
    public abstract void OnAssemblyComplete ();
  }

  #endregion

  #region BehaviorsAttribute

  [AttributeUsage (AttributeTargets.Class)]
  public class BehaviorsAttribute : Attribute
  {
  }

  #endregion

  #region Catch

  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public static class Catch
  {
    [CanBeNull]
    public static Exception Exception (Action throwingAction)
    {
      return Only<Exception>(throwingAction);
    }

    [CanBeNull]
    public static Exception Exception<T> (Func<T> throwingFunc)
    {
      try
      {
        throwingFunc();
      }
      catch (Exception exception)
      {
        return exception;
      }

      return null;
    }

    [CanBeNull]
    public static TException Only<TException> (Action throwingAction)
        where TException : Exception
    {
      try
      {
        throwingAction();
      }
      catch (TException exception)
      {
        return exception;
      }

      return null;
    }
  }

  #endregion
}