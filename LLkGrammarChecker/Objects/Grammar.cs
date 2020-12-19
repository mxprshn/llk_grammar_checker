using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LLkGrammarChecker
{
    public class Grammar
    {
        public Grammar(Nonterminal startSymbol)
        {
            StartSymbol = startSymbol;
            AddNonterminal(startSymbol);
        }

        private HashSet<Nonterminal> nonterminals = new HashSet<Nonterminal>();
        public IReadOnlyCollection<Nonterminal> Nonterminals => nonterminals;

        private HashSet<Terminal> terminals = new HashSet<Terminal>();
        public IReadOnlyCollection<Terminal> Terminals => terminals;

        private HashSet<(Sententia left, Sententia right)> productions = new HashSet<(Sententia left, Sententia right)>();
        public IReadOnlyCollection<(Sententia left, Sententia right)> Productions => productions;

        public Nonterminal StartSymbol { get; private set; }

        public Grammar AddNonterminal(Nonterminal nonterminal)
        {
            nonterminals.Add(nonterminal);
            return this;
        }

        public Grammar AddTerminal(Terminal terminal)
        {
            terminals.Add(terminal);
            return this;
        }

        public virtual Grammar AddProduction(Sententia left, Sententia right)
        {
            if (productions.Any(p => p.left == left && p.right == right))
            {
                return this;
            }

            productions.Add((left, right));

            return this;
        }

        public Grammar AddProduction(Nonterminal left, Sententia right)
        {
            return AddProduction(new Sententia(left), right);
        }

        public Grammar AddProduction(Nonterminal left, GrammarSymbol right)
        {
            return AddProduction(new Sententia(left), new Sententia(right));
        }

        public Grammar AddProduction(Sententia left, GrammarSymbol right)
        {
            return AddProduction(new Sententia(left), new Sententia(right));
        }
    }
}