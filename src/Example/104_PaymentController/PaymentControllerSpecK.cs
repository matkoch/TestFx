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
using FakeItEasy;
using TestFx.FakeItEasy;
using TestFx.Farada;
using TestFx.Specifications;

namespace Example._104_PaymentController
{
  [Subject (typeof (PaymentController), "Pay")]
  public class PaymentControllerSpecK : SpecK<PaymentController>
  {
    [Injected] [Faked] IPaymentService PaymentService;

    [Auto] PaymentModel Model;

    PaymentControllerSpecK ()
    {
      Specify (x => x.Pay (Model))
          .DefaultCase (_ => _
              .Given ("PaymentService returns true", x => A.CallTo (PaymentService).WithReturnType<bool> ().Returns (true))
              .ItReturns (x => "Success")
              .It ("calls PaymentService with model data",
                  x => A.CallTo (() => PaymentService.PayWithCreditCard (Model.Owner, Model.Number, Model.Validity, Model.Cvc)).MustHaveHappened ()))
          .Case ("Rejection", _ => _
              .Given ("PaymentService returns false", x => A.CallTo (PaymentService).WithReturnType<bool> ().Returns (false))
              .ItReturns (x => "Error"));
    }
  }
}