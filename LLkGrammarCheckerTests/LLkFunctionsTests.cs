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
    public class LLkFunctionsTests
    {
        private LLkFunctionsLogic functions;

        public LLkFunctionsTests()
        {
            var mockLogger = new Mock<ILogger>();
            functions = new LLkFunctionsLogic(mockLogger.Object);
        }

        [Theory]
        [MemberData(nameof(FirstTestCases))]
        public void FirstTest(CFG grammar, Sententia argument, int dimension, HashSet<Sententia> expected)
        {
            var actual = functions.First(grammar, argument, dimension);
            Assert.True(expected.SetEquals(actual));
        }

        public static IEnumerable<object[]> FirstTestCases()
        {
            var S = new Nonterminal("S");
            var A = new Nonterminal("A");
            var B = new Nonterminal("B");

            var a = new Terminal("a");
            var b = new Terminal("b");

            var grammar = new CFG(S)
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
                .AddProduction(B, b + S);

            var expectedForS = new HashSet<Sententia>()
            {
                a + b,
                a + a + b,
                a + a + a,
                a + b + a,
                a + b + b,
                b + b + a,
                b + b + b,
                b + a + a,
                b + a + b
            };

            return new List<object[]>()
            {
                new object[]
                {
                    grammar,
                    new Sententia(S),
                    3,
                    expectedForS
                }
            };
        }
    }
}
