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
using FluentAssertions;
using TestFx.Specifications;

namespace Example._102_BankAccount
{
  /// <summary>
  /// 
  /// This example shows how SpecK tests are organized if 
  /// you want to test more than one method per subject type.
  /// 
  /// Furthermore, it demonstrates how the base class can
  /// take over the creation of the subject instance.
  /// 
  /// </summary>
  /// <remarks>
  /// 
  /// In order to comply with the SpecK standard, each
  /// tested method should go into its own test class. This
  /// avoids confusion and also offers better possibilities
  /// for navigation in ReSharper.
  /// 
  /// For each test case, a new instance of the subject type
  /// must be created. This is something the SpecK base class
  /// can deal with. You simply declare the subject type as
  /// generic argument and define instance fields marked with
  /// the Injected-Attribute. The CreateSubject method will
  /// call the subject constructor, and inject all fields
  /// as arguments. This method can be overridden and is
  /// executed as first operation.
  /// 
  /// </remarks>
  public class BankAccountSpecK : SpecK<BankAccount>
  {
    // Injection allows to change casing.
    [Injected] static decimal InitialBalance = 100;

    // Custom subject creation if necessary.
    public override BankAccount CreateSubject ()
    {
      // Otherwise, call automatic subject creation.
      return base.CreateSubject (); // Analogue to new BankAccount (InitialBalance);
    }

    decimal Amount;

    [Subject (typeof (BankAccount), "Deposit")]
    public class BankAccountDepositSpecK : BankAccountSpecK
    {
      BankAccountDepositSpecK ()
      {
        Specify (x => x.Deposit (Amount))
            .DefaultCase (_ => _
                .Given (x => Amount = 50)
                .It ("Increases balance", x => x.Subject.CurrentBalance.Should ().Be (150)))
            .Case ("Non-positive amount", _ => _
                .Given (x => Amount = 0)
                .ItThrows (typeof (ArgumentException), "Amount must be greater than zero."));
      }
    }

    [Subject (typeof (BankAccount), "Withdraw")]
    public class BankAccountWithdrawSpecK : BankAccountSpecK
    {
      BankAccountWithdrawSpecK ()
      {
        Specify (x => x.Withdraw (Amount))
            .DefaultCase (_ => _
                .Given (x => Amount = 50)
                .It ("Decreases balance", x => x.Subject.CurrentBalance.Should ().Be (50)))
            .Case ("Insufficient money", _ => _
                .Given (x => Amount = 150)
                .ItThrows (typeof (BlankException), "Current balance of 100 doesn't allow withdrawal of 150.")
                .It ("Keeps balance", x => x.Subject.CurrentBalance.Should ().Be (100)))
            .Case ("Non-positive amount", _ => _
                .Given (x => Amount = 0)
                .ItThrows (typeof (ArgumentException), "Amount must be greater than zero."));
      }
    }
  }
}