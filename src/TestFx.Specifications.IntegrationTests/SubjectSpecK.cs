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
using System.IO;
using FluentAssertions;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (SubjectSpecK), "Method")]
  public class SubjectSpecK : SpecK<SubjectSpecK.DomainType>
  {
    BinaryWriter Writer;

    [Injected] MemoryStream Disposable = new MemoryStream ();
    [Injected] string String1 = "A";
    [Injected] string String2 = "B";
    [Injected] string String3 = "C";
    string String4 = "Uninjected";

    SubjectSpecK ()
    {
      Specify (x => x.ToString ())
          .Case ("Case 1", _ => _
              .It ("passes disposable", x => x.Subject.Disposable.Should ().BeSameAs (Disposable))
              .It ("passes strings as collection", x => x.Subject.Strings.Should ().Equal (new[] { String1, String2, String3 })))
          .Case ("Case 2", _ => _
              .Given ("reset disposable", x => Writer = new BinaryWriter (Stream.Null))
              .GivenSubject ("is created with null values", x => new DomainType (Writer, null))
              .It ("passes null disposable", x => x.Subject.Disposable.Should ().BeSameAs (Writer))
              .It ("passes null string collection", x => x.Subject.Strings.Should ().BeNull ()));
    }

    public class DomainType
    {
      public static int ConstructorCalls = 0;

      readonly IDisposable _disposable;
      readonly IEnumerable<string> _strings;

      public DomainType (IDisposable disposable, IEnumerable<string> strings)
      {
        ConstructorCalls++;

        _disposable = disposable;
        _strings = strings;
      }

      public IDisposable Disposable { get { return _disposable; } }
      public IEnumerable<string> Strings { get { return _strings; } }
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using NUnit.Framework;

  public class SubjectTest : TestBase<SubjectSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);

      SubjectSpecK.DomainType.ConstructorCalls.Should ().Be (2);

      AssertResult (OperationResults[1], "<OPERATION>", "<SubjectCreation>", State.Passed);

      AssertResult (OperationResults[7], "<OPERATION>", "subject is created with null values", State.Passed);
      AssertResult (OperationResults[8], "<OPERATION>", "DomainType.ToString()", State.Passed);
    }
  }
}

#endif