using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Sequence : Expression
    {
        public Sequence(ISpan span, Expression first, Expression rest)
            : base(span)
        {
            First = first;
            Rest = rest;
        }

        public Expression First { get; }
        public Expression Rest { get; }
    }
}
