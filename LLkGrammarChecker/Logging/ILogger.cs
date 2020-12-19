using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Logging
{
    public interface ILogger
    {
        void I(string message);
        void E(string message, Exception exception = null);
    }
}
