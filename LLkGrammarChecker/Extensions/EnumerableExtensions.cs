using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker.Extensions
{
    static class EnumerableExtensions
    {
        public static SententialForm ToSententia(this IEnumerable<GrammarSymbol> enumerable)
        {
            return new SententialForm(enumerable);
        }
    }
}
