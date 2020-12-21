using LLkGrammarChecker;
using LLkGrammarChecker.Logging;
using LLkGrammarChecker.Logic;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LLkGrammarCheckerTests
{
    public class LLkCheckerTests
    {
        private LLkFunctionsLogic functions;
        private LLkCheckerLogic checker;

        private static Nonterminal S = new Nonterminal("S");
        private static Nonterminal A = new Nonterminal("A");
        private static Nonterminal B = new Nonterminal("B");

        private static Nonterminal E = new Nonterminal("E");
        private static Nonterminal T = new Nonterminal("T");
        private static Nonterminal F = new Nonterminal("F");
        private static Nonterminal Ep = new Nonterminal("E'");
        private static Nonterminal Tp = new Nonterminal("T'");

        private static Terminal a = new Terminal("a");
        private static Terminal b = new Terminal("b");

        private static Terminal plus = new Terminal("+");
        private static Terminal ast = new Terminal("*");
        private static Terminal lBracket = new Terminal("(");
        private static Terminal rBracket = new Terminal(")");
        private static Terminal id = new Terminal("id");

        private static Cfg grammar1 = new Cfg(S)
            .AddNonterminal(A)
            .AddNonterminal(B)
            .AddTerminal(a)
            .AddTerminal(b)
            .AddProduction(S, a + B)
            .AddProduction(S, b + A)
            .AddProduction(A, a)
            .AddProduction(B, b)
            .AddProduction(A, b + A + A)
            .AddProduction(B, a + B + B)
            .AddProduction(A, a + S)
            .AddProduction(B, b + S) as Cfg;

        private static Cfg grammar2 = new Cfg(S)
            .AddNonterminal(A)
            .AddNonterminal(B)
            .AddTerminal(a)
            .AddTerminal(b)
            .AddProduction(S, a + A + a + B)
            .AddProduction(S, b + A + b + B)
            .AddProduction(A, a)
            .AddProduction(A, a + b)
            .AddProduction(B, a)
            .AddProduction(B, a + B) as Cfg;

        private static Cfg grammar3 = new Cfg(E)
            .AddNonterminal(E)
            .AddNonterminal(Ep)
            .AddNonterminal(T)
            .AddNonterminal(Tp)
            .AddNonterminal(F)
            .AddTerminal(plus)
            .AddTerminal(ast)
            .AddTerminal(lBracket)
            .AddTerminal(rBracket)
            .AddTerminal(id)
            .AddProduction(E, T + Ep)
            .AddProduction(Ep, plus + T + Ep)
            .AddProduction(Ep, SententialForm.Epsilon)
            .AddProduction(T, F + Tp)
            .AddProduction(Tp, ast + F + Tp)
            .AddProduction(Tp, SententialForm.Epsilon)
            .AddProduction(F, lBracket + E + rBracket)
            .AddProduction(F, id) as Cfg;

        public LLkCheckerTests()
        {
            var mockLogger = new Mock<ILogger>();
            functions = new LLkFunctionsLogic(mockLogger.Object);
            checker = new LLkCheckerLogic(mockLogger.Object, functions);
        }

        [Theory]
        [MemberData(nameof(CheckTestCases))]
        public void CheckTest(Cfg grammar, int dimension, bool expected)
        {
            var actual = checker.Check(grammar, dimension);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> CheckTestCases =>
            new List<object[]>()
            {
                new object[]
                {
                    grammar1,
                    3,
                    false
                },
                new object[]
                {
                    grammar1,
                    2,
                    false
                },
                new object[]
                {
                    grammar1,
                    1,
                    false
                },
                new object[]
                {
                    grammar2,
                    4,
                    true
                },
                new object[]
                {
                    grammar2,
                    3,
                    true
                },
                new object[]
                {
                    grammar2,
                    2,
                    false
                },
                new object[]
                {
                    grammar2,
                    1,
                    false
                },
                new object[]
                {
                    grammar3,
                    3,
                    true
                },
                new object[]
                {
                    grammar3,
                    2,
                    true
                },
                new object[]
                {
                    grammar3,
                    1,
                    true
                }
            };
    }
}