using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Interfaces
{
    public interface ILLkFunctions
    {
        public HashSet<SententialForm> First(Cfg grammar, SententialForm argument, int dimension = 1);

        public HashSet<HashSet<SententialForm>> Sigma(Cfg grammar, Nonterminal argument, int dimension = 1);

        public HashSet<SententialForm> Follow(Cfg grammar, Nonterminal argument, int dimension = 1);

        public HashSet<SententialForm> TerminalDirectSum(HashSet<SententialForm> left,
            HashSet<SententialForm> right, int dimension = 1);
    }
}
