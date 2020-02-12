using Joke.Front.Pony.Lex;
using System;

namespace Joke.Front.Pony.Tree
{
    public class Identifier : Expression
    {
        public Identifier(TSpan span)
            : base(span)
        {
        }
    }
}
