using System;
using System.Collections.Generic;
using System.Text;

namespace LLkGrammarChecker
{
    public abstract class GrammarSymbol
    {
        public string Literal { get; private set; }

        public GrammarSymbol(string literal)
        {
            Literal = literal;
        }

        public override int GetHashCode()
        {
            return Literal.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var haveSameType = obj.GetType() == this.GetType();
            return haveSameType && ((GrammarSymbol)obj).Literal == Literal ;
        }

        public override string ToString()
        {
            return Literal;
        }

        public static bool operator ==(GrammarSymbol left, GrammarSymbol right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GrammarSymbol left, GrammarSymbol right)
        {
            return !left.Equals(right);
        }
    }
}
