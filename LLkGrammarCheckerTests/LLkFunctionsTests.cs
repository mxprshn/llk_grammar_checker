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

            var E = new Nonterminal("E");
            var T = new Nonterminal("T");
            var F = new Nonterminal("F");
            var Ep = new Nonterminal("E'");
            var Tp = new Nonterminal("T'");

            var a = new Terminal("a");
            var b = new Terminal("b");

            var plus = new Terminal("+");
            var ast = new Terminal("*");
            var lBracket = new Terminal("(");
            var rBracket = new Terminal(")");
            var id = new Terminal("id");

            var grammar1 = new CFG(S)
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

            var grammar2 = new CFG(E)
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
                .AddProduction(Ep, Sententia.Epsilon)
                .AddProduction(T, F + Tp)
                .AddProduction(Tp, ast + F + Tp)
                .AddProduction(Tp, Sententia.Epsilon)
                .AddProduction(F, lBracket + E + rBracket)
                .AddProduction(F, id);

            return new List<object[]>()
            {
                new object[]
                {
                    grammar1,
                    new Sententia(S),
                    3,
                    new HashSet<Sententia>()
                    {
                        b + a,
                        a + b,
                        a + a + b,
                        a + a + a,
                        a + b + a,
                        a + b + b,
                        b + b + a,
                        b + b + b,
                        b + a + a,
                        b + a + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(S),
                    2,
                    new HashSet<Sententia>()
                    {
                        b + a,
                        a + b,
                        a + a,
                        b + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(S),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(b),
                        new Sententia(a)
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(A),
                    3,
                    new HashSet<Sententia>()
                    {
                        new Sententia(a),
                        a + a + b,
                        a + a + a,
                        a + b + a,
                        a + b + b,
                        b + b + a,
                        b + b + b,
                        b + a + a,
                        b + a + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(A),
                    2,
                    new HashSet<Sententia>()
                    {
                        new Sententia(a),
                        b + a,
                        a + b,
                        a + a,
                        b + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(A),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(b),
                        new Sententia(a)
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(B),
                    3,
                    new HashSet<Sententia>()
                    {
                        new Sententia(b),
                        a + a + b,
                        a + a + a,
                        a + b + a,
                        a + b + b,
                        b + b + a,
                        b + b + b,
                        b + a + a,
                        b + a + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(B),
                    2,
                    new HashSet<Sententia>()
                    {
                        new Sententia(b),
                        b + a,
                        a + b,
                        a + a,
                        b + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new Sententia(B),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(b),
                        new Sententia(a)
                    }
                },
                new object[]
                {
                    grammar2,
                    new Sententia(E),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(lBracket),
                        new Sententia(id)
                    }
                },
                new object[]
                {
                    grammar2,
                    new Sententia(T),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(lBracket),
                        new Sententia(id)
                    }
                },
                new object[]
                {
                    grammar2,
                    new Sententia(F),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(lBracket),
                        new Sententia(id)
                    }
                },
                new object[]
                {
                    grammar2,
                    new Sententia(Ep),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(plus),
                        Sententia.Epsilon
                    }
                },
                new object[]
                {
                    grammar2,
                    new Sententia(Tp),
                    1,
                    new HashSet<Sententia>()
                    {
                        new Sententia(ast),
                        Sententia.Epsilon
                    }
                }
            };
        }
    }
}
