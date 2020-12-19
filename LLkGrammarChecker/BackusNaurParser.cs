using LLkGrammarChecker.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LLkGrammarChecker
{
    class BackusNaurParser
    {
        public static async Task<CFG> FromFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException();

            return FromString(await File.ReadAllTextAsync(path));
        }

        public static CFG FromString(string source)
        {
            var grammar = new CFG();

            foreach (var ruleString in source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var leftAndRight = ruleString.Split("::=");

                if (leftAndRight.Length != 2) throw new BackusNaurParserException("::= is missing or appears more than once in line.");

                var left = leftAndRight[0].Trim();

                var definedNonterminal = grammar.AddOrGetNonterminal(FromAngleBrackets(left));
                if (definedNonterminal.NonterminalProductions.Count > 0 || definedNonterminal.TerminalProductions.Count > 0)
                    throw new ParserException($"Нетерминал {definedNonterminal} определяется во второй раз.");

                foreach (var right in leftAndRight[1].Split('|'))
                {
                    var productionSymbols = right.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (productionSymbols.Length == 0 || productionSymbols.Length > 2)
                        throw new ParserException("Число символов в правой части правила должно быть равно 1 или 2.");

                    var terminalLiteral = productionSymbols[0];
                    if (IsInAngleBrackets(terminalLiteral)) throw new ParserException("Терминал не может быть заключен в угловые скобки.");
                    var terminal = grammar.AddOrGetTerminal(terminalLiteral);

                    if (productionSymbols.Length == 1)
                    {
                        definedNonterminal.AddTerminalProduction(terminal);
                        continue;
                    }

                    var nonterminal = grammar.AddOrGetNonterminal(FromAngleBrackets(productionSymbols[1]));
                    definedNonterminal.AddNonterminalProduction(terminal, nonterminal);
                }
            }

            return grammar;
        }

        private static string FromAngleBrackets(string source)
        {
            if (!IsInAngleBrackets(source)) throw new ParserException("Нетерминал должен быть заключен в угловые скобки.");

            return source.Substring(1, source.Length - 2);
        }

        private static bool IsInAngleBrackets(string source)
        {
            return source.LastIndexOf('<') == 0 && source.IndexOf('>') == source.Length - 1;
        }
    }
}
