using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TestFx.Utilities
{
  /// <summary>
  /// Exception that is thrown when sorting a set of items is not possible due to a circular dependency.
  /// </summary>
  public class CircularDependencyException : Exception
  {
    private const string c_message = "Circular dependencies detected:\r\n";

    public CircularDependencyException (IEnumerable<IEnumerable> cycles)
    {
      Cycles = cycles;
    }

    protected CircularDependencyException (SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public IEnumerable<IEnumerable> Cycles { get; private set; }

    public override string Message
    {
      get { return c_message + Cycles; }
    }
  }
}