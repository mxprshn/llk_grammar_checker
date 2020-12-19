using LLkGrammarChecker.Extensions;
using LLkGrammarChecker.Interfaces;
using LLkGrammarChecker.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLkGrammarChecker.Logic
{
    public class LLkFunctionsLogic : ILLkFunctions
    {
        private ILogger logger;

        public LLkFunctionsLogic(ILogger logger)
        {
            this.logger = logger;
        }

        public HashSet<Sententia> First(CFG grammar, Sententia argument, int dimension = 1)
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
                approximations[terminal] = new HashSet<Sententia> { new Sententia(terminal) };
            }

            foreach (var nonterminal in grammar.Nonterminals)
            {
                approximations[nonterminal] = new HashSet<Sententia>();

                foreach (var production in grammar.Productions.Where(p => p.left == nonterminal))
                {
                    var trimmed = production.right.Trim(dimension);

                    if (!trimmed.ContainsNonterminals)
                    {
                        approximations[nonterminal].Add(trimmed);
                    }
                }
            }

            while (!prevApproximations.DeepEquals(approximations))
            {
                prevApproximations = approximations.DeepCopy();

                foreach (var nonterminal in grammar.Nonterminals)
                {
                    foreach (var production in grammar.Productions.Where(p => p.left == nonterminal))
                    {
                        var directSumOfRights = new HashSet<Sententia>(prevApproximations[production.left.First()]);

                        for (var i = 1; i < production.right.Length; ++i)
                        {
                            directSumOfRights = TerminalDirectSum(directSumOfRights,
                                prevApproximations[production.right[i]], dimension);
                        }

                        approximations[nonterminal].UnionWith(directSumOfRights);
                    }
                }
            }

            var result = new HashSet<Sententia>(approximations[argument.First()]);

            for (var i = 1; i < argument.Length; ++i)
            {
                result = TerminalDirectSum(result, approximations[argument[i]], dimension);
            }

            return result;
        }

        public HashSet<HashSet<Sententia>> Sigma(CFG grammar, Nonterminal argument, int dimension = 1)
        {
            if (dimension <= 0)
            {
                throw new ArgumentException("Dimension must be a positive number.");
            }

            if (!grammar.Nonterminals.Contains(argument))
            {
                throw new ArgumentException($"Nonterminal {argument} is not from grammar.");
            }

            var firstCash = new Dictionary<Sententia, HashSet<Sententia>>();

            var prevApproximations = new Dictionary<(Nonterminal, Nonterminal), HashSet<HashSet<Sententia>>>();
            var approximations = new Dictionary<(Nonterminal, Nonterminal), HashSet<HashSet<Sententia>>>();

            foreach (var leftNonterminal in grammar.Nonterminals)
            {
                foreach (var rightNonterminal in grammar.Nonterminals)
                {
                    foreach (var production in grammar.Productions.Where(p => p.left == leftNonterminal))
                    {
                        for (var i = 0; i < production.right.Length; ++i)
                        {
                            if (production.right[i] == rightNonterminal)
                            {
                                var tail = production.right.TakeLast(production.right.Length - i + 1).ToSententia();
                                var tailFirst = firstCash.GetValueOrDefault(tail) ?? First(grammar, tail, dimension);
                                firstCash.TryAdd(tail, tailFirst);

                                if (approximations.GetValueOrDefault((leftNonterminal, rightNonterminal)) == null)
                                {
                                    approximations[(leftNonterminal, rightNonterminal)] = new HashSet<HashSet<Sententia>>(HashSet<Sententia>.CreateSetComparer());
                                }

                                approximations[(leftNonterminal, rightNonterminal)].Add(tailFirst);
                            }
                        }
                    }
                }
            }

            while (!prevApproximations.DeepEquals(approximations))
            {
                prevApproximations = approximations.DeepCopy();

                foreach (var leftNonterminal in grammar.Nonterminals)
                {
                    foreach (var rightNonterminal in grammar.Nonterminals)
                    {
                        foreach (var production in grammar.Productions.Where(p => p.left == leftNonterminal))
                        {
                            for (var i = 0; i < production.right.Length; ++i)
                            {
                                if (!(production.right[i] is Nonterminal)) continue;

                                foreach (var approximation in prevApproximations[(production.right[i] as Nonterminal, rightNonterminal)])
                                {
                                    var tail = production.right.TakeLast(production.right.Length - i + 1).ToSententia();
                                    var tailFirst = firstCash.GetValueOrDefault(tail) ?? First(grammar, tail, dimension);
                                    firstCash.TryAdd(tail, tailFirst);

                                    var directSum = TerminalDirectSum(approximation, tailFirst, dimension);

                                    approximations[(leftNonterminal, rightNonterminal)].Add(directSum);
                                }
                            }
                        }
                    }
                }
            }

            return approximations[(grammar.StartSymbol, argument)];
        }

        public HashSet<Sententia> Follow(CFG grammar, Nonterminal argument, int dimension = 1)
        {
            if (dimension <= 0)
            {
                throw new ArgumentException("Dimension must be a positive number.");
            }

            if (!grammar.Nonterminals.Contains(argument))
            {
                throw new ArgumentException($"Nonterminal {argument} is not from grammar.");
            }

            var firstCash = new Dictionary<Sententia, HashSet<Sententia>>();

            var prevApproximations = new Dictionary<(Nonterminal, Nonterminal), HashSet<Sententia>>();
            var approximations = new Dictionary<(Nonterminal, Nonterminal), HashSet<Sententia>>();

            foreach (var leftNonterminal in grammar.Nonterminals)
            {
                foreach (var rightNonterminal in grammar.Nonterminals)
                {
                    foreach (var production in grammar.Productions.Where(p => p.left == leftNonterminal))
                    {
                        for (var i = 0; i < production.right.Length; ++i)
                        {
                            if (production.right[i] == rightNonterminal)
                            {
                                var tail = production.right.TakeLast(production.right.Length - i + 1).ToSententia();
                                var tailFirst = firstCash.GetValueOrDefault(tail) ?? First(grammar, tail, dimension);
                                firstCash.TryAdd(tail, tailFirst);

                                if (approximations.GetValueOrDefault((leftNonterminal, rightNonterminal)) == null)
                                {
                                    approximations[(leftNonterminal, rightNonterminal)] = new HashSet<Sententia>();
                                }

                                approximations[(leftNonterminal, rightNonterminal)].UnionWith(tailFirst);
                            }
                        }
                    }
                }
            }

            while (!prevApproximations.DeepEquals(approximations))
            {
                prevApproximations = approximations.DeepCopy();

                foreach (var leftNonterminal in grammar.Nonterminals)
                {
                    foreach (var rightNonterminal in grammar.Nonterminals)
                    {
                        foreach (var production in grammar.Productions.Where(p => p.left == leftNonterminal))
                        {
                            for (var i = 0; i < production.right.Length; ++i)
                            {
                                if (!(production.right[i] is Nonterminal)) continue;

                                var tail = production.right.TakeLast(production.right.Length - i + 1).ToSententia();
                                var tailFirst = firstCash.GetValueOrDefault(tail) ?? First(grammar, tail, dimension);
                                firstCash.TryAdd(tail, tailFirst);

                                var directSum = TerminalDirectSum(prevApproximations[(production.right[i] as Nonterminal, rightNonterminal)], tailFirst, dimension);

                                approximations[(leftNonterminal, rightNonterminal)].UnionWith(directSum);
                            }
                        }
                    }
                }
            }

            return approximations[(grammar.StartSymbol, argument)];
        }

        private HashSet<Sententia> TerminalDirectSum(HashSet<Sententia> left,
            HashSet<Sententia> right, int dimension = 1)
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
    }
}
