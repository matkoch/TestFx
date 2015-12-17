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
using System.Linq;
using NUnit.Framework;
using TestFx.MSpec.Implementation;

namespace TestFx.MSpec.Tests
{
  [TestFixture]
  public class HierarchyLoaderTest
  {
    [Test]
    public void Test ()
    {
      var hierarchy = HierarchyLoader.GetExecutionHierarchy(typeof (U3.U6)).ToList();

      var expectedHierarchy = new[]
                              {
                                  typeof (U1),
                                  typeof (X1.U2),
                                  typeof (U3),
                                  typeof (U4),
                                  typeof (X2.U5),
                                  typeof (U3.U6)
                              };
      Assert.That(hierarchy, Is.EqualTo(expectedHierarchy));
    }
  }

  public class U3 : X1.U2
  {
    public class U6 : X2.U5
    {
    }
  }

  public class X1
  {
    public class U2 : U1
    {
    }
  }

  public class U1
  {
  }

  public class X2
  {
    public class U5 : U4
    {
    }
  }

  public class U4
  {
  }
}