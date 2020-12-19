using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Interfaces
{
    public interface ILLkFunctions
    {
        public HashSet<Sententia> First(CFG grammar, Sententia argument, int dimension = 1);

        public HashSet<HashSet<Sententia>> Sigma(CFG grammar, Nonterminal argument, int dimension = 1);

        public HashSet<Sententia> Follow(CFG grammar, Nonterminal argument, int dimension = 1);
    }
}
