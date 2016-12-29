var resultData = {
  "text": "802b2818",
  "icon": "glyphicon glyphicon-remove red",
  "nodes": [
    {
      "text": "TestFx.SpecK.Tests.dll",
      "icon": "glyphicon glyphicon-remove red",
      "output": "Operations:\r\n✅ MyAssemblySetup.Setup\r\n✅ MyAssemblySetup.Setup\r\n.. InnerOperations ..\r\n✅ MyAssemblySetup.Cleanup\r\n✅ MyAssemblySetup.Cleanup\r\n",
      "nodes": [
        {
          "text": "ContextBehaviorTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ subject with ctor arg\r\n✅ &lt;Action&gt;\r\n✅ has result set to ctor arg\r\n⛔ has property set to null (AssertionException)\r\n\r\n\r\nNUnit.Framework.AssertionException:\r\nExpected string to be &lt;null&gt;, but found &quot;ctor arg&quot;.\r\n   at FluentAssertions.Execution.LateBoundTestFramework.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Shared\\Execution\\LateBoundTestFramework.cs:line 31\r\n   at FluentAssertions.Execution.TestFrameworkProvider.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\FluentAssertions.Net40\\Execution\\TestFrameworkProvider.cs:line 42\r\n   at FluentAssertions.Execution.DefaultAssertionStrategy.HandleFailure(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\DefaultAssertionStrategy.cs:line 25\r\n   at FluentAssertions.Execution.AssertionScope.FailWith(String message, Object[] args) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\AssertionScope.cs:line 197\r\n   at FluentAssertions.Primitives.ReferenceTypeAssertions`2.BeNull(String because, Object[] reasonArgs) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Primitives\\ReferenceTypeAssertions.cs:line 33\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "FailingTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ Create PassingDisposable\r\n⛔ Create ThrowingDisposable (TargetInvocationException)\r\n✅ Dispose PassingDisposable\r\n\r\n\r\nSystem.Reflection.TargetInvocationException:\r\nException has been thrown by the target of an invocation.\r\n---&gt; Exception of type &#39;System.Exception&#39; was thrown.\r\n   at System.RuntimeTypeHandle.CreateInstance(RuntimeType type, Boolean publicOnly, Boolean noCheck, Boolean&amp; canBeCached, RuntimeMethodHandleInternal&amp; ctor, Boolean&amp; bNeedSecurityCheck)\r\n   at System.RuntimeType.CreateInstanceSlow(Boolean publicOnly, Boolean skipCheckThis, Boolean fillCache, StackCrawlMark&amp; stackMark)\r\n   at System.RuntimeType.CreateInstanceDefaultCtor(Boolean publicOnly, Boolean skipCheckThis, Boolean fillCache, StackCrawlMark&amp; stackMark)\r\n   at System.Activator.CreateInstance(Type type, Boolean nonPublic)\r\n   at System.RuntimeType.CreateInstanceImpl(BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture, Object[] activationAttributes, StackCrawlMark&amp; stackMark)\r\n   at System.Activator.CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture, Object[] activationAttributes)\r\n   at System.Activator.CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture)\r\n\r\n--- Begin of inner exception stack trace ---\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "NestingTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ Create FirstDisposable\r\n✅ Create SecondDisposable (named)\r\n✅ Create DelegateDisposable\r\n✅ Arrangement\r\n✅ Create ThirdDisposable\r\n✅ &lt;Action&gt;\r\n⛔ Failing Assertion (Exception)\r\n✅ Dispose ThirdDisposable\r\n✅ Dispose DelegateDisposable\r\n✅ Dispose SecondDisposable (named)\r\n✅ Dispose FirstDisposable\r\n\r\n\r\nSystem.Exception:\r\nException of type &#39;System.Exception&#39; was thrown.\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "CustomCreationTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Create_Subject&gt;\r\n✅ &lt;Action&gt;\r\n✅ passes OtherString\r\n✅ creates subject only once\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "AdHocCreationTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ init OtherString\r\n✅ subject is created with OtherString\r\n✅ &lt;Action&gt;\r\n✅ passes OtherString\r\n✅ creates subject only once\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "NoDefaultConstructorTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n⛔ &lt;Create_Subject&gt; (EvaluationException)\r\n\r\n\r\nTestFx.Evaluation.EvaluationException:\r\nMissing default constructor for subject type &#39;DomainType&#39;.\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "MissingArgumentsTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n⛔ &lt;Create_Subject&gt; (EvaluationException)\r\n\r\n\r\nTestFx.Evaluation.EvaluationException:\r\nMissing constructor arguments for subject type &#39;DomainType&#39;: firstMissingString, secondMissingString\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "DefaultCreationTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Create_Subject&gt;\r\n✅ &lt;Action&gt;\r\n✅ passes InjectedString\r\n✅ creates subject only once\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "OrderedAssertionsTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Create_Fakes&gt;\r\n✅ &lt;Create_Subject&gt;\r\n✅ &lt;Action&gt;\r\n⛔ calls in order first and second disposable (ExpectationException)\r\n\r\n\r\nFakeItEasy.ExpectationException:\r\n\r\n\r\n  Assertion failed for the following calls:\r\n    &#39;System.IDisposable.Dispose()&#39; repeated at least once\r\n    &#39;System.IDisposable.Dispose()&#39; repeated at least once\r\n  The calls where found but not in the correct order among the calls:\r\n    1: System.IDisposable.Dispose()\r\n    2: System.IDisposable.Dispose()\r\n\r\n   at FakeItEasy.Core.OrderedFakeAsserter.ThrowExceptionWhenAssertionFailed(List`1 assertedCalls, CallWriter callWriter, IEnumerable`1 originalCallList)\r\n   at FakeItEasy.Core.OrderedFakeAsserter.AssertWasCalled(Func`2 callPredicate, String callDescription, Func`2 repeatPredicate, String repeatDescription)\r\n   at FakeItEasy.Core.OrderedFakeAsserterFactory.CompositeAsserter.AssertWasCalled(Func`2 callPredicate, String callDescription, Func`2 repeatPredicate, String repeatDescription)\r\n   at FakeItEasy.Configuration.RuleBuilder.MustHaveHappened(Repeated repeatConstraint)\r\n   at FakeItEasy.AssertConfigurationExtensions.MustHaveHappened(IAssertConfiguration configuration)\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "FakeSetupTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Create_Fakes&gt;\r\n✅ &lt;Setup_Fakes&gt;\r\n✅ &lt;Create_Subject&gt;\r\n✅ &lt;Action&gt;\r\n✅ retrieves Service from ServiceProvider\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "FakeCreationTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Create_Fakes&gt;\r\n✅ &lt;Create_Subject&gt;\r\n✅ FormatProvider returns\r\n✅ &lt;Action&gt;\r\n✅ returns FormatProvider\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "InconclusiveTest+DomainSpec",
          "icon": "glyphicon glyphicon-alert orange",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-alert orange",
              "output": "Operations:\r\n⛖ arranges something\r\n✅ &lt;Action&gt;\r\n⛖ does something\r\n✅ passes\r\n⛔ fails (Exception)\r\n\r\n\r\nSystem.Exception:\r\nException of type &#39;System.Exception&#39; was thrown.\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "IsolationTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "Setting",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n",
              "nodes": null
            },
            {
              "text": "Reusing",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n✅ resets instance object\r\n✅ saves static object\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "InheritanceTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "Base case",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Action&gt;\r\n\r\nOutput:\r\nSTD: True\r\nSTD: \r\n\r\n",
              "nodes": null
            },
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Action&gt;\r\n✅ Assertion\r\n\r\nOutput:\r\nSTD: True\r\nSTD: \r\n\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "InheritanceTest+SpecializedDomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "Base case",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n\r\nOutput:\r\nSTD: True\r\nSTD: \r\n\r\n",
              "nodes": null
            },
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n⛔ Assertion (Exception)\r\n\r\nOutput:\r\nSTD: True\r\nSTD: \r\n\r\n\r\n\r\nSystem.Exception:\r\nException of type &#39;System.Exception&#39; was thrown.\r\n",
              "nodes": null
            },
            {
              "text": "Additional case",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n\r\nOutput:\r\nSTD: True\r\nSTD: \r\n\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "VariableTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ set MyInteger\r\n✅ &lt;Set_Variables&gt;\r\n✅ &lt;Action&gt;\r\n✅ holds variables\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "FailingTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Action&gt;\r\n⛔ Failing assertion (Exception)\r\n✅ Passing assertion\r\n⛔ Another failing assertion (Exception)\r\n\r\n\r\nSystem.Exception:\r\nException of type &#39;System.Exception&#39; was thrown.\r\n\r\n\r\nSystem.Exception:\r\nException of type &#39;System.Exception&#39; was thrown.\r\n",
              "nodes": null
            },
            {
              "text": "Passing",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Action&gt;\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "HaltingTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n⛔ Throwing arrangement (Exception)\r\n\r\n\r\nSystem.Exception:\r\nException of type &#39;System.Exception&#39; was thrown.\r\n",
              "nodes": null
            },
            {
              "text": "Passing",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Action&gt;\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "PassingTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n✅ Assertion\r\n\r\nOutput:\r\nSTD: True\r\nSTD: \r\n\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "SimpleSetupExecutionTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n✅ SetupOnceMethod\r\n✅ &lt;lambda method&gt;\r\n.. InnerOperations ..\r\n✅ &lt;lambda method&gt;\r\n✅ CleanupOnceMethod\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;ConvertToNonGeneric&gt;b__0\r\n✅ subject static subject1\r\n✅ &lt;Action&gt;\r\n✅ has assembly setup\r\n✅ &lt;ConvertToNonGeneric&gt;b__0\r\n",
              "nodes": null
            },
            {
              "text": "Case 2",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;ConvertToNonGeneric&gt;b__0\r\n✅ subject static subject2\r\n✅ &lt;Action&gt;\r\n✅ &lt;ConvertToNonGeneric&gt;b__0\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "ThrowingSetupExecutionTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n✅ SetupOnceMethod\r\n⛔ &lt;.cctor&gt;b__12_0 (Exception)\r\n.. InnerOperations ..\r\n✅ CleanupOnceMethod\r\n\r\n\r\nSystem.Exception:\r\nException of type &#39;System.Exception&#39; was thrown.\r\n",
          "nodes": []
        },
        {
          "text": "GenericArgumentTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "Passing",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n✅ Returns x =&gt; Convert(1)\r\n",
              "nodes": null
            },
            {
              "text": "Failing Value",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n⛔ Returns x =&gt; &quot;two&quot; (Exception)\r\n\r\n\r\nSystem.Exception:\r\nResult must be equal to &#39;two&#39; but was &#39;one&#39;.\r\n",
              "nodes": null
            },
            {
              "text": "Failing Type",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n⛔ Returns x =&gt; &quot;one&quot; (Exception)\r\n\r\n\r\nSystem.Exception:\r\nResult must be equal to &#39;one&#39; but was &#39;1&#39;.\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "AutoConfigurationTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Create_AutoData&gt;&lt;1337&gt;\r\n✅ &lt;Action&gt;\r\n✅ Fills properties\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "AutoCreationTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Create_AutoData&gt;&lt;1337&gt;\r\n✅ &lt;Action&gt;\r\n✅ Fills properties\r\n✅ Fills fields\r\n✅ Executes AutoAttribute.Mutate\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "UnexpectedExceptionTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ a message\r\n✅ an inner exception with message\r\n⛔ &lt;Action&gt; (ArgumentException)\r\n\r\n\r\nSystem.ArgumentException:\r\nMessage\r\n---&gt; InnerMessage\r\n   at UserNamespace.UserClass.Throw[TException](String message, Exception innerException) in C:\\OSS\\TestFx2\\src\\TestFx.SpecK.Tests\\Exceptions\\UserClass.cs:line 27\r\n\r\n--- Begin of inner exception stack trace ---\r\n\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "ExceptionAssertionTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ a message\r\n✅ an inner exception\r\n✅ &lt;Action&gt;\r\n✅ Throws ArgumentException\r\n",
              "nodes": null
            },
            {
              "text": "Wrong exception type",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n⛔ Throws InvalidOperationException (Exception)\r\n\r\n\r\nSystem.Exception:\r\nException must be assignable to &#39;System.InvalidOperationException&#39; but was &#39;ArgumentNullException&#39;.\r\n",
              "nodes": null
            },
            {
              "text": "Wrong message",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n⛔ Throws ArgumentException (Exception)\r\n\r\n\r\nSystem.Exception:\r\nException message must be &#39;Wrong message&#39; but was &#39;[NullGuard] message is null.\r\nParameter name: message&#39;.\r\n",
              "nodes": null
            },
            {
              "text": "Wrong message provider",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ a message\r\n✅ &lt;Action&gt;\r\n⛔ Throws ArgumentException (Exception)\r\n\r\n\r\nSystem.Exception:\r\nException message must be &#39;Wrong message&#39; but was &#39;[NullGuard] innerException is null.\r\nParameter name: innerException&#39;.\r\n",
              "nodes": null
            },
            {
              "text": "Wrong inner exception provider",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ a message\r\n✅ &lt;Action&gt;\r\n⛔ Throws ArgumentException (Exception)\r\n\r\n\r\nSystem.Exception:\r\nException message must be &#39;Message&#39; but was &#39;[NullGuard] innerException is null.\r\nParameter name: innerException&#39;.\r\n",
              "nodes": null
            },
            {
              "text": "Custom failing assertion",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n⛔ Throws exception with special properties (AssertionException)\r\n\r\n\r\nNUnit.Framework.AssertionException:\r\nExpected object not to be &lt;null&gt;.\r\n   at FluentAssertions.Execution.LateBoundTestFramework.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Shared\\Execution\\LateBoundTestFramework.cs:line 31\r\n   at FluentAssertions.Execution.TestFrameworkProvider.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\FluentAssertions.Net40\\Execution\\TestFrameworkProvider.cs:line 42\r\n   at FluentAssertions.Execution.DefaultAssertionStrategy.HandleFailure(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\DefaultAssertionStrategy.cs:line 25\r\n   at FluentAssertions.Execution.AssertionScope.FailWith(String message, Object[] args) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\AssertionScope.cs:line 197\r\n   at FluentAssertions.Primitives.ReferenceTypeAssertions`2.NotBeNull(String because, Object[] reasonArgs) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Primitives\\ReferenceTypeAssertions.cs:line 53\r\n",
              "nodes": null
            },
            {
              "text": "Custom passing assertion",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Action&gt;\r\n✅ Throws exception with special properties\r\n",
              "nodes": null
            }
          ]
        },
        {
          "text": "PermutationsTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n.. InnerOperations ..\r\n",
              "nodes": [
                {
                  "text": "Object = Object, A = 1, B = 3",
                  "icon": "glyphicon glyphicon-remove red",
                  "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n⛔ returns result (AssertionException)\r\n\r\n\r\nNUnit.Framework.AssertionException:\r\nExpected value to be 5, but found 4.\r\n   at FluentAssertions.Execution.LateBoundTestFramework.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Shared\\Execution\\LateBoundTestFramework.cs:line 31\r\n   at FluentAssertions.Execution.TestFrameworkProvider.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\FluentAssertions.Net40\\Execution\\TestFrameworkProvider.cs:line 42\r\n   at FluentAssertions.Execution.DefaultAssertionStrategy.HandleFailure(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\DefaultAssertionStrategy.cs:line 25\r\n   at FluentAssertions.Execution.AssertionScope.FailWith(String message, Object[] args) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\AssertionScope.cs:line 197\r\n   at FluentAssertions.Numeric.NumericAssertions`1.Be(T expected, String because, Object[] reasonArgs) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Numeric\\NumericAssertions.cs:line 44\r\n",
                  "nodes": null
                },
                {
                  "text": "Object = Object, A = 1, B = 4",
                  "icon": "glyphicon glyphicon-ok green",
                  "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n✅ returns result\r\n",
                  "nodes": null
                },
                {
                  "text": "Object = Object, A = 2, B = 3",
                  "icon": "glyphicon glyphicon-ok green",
                  "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n✅ returns result\r\n",
                  "nodes": null
                },
                {
                  "text": "Object = Object, A = 2, B = 4",
                  "icon": "glyphicon glyphicon-remove red",
                  "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n⛔ returns result (AssertionException)\r\n\r\n\r\nNUnit.Framework.AssertionException:\r\nExpected value to be 5, but found 6.\r\n   at FluentAssertions.Execution.LateBoundTestFramework.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Shared\\Execution\\LateBoundTestFramework.cs:line 31\r\n   at FluentAssertions.Execution.TestFrameworkProvider.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\FluentAssertions.Net40\\Execution\\TestFrameworkProvider.cs:line 42\r\n   at FluentAssertions.Execution.DefaultAssertionStrategy.HandleFailure(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\DefaultAssertionStrategy.cs:line 25\r\n   at FluentAssertions.Execution.AssertionScope.FailWith(String message, Object[] args) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\AssertionScope.cs:line 197\r\n   at FluentAssertions.Numeric.NumericAssertions`1.Be(T expected, String because, Object[] reasonArgs) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Numeric\\NumericAssertions.cs:line 44\r\n",
                  "nodes": null
                }
              ]
            }
          ]
        },
        {
          "text": "SequencesTest+DomainSpec",
          "icon": "glyphicon glyphicon-remove red",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "&lt;Default&gt;",
              "icon": "glyphicon glyphicon-remove red",
              "output": "Operations:\r\n.. InnerOperations ..\r\n",
              "nodes": [
                {
                  "text": "First sequence",
                  "icon": "glyphicon glyphicon-ok green",
                  "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n✅ returns result\r\n",
                  "nodes": null
                },
                {
                  "text": "Second sequence",
                  "icon": "glyphicon glyphicon-remove red",
                  "output": "Operations:\r\n✅ &lt;Reset_Instance_Fields&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Arrangement&gt;\r\n✅ &lt;Action&gt;\r\n⛔ returns result (AssertionException)\r\n\r\n\r\nNUnit.Framework.AssertionException:\r\nExpected value to be 6, but found 5.\r\n   at FluentAssertions.Execution.LateBoundTestFramework.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Shared\\Execution\\LateBoundTestFramework.cs:line 31\r\n   at FluentAssertions.Execution.TestFrameworkProvider.Throw(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\FluentAssertions.Net40\\Execution\\TestFrameworkProvider.cs:line 42\r\n   at FluentAssertions.Execution.DefaultAssertionStrategy.HandleFailure(String message) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\DefaultAssertionStrategy.cs:line 25\r\n   at FluentAssertions.Execution.AssertionScope.FailWith(String message, Object[] args) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Execution\\AssertionScope.cs:line 197\r\n   at FluentAssertions.Numeric.NumericAssertions`1.Be(T expected, String because, Object[] reasonArgs) in C:\\projects\\fluentassertions-vf06b\\Src\\Core\\Numeric\\NumericAssertions.cs:line 44\r\n",
                  "nodes": null
                }
              ]
            }
          ]
        },
        {
          "text": "AsyncTest+DomainSpec",
          "icon": "glyphicon glyphicon-ok green",
          "output": "Operations:\r\n.. InnerOperations ..\r\n",
          "nodes": [
            {
              "text": "Action",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Action&gt;\r\n✅ waits\r\n",
              "nodes": null
            },
            {
              "text": "Func",
              "icon": "glyphicon glyphicon-ok green",
              "output": "Operations:\r\n✅ &lt;Action&gt;\r\n✅ Returns x =&gt; 10\r\n",
              "nodes": null
            }
          ]
        }
      ]
    }
  ]
};