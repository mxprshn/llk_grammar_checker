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

        private static CFG grammar1 = new CFG(S)
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
            .AddProduction(B, b + S) as CFG;

        private static CFG grammar2 = new CFG(E)
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
            .AddProduction(F, id) as CFG;

        private static CFG grammar3 = new CFG(S)
            .AddNonterminal(A)
            .AddNonterminal(B)
            .AddTerminal(a)
            .AddTerminal(b)
            .AddProduction(S, a + A + a + a)
            .AddProduction(S, b + A + b + a)
            .AddProduction(A, b)
            .AddProduction(A, SententialForm.Epsilon) as CFG;

        private static CFG grammar4 = new CFG(S)
            .AddNonterminal(A)
            .AddTerminal(a)
            .AddTerminal(b)
            .AddProduction(S, A + S)
            .AddProduction(S, SententialForm.Epsilon)
            .AddProduction(A, a + A)
            .AddProduction(A, b) as CFG;

        public LLkFunctionsTests()
        {
            var mockLogger = new Mock<ILogger>();
            functions = new LLkFunctionsLogic(mockLogger.Object);
        }

        [Theory]
        [MemberData(nameof(FirstTestCases))]
        public void FirstTest(CFG grammar, SententialForm argument, int dimension, HashSet<SententialForm> expected)
        {
            var actual = functions.First(grammar, argument, dimension);
            Assert.True(expected.SetEquals(actual));
        }

        [Theory]
        [MemberData(nameof(FollowTestCases))]
        public void FollowTest(CFG grammar, Nonterminal argument, int dimension, HashSet<SententialForm> expected)
        {
            var actual = functions.Follow(grammar, argument, dimension);
            Assert.True(expected.SetEquals(actual));
        }

        [Theory]
        [MemberData(nameof(SigmaTestCases))]
        public void SigmaTest(CFG grammar, Nonterminal argument, int dimension, HashSet<HashSet<SententialForm>> expected)
        {
            var actual = functions.Sigma(grammar, argument, dimension);
            Assert.True(expected.SetEquals(actual));
        }

        public static IEnumerable<object[]> FirstTestCases =>
            new List<object[]>()
            {
                new object[]
                {
                    grammar1,
                    new SententialForm(S),
                    3,
                    new HashSet<SententialForm>()
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
                    new SententialForm(S),
                    2,
                    new HashSet<SententialForm>()
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
                    new SententialForm(S),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(b),
                        new SententialForm(a)
                    }
                },
                new object[]
                {
                    grammar1,
                    new SententialForm(A),
                    3,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(a),
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
                    new SententialForm(A),
                    2,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(a),
                        b + a,
                        a + b,
                        a + a,
                        b + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new SententialForm(A),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(b),
                        new SententialForm(a)
                    }
                },
                new object[]
                {
                    grammar1,
                    new SententialForm(B),
                    3,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(b),
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
                    new SententialForm(B),
                    2,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(b),
                        b + a,
                        a + b,
                        a + a,
                        b + b
                    }
                },
                new object[]
                {
                    grammar1,
                    new SententialForm(B),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(b),
                        new SententialForm(a)
                    }
                },
                new object[]
                {
                    grammar2,
                    new SententialForm(E),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(lBracket),
                        new SententialForm(id)
                    }
                },
                new object[]
                {
                    grammar2,
                    new SententialForm(T),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(lBracket),
                        new SententialForm(id)
                    }
                },
                new object[]
                {
                    grammar2,
                    new SententialForm(F),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(lBracket),
                        new SententialForm(id)
                    }
                },
                new object[]
                {
                    grammar2,
                    new SententialForm(Ep),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(plus),
                        SententialForm.Epsilon
                    }
                },
                new object[]
                {
                    grammar2,
                    new SententialForm(Tp),
                    1,
                    new HashSet<SententialForm>()
                    {
                        new SententialForm(ast),
                        SententialForm.Epsilon
                    }
                },
                new object[]
                {
                    grammar3,
                    b + a + a,
                    2,
                    new HashSet<SententialForm>()
                    {
                        b + a
                    }
                },
                new object[]
                {
                    grammar3,
                    a + a,
                    2,
                    new HashSet<SententialForm>()
                    {
                        a + a
                    }
                },
                new object[]
                {
                    grammar3,
                    b + b + a,
                    2,
                    new HashSet<SententialForm>()
                    {
                        b + b
                    }
                }
            };

        public static IEnumerable<object[]> FollowTestCases =>
            new List<object[]>()
            {
                new object[]
                {
                    grammar1,
                    B,
                    3,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(a),
                        new SententialForm(b),
                        a + a,
                        b + b,
                        a + b,
                        b + a,
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
                    A,
                    3,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(a),
                        new SententialForm(b),
                        a + a,
                        b + b,
                        a + b,
                        b + a,
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
                    A,
                    2,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(a),
                        new SententialForm(b),
                        a + a,
                        b + b,
                        a + b,
                        b + a
                    }
                },
                new object[]
                {
                    grammar1,
                    B,
                    2,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(a),
                        new SententialForm(b),
                        a + a,
                        b + b,
                        a + b,
                        b + a
                    }
                },
                new object[]
                {
                    grammar1,
                    B,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(b),
                        new SententialForm(a)
                    }
                },
                new object[]
                {
                    grammar1,
                    A,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(b),
                        new SententialForm(a)
                    }
                },
                new object[]
                {
                    grammar1,
                    S,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(b),
                        new SententialForm(a)
                    }
                },
                new object[]
                {
                    grammar2,
                    E,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(rBracket),
                    }
                },
                new object[]
                {
                    grammar2,
                    Ep,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(rBracket),
                    }
                },
                new object[]
                {
                    grammar2,
                    T,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(rBracket),
                        new SententialForm(plus)
                    }
                },
                new object[]
                {
                    grammar2,
                    Tp,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(rBracket),
                        new SententialForm(plus)
                    }
                },
                new object[]
                {
                    grammar2,
                    F,
                    1,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon,
                        new SententialForm(rBracket),
                        new SententialForm(plus),
                        new SententialForm(ast)
                    }
                },
                new object[]
                {
                    grammar3,
                    A,
                    2,
                    new HashSet<SententialForm>()
                    {
                        a + a,
                        b + a
                    }
                },
                new object[]
                {
                    grammar3,
                    S,
                    2,
                    new HashSet<SententialForm>()
                    {
                        SententialForm.Epsilon
                    }
                }
            };

        public static IEnumerable<object[]> SigmaTestCases =>
            new List<object[]>()
            {
                new object[]
                {
                    grammar4,
                    S,
                    1,
                    new HashSet<HashSet<SententialForm>>(HashSet<SententialForm>.CreateSetComparer())
                    {
                        new HashSet<SententialForm>()
                        {
                            SententialForm.Epsilon
                        }
                    }
                },
                new object[]
                {
                    grammar4,
                    A,
                    1,
                    new HashSet<HashSet<SententialForm>>(HashSet<SententialForm>.CreateSetComparer())
                    {
                        new HashSet<SententialForm>()
                        {
                            SententialForm.Epsilon,
                            new SententialForm(a),
                            new SententialForm(b)
                        }
                    }
                }
            };
    }
}