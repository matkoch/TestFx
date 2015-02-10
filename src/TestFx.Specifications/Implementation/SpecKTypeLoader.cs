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
using System.Linq;
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Specifications.Implementation
{
  public class SpecKTypeLoader : TypeLoader<ISpecK, SubjectAttribute>
  {
    private readonly IControllerFactory _controllerFactory;
    private readonly ISubjectFactoryGenerator _subjectFactoryGenerator;

    public SpecKTypeLoader (
        IControllerFactory controllerFactory,
        ISubjectFactoryGenerator subjectFactoryGenerator,
        IIntrospectionPresenter introspectionPresenter)
        : base(introspectionPresenter)
    {
      _controllerFactory = controllerFactory;
      _subjectFactoryGenerator = subjectFactoryGenerator;
    }

    protected override void InitializeTypeSpecificFields (ISpecK suite, SuiteProvider provider)
    {
      var suiteType = suite.GetType();
      var closedSpeckType = suiteType.GetClosedTypeFor(typeof (ISpecK<>));
      var subjectType = closedSpeckType.GetGenericArguments().Single();

      var suiteController = _controllerFactory.CreateClassSuiteController(suite, subjectType, provider);
      var subjectFactory = _subjectFactoryGenerator.GetFactory(suiteType);

      suite.SetMemberValue("_classSuiteController", suiteController);
      suite.SetMemberValue("_subjectFactory", subjectFactory);
    }
  }
}