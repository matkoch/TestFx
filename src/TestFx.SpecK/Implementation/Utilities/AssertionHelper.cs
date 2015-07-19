// Copyright 2015, 2014 Matthias Koch
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

namespace TestFx.SpecK.Implementation.Utilities
{
  public static class AssertionHelper
  {
    // TODO: can be null?
    public static void AssertObjectEquals (string objectName, [CanBeNull] object expectedObject, [CanBeNull] object actualObject)
    {
      // TODO: own exception type / 
      if (!Equals(expectedObject, actualObject))
        throw new Exception(String.Format("{0} must be equal to '{1}' but was '{2}'.", objectName, expectedObject ?? "null", actualObject ?? "null"));
    }

    public static void AssertInstanceOfType (string objectName, Type expectedType, object actualObject)
    {
      if (!expectedType.IsInstanceOfType(actualObject))
        throw new Exception(
            String.Format(
                "{0} must be assignable to '{1}' but was '{2}'.",
                objectName,
                expectedType,
                actualObject != null ? actualObject.GetType().Name : "null"));
    }

    public static void AssertExceptionMessage (string expectedMessage, Exception exception)
    {
      if (exception.Message != expectedMessage)
        throw new Exception(String.Format("Exception message must be '{0}' but was '{1}'.", expectedMessage, exception.Message));
    }
  }
}