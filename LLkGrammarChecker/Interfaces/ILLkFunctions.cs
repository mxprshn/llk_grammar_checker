using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Interfaces
{
    public interface ILLkFunctions
    {
        public HashSet<SententialForm> First(CFG grammar, SententialForm argument, int dimension = 1);

        public HashSet<HashSet<SententialForm>> Sigma(CFG grammar, Nonterminal argument, int dimension = 1);

        public HashSet<SententialForm> Follow(CFG grammar, Nonterminal argument, int dimension = 1);

        public HashSet<SententialForm> TerminalDirectSum(HashSet<SententialForm> left,
            HashSet<SententialForm> right, int dimension = 1);
    }
}
