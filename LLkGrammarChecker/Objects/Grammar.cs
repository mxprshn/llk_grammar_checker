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

        private HashSet<(SententialForm left, SententialForm right)> productions = new HashSet<(SententialForm left, SententialForm right)>();
        public IReadOnlyCollection<(SententialForm left, SententialForm right)> Productions => productions;

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

        public virtual Grammar AddProduction(SententialForm left, SententialForm right)
        {
            if (productions.Any(p => p.left == left && p.right == right))
            {
                return this;
            }

            productions.Add((left, right));

            return this;
        }

        public Grammar AddProduction(Nonterminal left, SententialForm right)
        {
            return AddProduction(new SententialForm(left), right);
        }

        public Grammar AddProduction(Nonterminal left, GrammarSymbol right)
        {
            return AddProduction(new SententialForm(left), new SententialForm(right));
        }

        public Grammar AddProduction(SententialForm left, GrammarSymbol right)
        {
            return AddProduction(new SententialForm(left), new SententialForm(right));
        }
    }
}