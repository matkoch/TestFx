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

namespace Example._102_BankAccount
{
  public class BankAccount
  {
    public BankAccount (decimal initialBalance)
    {
      CurrentBalance = initialBalance;
    }

    public decimal CurrentBalance { get; private set; }

    public void Deposit (decimal amount)
    {
      EnsureAmountIsGreaterZero (amount);

      CurrentBalance += amount;
    }

    public void Withdraw (decimal amount)
    {
      EnsureAmountIsGreaterZero (amount);

      if (amount > CurrentBalance)
        throw new BlankException (string.Format ("Current balance of {0} doesn't allow withdrawal of {1}.", CurrentBalance, amount));

      CurrentBalance -= amount;
    }

    void EnsureAmountIsGreaterZero (decimal amount)
    {
      if (amount <= 0)
        throw new ArgumentException ("Amount must be greater than zero.");
    }
  }
}