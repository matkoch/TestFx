<img src="http://matkoch.github.io/TestFx/gangsterscriptkiddie.gif" height="200px" alt="Scriptkiddie" />

# TestFx

[![Build](https://img.shields.io/teamcity/codebetter/Testfx_Ci.svg?label=master)](http://teamcity.codebetter.com/project.html?projectId=Testfx)
[![Coverage](https://img.shields.io/teamcity/coverage/Testfx_Ci.svg)](http://teamcity.codebetter.com/viewLog.html?buildTypeId=Testfx_Ci&buildId=lastSuccessful&tab=coverage_dotnet)
[![Last Stable](https://img.shields.io/nuget/v/TestFx.svg?label=last stable)](http://nuget.org/packages/TestFx)
[![Downloads](https://img.shields.io/nuget/dt/TestFx.svg?label=downloads)](http://nuget.org/packages/TestFx)
[![License](https://img.shields.io/github/license/matkoch/testfx.svg)](https://github.com/matkoch/TestFx/blob/master/LICENSE)

Writing **high-quality**, **readable**, and **reliable** tests is everything this framework wants you to do. TestFx already includes the core requirements for a pleasent testing experience, like a **console runner** and **ReSharper integration**. It provides easy-to-use facilities to create **new test languages** as well as **adapters to existing frameworks**. Currently, it is most known for [TestFx.Specifications](#testfxspecifications).

If you have questions or comments, please don't hesitate to visit us on
[![Chat](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/matkoch/TestFx).

## TestFx.Specifications

TestFx.Specifications is a fluent syntax extension to TestFx, that defines `SpecK` as a base class for all test classes. When testing a method, the method is only stated once, followed by various test cases for this method. A typical test suite for the `Math.Pow` method could look like this:

![Math.Pow Sample](http://matkoch.github.io/TestFx/math_pow.png)

ReSharper recognizes test cases and also supports the `UnitTestRunContext` action (default on shortcut <kbd>CTRL</kbd>+<kbd>U</kbd>,<kbd>R</kbd>) to execute tests by cursor position. After the execution of tests, ReSharper displays the results in the UnitTestSession view accordingly:

![UnitTestSession Window](http://matkoch.github.io/TestFx/unit_test_session.png)

The output window offers a comprehensive summary for a particular test. It displays a list of all operations executed, along with an indicator whether it was successful or not. Thrown exceptions are displayed by name.

For a more details on the test language, read on with [examples](#examples) or [features](#features).

### Examples

The following examples demonstrate how real-world problems can be solved using TestFx.Specifications.

1. [FizzBuzz](https://github.com/matkoch/TestFx/tree/master/src/Example/101_FizzBuzz)
 * Concept of Act-Arrange-Assert
 * Default and named cases
1. [BankAccount](https://github.com/matkoch/TestFx/tree/master/src/Example/102_BankAccount)
  * Organization of test classes
  * Default and custom subject creation
1. [Calculator](https://github.com/matkoch/TestFx/tree/master/src/Example/103_Calculator)
  * Extension of test DSL
1. [PaymentController](https://github.com/matkoch/TestFx/tree/master/src/Example/104_PaymentController)
  * Faked dependencies ([FakeItEasy](https://github.com/FakeItEasy/FakeItEasy))
  * Random data generation ([Farada](https://github.com/Inspyro/Farada))

### Features

The following feature-list is very focused on every individual feature and thus illustrates them using integration tests.

* [Fluent syntax:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Simple/PassingSpecK.cs) Tests are written with an extensible fluent interface
* [Auto-catch exceptions:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Exceptions/UnexpectedExceptionTest.cs) Exceptions from Act part are automatically catched
* [Exception assertions:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Exceptions/ExceptionAssertionTest.cs) Various pre-defined assertions for exceptions, their message and inner exception
* [Default subject creation:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Subject/DefaultCreationTest.cs) Automatically via marked fields and reflection
* [Custom subject creation:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Subject/CustomCreationTest.cs) Individual per test class
* [AdHoc subject creation:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Subject/AdHocCreationTest.cs) Individual per test case
* [Context scoping:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Using/NestingTest.cs) Scopes are automatically created/disposed after test execution
* [Fake creation:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/FakeItEasy/FakeCreationTest.cs) Automatically create FakeItEasy fakes based on marker attributes
* [Fake setup:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/FakeItEasy/FakeSetupTest.cs) Automatically return values for FakeItEasy fakes based on marker attributes
* [Ordered fake assertions:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/FakeItEasy/OrderedAssertionsTest.cs) Contextless assertion of ordered calls on FakeItEasy fakes
* [Test data creation:](https://github.com/matkoch/TestFx/blob/master/src/TestFx.Specifications.IntegrationTests/Farada/AutoCreationTest.cs) Automatically create test data using Farada

## Acknowledgments

This project was not possible without the help of some special guys. Raise your hands for:

- [@drauch](https://github.com/drauch), [@fschmied](https://github.com/fschmied), [@Inspyro](https://github.com/Inspyro), [@ulrichb](https://github.com/ulrichb) for having so many discussions about testing
- [@citizenmatt](https://github.com/citizenmatt), [@nesteruk](https://github.com/nesteruk), [@kskrygan](https://github.com/kskrygan), [@controlflow](https://github.com/controlflow) for plugin support
- [@xavierdecoster](https://github.com/xavierdecoster) for serving developers with [<img src="http://matkoch.github.io/TestFx/myget.png" height="18px" />](http://myget.org/)
- [<img src="http://matkoch.github.io/TestFx/jetbrains.png" height="18px"/>](http://www.jetbrains.com) for serving developers with [<img src="http://matkoch.github.io/TestFx/codebetter.png" height="18px"/>](http://codebetter.com/)



