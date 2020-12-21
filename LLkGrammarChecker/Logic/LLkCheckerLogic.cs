using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LLkGrammarChecker.Extensions;
using LLkGrammarChecker.Logging;
using LLkGrammarChecker.Interfaces;

namespace LLkGrammarChecker
{
    public class LLkCheckerLogic : ILLkChecker
    {
        private ILogger logger;
        private ILLkFunctions functions;

        public LLkCheckerLogic(ILogger logger, ILLkFunctions functions)
        {
            this.logger = logger;
            this.functions = functions;
        }

        public bool Check(Cfg grammar, int dimension = 1)
        {
            if (dimension <= 0)
            {
                throw new ArgumentException("Dimension must be a positive number.");
            }

            var nonterminalsWithMultipleProductions = grammar.Nonterminals
                .Where(nt => grammar.Productions.Select(p => p.left == nt).Count() > 1);

            foreach (var nonterminal in nonterminalsWithMultipleProductions)
            {
                var sigma = functions.Sigma(grammar, nonterminal, dimension);
                var productionsWithNonterminal = grammar.Productions.Where(p => p.left == nonterminal).ToArray();

                foreach (var sigmaSet in sigma)
                {
                    for (var i = 0; i < productionsWithNonterminal.Length - 1; ++i)
                    {
                        for (var j = i + 1; j < productionsWithNonterminal.Length; ++j)
                        {
                            var production1 = productionsWithNonterminal[i];
                            var production2 = productionsWithNonterminal[j];

                            var intersection = functions.TerminalDirectSum(functions.First(grammar, production1.right, dimension), sigmaSet, dimension)
                                .Intersect(functions.TerminalDirectSum(functions.First(grammar, production2.right, dimension), sigmaSet, dimension));

                            if (intersection.Count() > 0)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}