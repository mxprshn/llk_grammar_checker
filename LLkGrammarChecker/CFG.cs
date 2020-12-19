﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker
{
    public class CFG : Grammar
    {
        public CFG(Nonterminal startSymbol) : base(startSymbol) { }

        public override void AddProduction(Sententia left, Sententia right)
        {
            if (!left.IsNonterminal)
            {
                throw new ArgumentException("CFG must contain only producions with one nonterminal in the left part.");
            }

            base.AddProduction(left, right);
        }
    }
}