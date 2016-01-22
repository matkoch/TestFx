using System;
using System.Collections;
using System.Runtime.Serialization;

namespace TestFx.Utilities.Collections
{
  /// <summary>
  /// Exception that is thrown when ordering a set of items where the order is not totally defined.
  /// </summary>
  [Serializable]
  public class UndefinedOrderException : Exception
  {
    private const string c_message = "Undefined order of items.";

    public UndefinedOrderException ()
      : base(c_message)
    {
    }

    protected UndefinedOrderException (SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
  }
}