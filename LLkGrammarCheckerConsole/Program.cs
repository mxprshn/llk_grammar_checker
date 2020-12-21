using LLkGrammarChecker;
using LLkGrammarChecker.Exceptions;
using LLkGrammarChecker.Interfaces;
using LLkGrammarChecker.Logic;
using Pastel;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LLkGrammarCheckerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            var parser = new BackusNaurParserLogic();
            var logger = new ConsoleLogger();
            var functions = new LLkFunctionsLogic(logger);
            var checker = new LLkCheckerLogic(logger, functions);

            if (args.Length == 0)
            {
                Console.WriteLine("Grammar file path not specified.");
                return;
            }

            if (args.Length > 1)
            {
                Console.WriteLine("Too many command line arguments.");
                return;
            }

            Cfg grammar = null;
            
            try
            {
                grammar = parser.FromFile(args[0]);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {args[0]}.");
                return;
            }
            catch (BackusNaurParserException e)
            {
                Console.WriteLine($"Parsing error. {e.Message}");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected exception. {e.Message}");
                return;
            }

            PrintGrammar(grammar);

            Console.WriteLine("Commands:\n" +
                "\tfirst k --- calculate FIRST_k set for sentential form\n" +
                "\tfollow k --- calculate FOLLOW_k set for nonterminal\n" +
                "\tcheck k --- check grammar for being LL_k\n" +
                "\texit --- exit from program");            

            while (true)
            {
                Console.Write("Enter command: ");
                var command = Console.ReadLine();

                var split = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (split.Length == 1 && split[0] == "exit")
                {
                    return;
                }

                if (split.Length == 2 && int.TryParse(split[1], out int dimension) && dimension > 0)
                {
                    if (split[0] == "first")
                    {
                        CalculateFirst(grammar, dimension, functions);
                        continue;
                    }

                    if (split[0] == "follow")
                    {
                        CalculateFollow(grammar, dimension, functions);
                        continue;
                    }

                    if (split[0] == "check")
                    {
                        CheckLLk(grammar, dimension, checker);
                        continue;
                    }
                }

                Console.WriteLine("Invalid command.");
            }
        }

        private static void PrintGrammar(Cfg grammar)
        {
            Console.Write("Nonterminals:\n\t");

            foreach (var nonterminal in grammar.Nonterminals)
            {
                Console.Write($"{nonterminal} ");
            }

            Console.WriteLine();

            Console.Write("Terminals:\n\t");

            foreach (var terminal in grammar.Terminals)
            {
                Console.Write($"{terminal} ");
            }

            Console.WriteLine();

            Console.WriteLine($"Start symbol:\n\t{grammar.StartSymbol}");

            Console.Write("Productions:\n");

            foreach (var production in grammar.Productions)
            {
                Console.Write($"\t{production.left} ->");

                foreach (var symbol in production.right)
                {
                    Console.Write($" {symbol}");
                }

                Console.WriteLine();
            }
        }

        private static void CalculateFirst(Cfg grammar, int dimension, ILLkFunctions functions)
        {
            try
            {
                Console.Write("Enter sentential form (use space as delimiters): ");
                var entered = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (entered.Length == 0)
                {
                    Console.WriteLine("Empty sentential form entered.");
                    return;
                }

                var sententialForm = new SententialForm();

                for (var i = 0; i < entered.Length; ++i)
                {
                    var symbol = entered[i];
                    var terminal = grammar.Terminals.Where(t => t.Literal == symbol).SingleOrDefault();
                    var nonterminal = grammar.Nonterminals.Where(t => t.Literal == symbol).SingleOrDefault();

                    if (terminal == null && nonterminal == null)
                    {
                        Console.WriteLine($"Symbol '{symbol}' not found in the grammar.");
                        return;
                    }

                    if (terminal != null && nonterminal != null)
                    {
                        while (true)
                        {
                            Console.Write($"Ambiguity. Did you mean nonterminal or terminal '{symbol}' at position {i}? (N/T) ");

                            var resolution = Console.ReadLine();

                            if (resolution == "N" || resolution == "n")
                            {
                                sententialForm += nonterminal;
                                break;
                            }

                            if (resolution == "T" || resolution == "t")
                            {
                                sententialForm += terminal;
                                break;
                            }
                        }
                    }
                    else if (terminal != null)
                    {
                        sententialForm += terminal;
                    }
                    else
                    {
                        sententialForm += nonterminal;
                    }
                }

                var firstSet = functions.First(grammar, sententialForm, dimension);

                Console.WriteLine($"FIRST_{dimension} set for sentential form:");

                foreach (var word in firstSet.OrderBy(w => w.Length))
                {
                    Console.Write("\t");

                    if (word == SententialForm.Epsilon)
                    {
                        Console.Write($"{word}");
                    }

                    foreach (var symbol in word)
                    {
                        Console.Write($"{symbol} ");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error. {e.Message}");
            }
        }

        private static void CalculateFollow(Cfg grammar, int dimension, ILLkFunctions functions)
        {
            try
            {
                Console.Write("Enter nonterminal: ");
                var entered = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (entered.Length != 1)
                {
                    Console.WriteLine("No nonterminal entered or entered more than one nonterminal.");
                    return;
                }

                var nonterminal = grammar.Nonterminals.Where(t => t.Literal == entered[0]).SingleOrDefault();

                if (nonterminal == null)
                {
                    Console.WriteLine($"Nonterminal '{entered[0]}' not found in the grammar.");
                    return;
                }

                var followSet = functions.Follow(grammar, nonterminal, dimension);

                Console.WriteLine($"FOLLOW_{dimension} set for nonterminal {nonterminal}:");

                foreach (var word in followSet.OrderBy(w => w.Length))
                {
                    Console.Write("\t");

                    if (word == SententialForm.Epsilon)
                    {
                        Console.Write($"{word}");
                    }

                    foreach (var symbol in word)
                    {
                        Console.Write($"{symbol} ");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error. {e.Message}");
            }
        }

        private static void CheckLLk(Cfg grammar, int dimension, ILLkChecker checker)
        {
            try
            {
                var checkResult = checker.Check(grammar, dimension);

                if (checkResult)
                {
                    Console.WriteLine($"Grammar is LL_{dimension}".Pastel(Color.LawnGreen));
                }
                else
                {
                    Console.WriteLine($"Grammar is not LL_{dimension}".Pastel(Color.Red));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error. {e.Message}");
            }
        }
    }
}
