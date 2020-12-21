using LLkGrammarChecker.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarCheckerConsole
{
    class ConsoleLogger : ILogger
    {
        public void E(string message, Exception exception = null)
        {
            Console.WriteLine($"ERROR | {message} | {exception.Message}");
        }

        public void I(string message)
        {
            Console.WriteLine(message);
        }
    }
}
