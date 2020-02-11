using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Lex
{
    public struct Token
    {
        public Token(int start, int length, TK kind) 
        {
            Start = start;
            Length = length;
            Kind = kind;
        }

        public readonly int Start;
        public readonly int Length;
        public readonly TK Kind;
    }
}
