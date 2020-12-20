using LLkGrammarChecker.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using LLkGrammarChecker.Parser;

namespace LLkGrammarChecker
{
    public class BackusNaurParserLogic : IParser
    {
        public async Task<CFG> FromFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException();

            return FromString(await File.ReadAllTextAsync(path));
        }

        public CFG FromString(string source)
        {
            CFG grammar = null;

            var lines = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; ++i)
            {
                var leftAndRight = lines[i].Split("::=");

                if (leftAndRight.Length != 2) throw new BackusNaurParserException("::= is missing or appears more than once in line.");

                var left = leftAndRight[0].Trim();

                var nonterminal = new Nonterminal(FromAngleBrackets(left));

                if (i != 0)
                {
                    grammar.AddNonterminal(nonterminal);                    
                }
                else
                {
                    grammar = new CFG(nonterminal);
                }                

                if (grammar.Productions.Count(p => p.left == nonterminal) != 0)
                {
                    throw new BackusNaurParserException($"Nonterminal {nonterminal} is already defined.");
                }

                foreach (var right in leftAndRight[1].Split('|'))
                {
                    var productionSymbols = right.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (productionSymbols.Length == 0)
                    {
                        throw new BackusNaurParserException("Right part of the production cannot be empty.");
                    }

                    var sententia = new SententialForm();

                    foreach (var productionSymbol in productionSymbols)
                    {
                        if (IsInAngleBrackets(productionSymbol))
                        {
                            var productionNonterminal = new Nonterminal(productionSymbol);
                            sententia += productionNonterminal;
                        }
                        else
                        {
                            var withoutQuotes = FromQuotes(productionSymbol);
                            var productionTerminal = new Terminal(withoutQuotes);
                            sententia += productionTerminal;
                        }
                    }

                    grammar.AddProduction(new SententialForm(nonterminal), sententia);
                }
            }

            if (grammar == null)
            {
                throw new BackusNaurParserException("Cannot find any nonterminal definitions.");
            }

            return grammar;
        }

        private string FromAngleBrackets(string source)
        {
            if (!IsInAngleBrackets(source)) throw new BackusNaurParserException("Nonterminal should be enclosed in angle brackets.");

            return source.Substring(1, source.Length - 2);
        }

        private string FromQuotes(string source)
        {
            if (!IsInQuotes(source))
            {
                return source;
            }

            return source.Substring(1, source.Length - 2);
        }

        private bool IsInAngleBrackets(string source)
        {
            return source.LastIndexOf('<') == 0 && source.IndexOf('>') == source.Length - 1;
        }

        private bool IsInQuotes(string source)
        {
            return source.LastIndexOf('"') == source.Length - 1 && source.IndexOf('"') == 0;
        }
    }
}