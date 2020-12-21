using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker
{
    public class Cfg : Grammar
    {
        public Cfg(Nonterminal startSymbol) : base(startSymbol) { }

        public override Grammar AddProduction(SententialForm left, SententialForm right)
        {
            if (!left.IsNonterminal)
            {
                throw new ArgumentException("CFG must contain only producions with one nonterminal in the left part.");
            }

            return base.AddProduction(left, right);
        }
    }
}