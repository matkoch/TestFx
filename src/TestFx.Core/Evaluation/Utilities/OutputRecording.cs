// Copyright 2016, 2015, 2014 Matthias Koch
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using TestFx.Evaluation.Results;

namespace TestFx.Evaluation.Utilities
{
  public interface IOutputRecording : IDisposable, IEnumerable<OutputEntry>
  {
  }

  internal sealed class OutputRecording : IOutputRecording
  {
    private readonly TextWriter _previousStdOut;
    private readonly TextWriter _previousErrOut;
    private readonly DelegateTraceListener _previousTraceListener;

    private readonly List<OutputEntry> _entries = new List<OutputEntry>();
    private readonly object _lockObject = new object();

    public OutputRecording ()
    {
      _previousStdOut = Console.Out;
      _previousErrOut = Console.Error;
      _previousTraceListener = Trace.Listeners.OfType<DelegateTraceListener>().SingleOrDefault();

      lock (_lockObject)
      {
        SwapTraceListeners(new DelegateTraceListener(m => Write(OutputType.Debug, m)), _previousTraceListener);
        ConfigureConsole(new DelegateStringWriter(m => Write(OutputType.Standard, m)), new DelegateStringWriter(m => Write(OutputType.Error, m)));
      }
    }

    public void Dispose ()
    {
      var traceListener = Trace.Listeners.OfType<DelegateTraceListener>().Single();

      lock (_lockObject)
      {
        SwapTraceListeners(_previousTraceListener, traceListener);
        ConfigureConsole(_previousStdOut, _previousErrOut);
      }
    }

    private void ConfigureConsole (TextWriter stdOut, TextWriter errOut)
    {
      Console.SetOut(TextWriter.Synchronized(stdOut));
      Console.SetError(TextWriter.Synchronized(errOut));
    }

    private void SwapTraceListeners ([CanBeNull] TraceListener newListener, [CanBeNull] TraceListener oldListener)
    {
      if (oldListener != null)
        Trace.Listeners.Remove(oldListener);
      if (newListener != null)
        Trace.Listeners.Add(newListener);
    }

    private void Write (OutputType type, string message)
    {
      lock (_lockObject)
      {
        _entries.Add(new OutputEntry { Type = type, Message = message });
      }
    }

    public IEnumerator<OutputEntry> GetEnumerator ()
    {
      lock (_lockObject)
      {
        return _entries.GetEnumerator();
      }
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
      return GetEnumerator();
    }
  }
}