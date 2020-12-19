using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LLkGrammarChecker
{
    public static class Checker
    {
        public static HashSet<Sententia> First(CFG grammar, Sententia argument, int dimension = 1)
        {
            if (dimension <= 0)
            {
                throw new ArgumentException("Dimension must be a positive number.");
            }

            foreach (var symbol in argument)
            {
                if (symbol is Terminal terminal && !grammar.Terminals.Contains(terminal))
                {
                    throw new ArgumentException($"Terminal {terminal} in sententia is not from grammar.");
                }

                if (symbol is Nonterminal nonterminal && !grammar.Nonterminals.Contains(nonterminal))
                {
                    throw new ArgumentException($"Nonterminal {nonterminal} in sententia is not from grammar.");
                }
            }

            var approximations = new Dictionary<GrammarSymbol, HashSet<Sententia>>();
            var prevApproximations = new Dictionary<GrammarSymbol, HashSet<Sententia>>();

            foreach (var terminal in grammar.Terminals)
            {
                prevApproximations[terminal] = new HashSet<Sententia> { new Sententia(terminal) };
            }

            foreach (var nonterminal in grammar.Nonterminals)
            {
                prevApproximations[nonterminal] = new HashSet<Sententia>();

                foreach (var production in grammar.Productions.Where(p => p.left == nonterminal))
                {
                    var trimmed = production.right.Trim(dimension);

                    if (!trimmed.ContainsNonterminals)
                    {
                        prevApproximations[nonterminal].Add(trimmed);
                    }
                }
            }
        }

        private static ICollection<Sententia> TerminalDirectSum(ICollection<Sententia> left,
            ICollection<Sententia> right, int dimension = 1)
        {
            var result = new HashSet<Sententia>();

            foreach (var leftSententia in left)
            {
                foreach (var rightSententia in right)
                {
                    if (leftSententia.ContainsNonterminals || rightSententia.ContainsNonterminals)
                    {
                        throw new InvalidOperationException("Left or right sententia in direct sum contains nonterminal.");
                    }

                    var concatenated = leftSententia + rightSententia;
                    concatenated = concatenated.Trim(dimension);

                    result.Add(concatenated);
                }
            }

            return result;
        }

        private static bool AreApproximationsEqual(Dictionary<GrammarSymbol, HashSet<Sententia>> one,
            Dictionary<GrammarSymbol, HashSet<Sententia>> another)
        {
            if (one.Count != another.Count)
            {
                return false;
            }

            foreach (var (symbolsInOne, valuesInOne) in one)
            {
                var valuesInAnother = another.GetValueOrDefault(symbolsInOne);

                if (valuesInAnother == null || !valuesInOne.SetEquals(valuesInAnother))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
