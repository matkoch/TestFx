**NOTE: TestFx is in an early development stage. It is not published to the offical NuGet Gallery and ReSharper Extension Gallery. Please use https://www.myget.org/F/matkoch/api/v2 to install the NuGet Package and ReSharper Extension.**

[<img src="http://catholictechtalk.com/wp-content/uploads/2012/09/Skid.jpg" height="300px"/>](https://github.com/matkoch/TestFx/wiki/Meanwhile-in-a-parallel-universe)

# TestFx <img src="https://raw.githubusercontent.com/ao5357/retina-spacer-gif/master/spacer@2x.gif" width="385px" height="10px" /> [<img src="https://www.myget.org/BuildSource/Badge/matkoch?identifier=2f4ec854-4227-47f0-8bca-6bea2ef5e555" height="14px"/>](https://www.myget.org/) [<img src="http://teamcity.codebetter.com/app/rest/builds/buildType:bt1226/statusIcon" height="14px"/>](http://teamcity.codebetter.com/viewType.html?buildTypeId=bt1226&guest=1)#

We love clean code, but still our tests are often difficult to read or take more time than actually implementing the stuff. TestFx is a framework that brings testing to the next level:

- **Readability:** Like code, tests are written once, but read often. That's why reading a tests should be like reading a story or a specification. TestFx improves readability by introducing a fluent interface that is short, clean, expressive, and extensible.
- **Flexibility:** As a good developer we know all these wonderful features like generics, lambda expressions or extension methods. TestFx encourages you to use them all in order to make your tests context-aware in every single line.
- **Integration:** The .NET community serves us with such great tools so that we only have one choice - integrate them. TestFx currently comes with dedicated support for [ReSharper](https://www.jetbrains.com/resharper/), [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy), and [Farada](https://github.com/Inspyro/Farada).
- **Extensibility:** Besides the fluent syntax, it is also possible to extend the framework with your own test language. You just have to implement some basic interfaces and will have most of the ReSharper support out of the box.

## Features ##

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

## 101 Examples ##

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

## Acknowledgments ##

This project was not possible without the help of some special guys. I want to thank:

- [@drauch](https://github.com/drauch), [@fschmied](https://github.com/fschmied), [@Inspyro](https://github.com/Inspyro), [@ulrichb](https://github.com/ulrichb) for having so many discussions about testing
- [@citizenmatt](https://github.com/citizenmatt), [@nesteruk](https://github.com/nesteruk), and the rest of the ReSharper team for supporting me in writing the plugin
- [@xavierdecoster](https://github.com/xavierdecoster) for serving developers with MyGet

## <img src="https://raw.githubusercontent.com/ao5357/retina-spacer-gif/master/spacer@2x.gif" width="250px" height="10px" /> Powered by ##

<img src="https://raw.githubusercontent.com/ao5357/retina-spacer-gif/master/spacer@2x.gif" width="42px" height="10px" />
[<img src="http://confluence.jetbrains.com/download/attachments/2/logo_jetbrains.gif?version=1&modificationDate=1255699747000" height="45px"/>](http://jetbrains.com/)
<img src="https://raw.githubusercontent.com/ao5357/retina-spacer-gif/master/spacer@2x.gif" width="85px" height="10px" />
[<img src="http://cdn1.codebetter.com/wp-content/themes/codebetter/images/codebetter_logo.png" height="40px"/>](http://codebetter.com/)
<img src="https://raw.githubusercontent.com/ao5357/retina-spacer-gif/master/spacer@2x.gif" width="85px" height="10px" />[<img src="https://my.pingdom.com/uploads/v4x5589hs7yg.png?q=1397642676" height="35px" />](http://myget.org/)
