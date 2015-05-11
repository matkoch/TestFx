using System;
using System.Collections;
using System.Runtime.Serialization;

namespace TestFx.Utilities.Collections
{
  /// <summary>
  /// Exception that is thrown when ordering a set of items where the order is not totally defined.
  /// </summary>
  public class UndefinedOrderException : Exception
  {
    private const string c_message = "Undefined order for items:\r\n";

    public UndefinedOrderException (IEnumerable items)
    {
      Items = items;
    }

    protected UndefinedOrderException (SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public IEnumerable Items { get; private set; }

    public override string Message
    {
      get { return c_message + Items; }
    }
  }
}