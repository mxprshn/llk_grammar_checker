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
            if (ReferenceEquals(null, obj))
                return ReferenceEquals(null, this);

            var haveSameType = obj.GetType() == this.GetType();
            return haveSameType && ((GrammarSymbol)obj).Literal == Literal ;
        }

        public override string ToString()
        {
            return Literal;
        }

        public static SententialForm operator +(GrammarSymbol left, GrammarSymbol right)
        {
            return new SententialForm(new GrammarSymbol[2] { left, right });
        }

        public static bool operator ==(GrammarSymbol left, GrammarSymbol right)
        {
            if (ReferenceEquals(null, left))
                return ReferenceEquals(null, right);

            return left.Equals(right);
        }

        public static bool operator !=(GrammarSymbol left, GrammarSymbol right)
        {
            if (ReferenceEquals(null, left))
                return !ReferenceEquals(null, right);

            return !left.Equals(right);
        }
    }
}
