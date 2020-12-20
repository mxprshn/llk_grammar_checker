using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Interfaces
{
    interface ILLkChecker
    {
        public bool Check(CFG grammar, int dimension = 1);
    }
}