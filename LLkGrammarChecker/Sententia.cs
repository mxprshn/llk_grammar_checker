using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace LLkGrammarChecker
{
    public class Sententia : IEnumerable<GrammarSymbol>
    {
        private GrammarSymbol[] elements;

        public int Length => elements.Length;

        public bool IsTerminal => Length == 1 && this[0] is Terminal;
        public bool IsNonterminal => Length == 1 && this[0] is Nonterminal;
        public bool ContainsNonterminals => elements.OfType<Nonterminal>().Count() > 0;

        public GrammarSymbol this[int index] => elements[index];

        public Sententia(IEnumerable<GrammarSymbol> elements)
        {
            this.elements = elements.ToArray();
        }

        public Sententia(GrammarSymbol element)
        {
            this.elements = new GrammarSymbol[] { element };
        }

        public Sententia Trim(int lengthToKeep)
        {
            if (lengthToKeep < 0)
            {
                throw new ArgumentException("Length to keep should be >= 0.");
            }

            var newSize = Math.Min(lengthToKeep, elements.Length);
            var copy = new GrammarSymbol[newSize];
            Array.Copy(elements, copy, newSize);
            return new Sententia(copy);
        }

        public override bool Equals(object obj)
        {
            return (obj as Sententia).SequenceEqual(this);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Join(String.Empty, elements.Select(e => e.Literal));
        }

        public IEnumerator<GrammarSymbol> GetEnumerator()
        {
            return elements.GetEnumerator() as IEnumerator<GrammarSymbol>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public static Sententia operator +(Sententia left, Sententia right)
        {
            return new Sententia(left.Concat(right));
        }

        public static bool operator ==(Sententia left, Nonterminal right)
        {
            return left.IsNonterminal && left.elements[0] == right;
        }

        public static bool operator !=(Sententia left, Nonterminal right)
        {
            return !left.IsNonterminal || left.elements[0] != right;
        }

        public static bool operator ==(Sententia left, Terminal right)
        {
            return left.IsTerminal && left.elements[0] == right;
        }

        public static bool operator !=(Sententia left, Terminal right)
        {
            return !left.IsTerminal || left.elements[0] != right;
        }
    }
}
