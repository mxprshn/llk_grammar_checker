using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker
{
    public class Nonterminal : GrammarSymbol
    {
        public Nonterminal(string literal) : base(literal) { }
    }
}