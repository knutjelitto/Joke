using System;
using System.Collections;
using System.Linq;

namespace Joke.Front.Pony.Parsing
{
    public class TokenSet<TS, TK> where TS : TokenSet<TS, TK>, new() where TK: struct, IConvertible
    {
        private static int tokenCount = Enum.GetValues(typeof(TK)).Cast<int>().Max() + 1;
        private readonly BitArray bits;

        public TokenSet()
        {
            bits = new BitArray(tokenCount);
        }

        public TokenSet(TS other)
        {
            bits = new BitArray(other.bits);
        }

        public TokenSet(params TS[] sets)
            : this()
        {
            for (var i = 0; i < sets.Length; ++i)
            {
                bits.Or(sets[i].bits);
            }
        }

        public TokenSet(params TK[] tokens)
            : this()
        {
            foreach (var token in tokens)
            {
                bits.Set(token.ToInt32(null), true);
            }
        }

        public bool this[TK token]
        {
            get => bits.Get(token.ToInt32(null));
            set => bits.Set(token.ToInt32(null), value);
        }

        public TS Union(TS other)
        {
            bits.Or(other.bits);
            return (TS)this;
        }

        public static TS Union(params TS[] sets)
        {
            var result = new TS();
            for (var i = 0; i < sets.Length; ++i)
            {
                result.bits.Or(sets[i].bits);
            }
            return result;
        }
    }
}
