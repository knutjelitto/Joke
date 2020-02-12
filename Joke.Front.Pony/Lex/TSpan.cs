using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Lex
{
    public struct TSpan
    {
        public TSpan(IReadOnlyList<Token> tokens, int start, int next)
        {
            Tokens = tokens;
            Start = start;
            Next = next;
        }

        public readonly IReadOnlyList<Token> Tokens;
        public readonly int Start;
        public readonly int Next;
    }
}
