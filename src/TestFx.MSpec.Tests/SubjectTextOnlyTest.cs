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
using System.Linq;
using FakeItEasy.Core;
using Machine.Specifications;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.MSpec.Tests
{
  [Subject ("Foo")]
  internal class when_subject_text_only
  {
    It dummy = () => { };
  }

  [TestFixture]
  internal class SubjectTextOnlyTest : TestBase<when_subject_text_only>
  {
    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetClassSuiteResult().HasText("Foo, when subject text only");
    }
  }
}