using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace LLkGrammarChecker
{
    public class SententialForm : IEnumerable<GrammarSymbol>
    {
        private GrammarSymbol[] elements;

        public int Length => elements.Length;

        public bool IsTerminal => Length == 1 && this[0] is Terminal;
        public bool IsNonterminal => Length == 1 && this[0] is Nonterminal;
        public bool ContainsNonterminals => elements.OfType<Nonterminal>().Count() > 0;

        public GrammarSymbol this[int index] => elements[index];

        public SententialForm()
        {
            this.elements = new GrammarSymbol[0];
        }

        public SententialForm(IEnumerable<GrammarSymbol> elements)
        {
            this.elements = elements.ToArray();
        }

        public SententialForm(GrammarSymbol element)
        {
            this.elements = new GrammarSymbol[] { element };
        }

        public SententialForm Trim(int lengthToKeep)
        {
            if (lengthToKeep < 0)
            {
                throw new ArgumentException("Length to keep should be >= 0.");
            }

            var newSize = Math.Min(lengthToKeep, elements.Length);
            var copy = new GrammarSymbol[newSize];
            Array.Copy(elements, copy, newSize);
            return new SententialForm(copy);
        }

        public override bool Equals(object obj)
        {
            return (obj as SententialForm).SequenceEqual(this);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            if (elements.Length == 0) return "ε";

            return String.Join(String.Empty, elements.Select(e => e.Literal));
        }

        public IEnumerator<GrammarSymbol> GetEnumerator()
        {
            return ((IEnumerable<GrammarSymbol>)elements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public static SententialForm Epsilon => new SententialForm();

        public static SententialForm operator +(SententialForm left, SententialForm right)
        {
            return new SententialForm(left.Concat(right.elements));
        }

        public static SententialForm operator +(SententialForm left, GrammarSymbol right)
        {
            return new SententialForm(left.Concat(new GrammarSymbol[1] { right }));
        }

        public static bool operator ==(SententialForm left, SententialForm right)
        {
            if (ReferenceEquals(null, left))
                return ReferenceEquals(null, right);

            return left.Equals(right);
        }

        public static bool operator !=(SententialForm left, SententialForm right)
        {
            if (ReferenceEquals(null, left))
                return !ReferenceEquals(null, right);

            return !left.Equals(right);
        }

        public static bool operator ==(SententialForm left, Nonterminal right)
        {
            if (ReferenceEquals(null, left))
                return ReferenceEquals(null, right);

            return left.IsNonterminal && left.elements[0] == right;
        }

        public static bool operator !=(SententialForm left, Nonterminal right)
        {
            if (ReferenceEquals(null, left))
                return !ReferenceEquals(null, right);

            return !left.IsNonterminal || left.elements[0] != right;
        }

        public static bool operator ==(SententialForm left, Terminal right)
        {
            if (ReferenceEquals(null, left))
                return ReferenceEquals(null, right);

            return left.IsTerminal && left.elements[0] == right;
        }

        public static bool operator !=(SententialForm left, Terminal right)
        {
            if (ReferenceEquals(null, left))
                return !ReferenceEquals(null, right);

            return !left.IsTerminal || left.elements[0] != right;
        }
    }
}
