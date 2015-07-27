<img src="http://matkoch.github.io/TestFx/gangsterscriptkiddie.gif" height="200px" alt="Scriptkiddie" />

## TestFx

[![Build](https://img.shields.io/teamcity/codebetter/Testfx_Ci.svg?label=master)](http://teamcity.codebetter.com/project.html?projectId=Testfx)
[![Coverage](https://img.shields.io/teamcity/coverage/Testfx_Ci.svg)](http://teamcity.codebetter.com/viewLog.html?buildTypeId=Testfx_Ci&buildId=lastSuccessful&tab=coverage_dotnet)
[![Latest Beta](https://img.shields.io/nuget/v/TestFx.Core.svg?label=latest beta)](http://nuget.org/packages/TestFx.Core)
[![License](https://img.shields.io/github/license/matkoch/testfx.svg)](https://github.com/matkoch/TestFx/blob/master/LICENSE)
[![Chat](https://img.shields.io/badge/gitter-join_chat-ff69b4.svg)](https://gitter.im/matkoch/TestFx)

*Yet another test framework? [Yes!](https://github.com/matkoch/TestFx/wiki/Motivation)*

TestFx is indeed another test framework, but it is also a **platform**. Its novelty lies in the **generalization of execution**, allowing a test to be represented in every possible fashion. Developers can now **invent new test languages** more rapidly, without worrying about the foundation for a pleasent testing experience, like a **console runner** or **ReSharper integration**.

Built on top of the platform, [TestFx.SpecK](https://github.com/matkoch/TestFx/wiki/SpecK) enables you to write reliable tests using an **extensible fluent interface**. By using the `Spec` base class, it **eliminates boiler-plate code** from your test suites, like setting up fakes and creating the subject instances. The actual assertions can be written with any third-party library you're familar with.

A typical test suite for the `Math.Pow` method could look like this:

![Math.Pow Sample](http://matkoch.github.io/TestFx/math_pow-1.png)

ReSharper recognizes test cases and also supports the `UnitTestRunContext` action (default on shortcut <kbd>CTRL</kbd>+<kbd>U</kbd>,<kbd>R</kbd>) to execute tests by cursor position. After the execution of tests, ReSharper displays the results in the UnitTestSession view accordingly:

![UnitTestSession Window](http://matkoch.github.io/TestFx/unit_test_session-1.png)

The output window offers a comprehensive summary for a particular test. It displays a list of all operations executed, along with an indicator whether it was successful or not. Thrown exceptions are displayed by name.

## Acknowledgments

Raise your hands for some special guys who helped me out a lot:

- [@drauch](https://github.com/drauch), [@fschmied](https://github.com/fschmied), [@Inspyro](https://github.com/Inspyro), [@ulrichb](https://github.com/ulrichb) for having so many discussions about testing
- [@citizenmatt](https://github.com/citizenmatt), [@kropp](https://github.com/kropp), [@kskrygan](https://github.com/kskrygan), [@nesteruk](https://github.com/nesteruk),  [@controlflow](https://github.com/controlflow) for plugin support
- [@xavierdecoster](https://github.com/xavierdecoster) for serving developers with [<img src="http://matkoch.github.io/TestFx/myget.png" height="18px" />](http://myget.org/)
- [<img src="http://matkoch.github.io/TestFx/jetbrains.png" height="18px"/>](http://www.jetbrains.com) for serving developers with [<img src="http://matkoch.github.io/TestFx/codebetter.png" height="18px"/>](http://codebetter.com/)
