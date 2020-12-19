using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LLkGrammarChecker.Extensions;
using LLkGrammarChecker.Logging;
using LLkGrammarChecker.Interfaces;

namespace LLkGrammarChecker
{
    public class LLkCheckerLogic
    {
        private ILogger logger;
        private ILLkFunctions functions;

        public LLkCheckerLogic(ILogger logger, ILLkFunctions functions)
        {
            this.logger = logger;
            this.functions = functions;
        }
    }
}
