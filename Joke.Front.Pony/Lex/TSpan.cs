using Joke.Front.Pony.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Lex
{
    public struct TSpan
    {
        public TSpan(PonyParser parser, int start, int next)
        {
            Parser = parser;
            Start = start;
            Next = next;
        }

        public readonly int Start;
        public readonly int Next;

        public PonyParser Parser { get; }
    }
}
