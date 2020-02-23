using System;
using System.Collections;
using System.Linq;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Syntax
{
    public class TokenSet
    {
        private static int tokenCount = Enum.GetValues(typeof(TK)).Cast<int>().Max() + 1;
        private readonly BitArray bits;

        public TokenSet()
        {
            bits = new BitArray(tokenCount);
        }

        public TokenSet(TokenSet other)
        {
            bits = new BitArray(other.bits);
        }

        public TokenSet(params TK[] tokens)
            : this()
        {
            foreach (var token in tokens)
            {
                bits.Set((int)token, true);
            }
        }

        public bool this[TK token]
        {
            get => bits.Get((int)token);
            set => bits.Set((int)token, value);
        }

        public TokenSet Union(TokenSet other)
        {
            bits.Or(other.bits);
            return this;
        }

        public static TokenSet Union(params TokenSet[] sets)
        {
            var result = new TokenSet(sets[0]);
            for (var i = 1; i < sets.Length; ++i)
            {
                result.bits.Or(sets[i].bits);
            }
            return result;
        }
    }
}
