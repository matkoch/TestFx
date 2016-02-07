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
using Autofac;
using FakeItEasy;
using FakeItEasy.Core;
using NUnit.Framework;
using TestFx.Evaluation;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Runners;

namespace TestFx.TestInfrastructure
{
  [TestFixture]
  public abstract class TestBase<T>
  {
    private IRootRunner _rootRunner;

    [TestFixtureSetUp]
    public void FixtureSetUp ()
    {
      var builder = new ContainerBuilder();
      var evaluationModule = new EvaluationModule(new RunListener(), useSeparateAppDomains: false);
      builder.RegisterModule(evaluationModule);
      var container = builder.Build();
      _rootRunner = container.Resolve<IRootRunner>();
    }

    [SetUp]
    public virtual void SetUp ()
    {
    }

    [Test]
    public void Test ()
    {
      IRunResult runResult;
      IFakeScope scope;

      var runIntent = RunIntent.Create(useSeparateAppDomains: false);
      runIntent.AddType(typeof (T));

      using (scope = Fake.CreateScope())
      {
        runResult = _rootRunner.Run(runIntent);
      }

      AssertResults(runResult, scope);
    }

    protected abstract void AssertResults (IRunResult runResult, IFakeScope scope);
  }
}