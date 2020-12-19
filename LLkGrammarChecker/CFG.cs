using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker
{
    public class CFG : Grammar
    {
        public override void AddProduction(Sententia left, Sententia right)
        {
            if (!left.IsNonterminal)
            {
                throw new ArgumentException("CFG must contain only producions with one nonterminal in the left part.");
            }

            base.AddProduction(left, right);
        }



        public HashSet<Sententia> Follow(Sententia argument, int dimension = 1)
        {
            if (dimension <= 0)
            {
                throw new ArgumentException("Dimension must be a positive number.");
            }

            return null;
        }
    }
}
