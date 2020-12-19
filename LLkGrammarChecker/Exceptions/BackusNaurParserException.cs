using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Exceptions
{
    [Serializable]
    public class BackusNaurParserException : Exception
    {
        public BackusNaurParserException() { }
        public BackusNaurParserException(string message) : base(message) { }
        public BackusNaurParserException(string message, Exception inner) : base(message, inner) { }
        protected BackusNaurParserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
