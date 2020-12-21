using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Interfaces
{
    public interface ILLkChecker
    {
        public bool Check(Cfg grammar, int dimension = 1);
    }
}