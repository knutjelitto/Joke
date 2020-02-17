using Joke.Front.Pony.Lex;
using System;

namespace Joke.Front.Pony.Ast
{
    public class Identifier : Expression
    {
        public Identifier(TSpan span)
            : base(span)
        {
        }
    }
}
