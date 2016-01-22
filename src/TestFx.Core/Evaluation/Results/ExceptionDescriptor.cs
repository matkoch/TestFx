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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TestFx.Utilities.Collections;

namespace TestFx.Evaluation.Results
{
  public interface IExceptionDescriptor
  {
    string Name { get; }
    string FullName { get; }
    string Message { get; }
    string StackTrace { get; }
  }

  [Serializable]
  [DebuggerDisplay("Name = {Name}, Message = {Message}")]
  public class ExceptionDescriptor : IExceptionDescriptor
  {
    public const ExceptionDescriptor None = null;

    private static readonly Regex s_isFrameworkCode = new Regex(@"^\s+\w+\sTestFx", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static ExceptionDescriptor Create (Exception exception)
    {
      var messageBuilder = new StringBuilder();
      var stackTraceBuilder = new StringBuilder();

      foreach (var ex in exception.DescendantsAndSelf(x => x.InnerException))
      {
        if (ex != exception)
        {
          messageBuilder.Append(Environment.NewLine).Append("---> ");
          stackTraceBuilder.Append(Environment.NewLine).Append("--- Begin of inner exception stack trace ---").Append(Environment.NewLine);
        }

        messageBuilder.Append(ex.Message);

        var stackTrace = ex.StackTrace ?? string.Empty;
        var stackTraceLines = stackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        var userCodeStackTrace = stackTraceLines.Where(IsUserCode);
        userCodeStackTrace.ForEach(x => stackTraceBuilder.Append(x).Append(Environment.NewLine));
      }

      var type = exception.GetType();
      return new ExceptionDescriptor(type.Name, type.FullName, messageBuilder.ToString(), stackTraceBuilder.ToString());
    }

    private static bool IsUserCode (string line)
    {
      return !s_isFrameworkCode.IsMatch(line);
    }

    private readonly string _name;
    private readonly string _fullName;
    private readonly string _message;
    private readonly string _stackTrace;

    private ExceptionDescriptor (string name, string fullName, string message, string stackTrace)
    {
      _name = name;
      _fullName = fullName;
      _message = message;
      _stackTrace = stackTrace;
    }

    public string Name
    {
      get { return _name; }
    }

    public string FullName
    {
      get { return _fullName; }
    }

    public string Message
    {
      get { return _message; }
    }

    public string StackTrace
    {
      get { return _stackTrace; }
    }
  }
}