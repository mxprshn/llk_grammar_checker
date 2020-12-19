using LLkGrammarChecker;
using System;
using Xunit;

namespace LLkGrammarCheckerTests
{
    public class SymbolTests
    {
        [Fact]
        public void NonterminalAndTerminalWithSimilarLiteralsAreNotEqual()
        {
            var nonterminal = new Nonterminal("ololo");
            var terminal = new Terminal("ololo");
            var equal = nonterminal == terminal;
            Assert.False(equal);
        }

        [Fact]
        public void NonterminalsWithSimilarLiteralsAreEqual()
        {
            var nonterminal1 = new Nonterminal("ololo");
            var nonterminal2 = new Nonterminal("ololo");
            var equal = nonterminal1 == nonterminal2;
            Assert.True(equal);
        }

        [Fact]
        public void TerminalsWithSimilarLiteralsAreEqual()
        {
            var terminal1 = new Terminal("ololo");
            var terminal2 = new Terminal("ololo");
            var equal = terminal1 == terminal2;
            Assert.True(equal);
        }
    }
}