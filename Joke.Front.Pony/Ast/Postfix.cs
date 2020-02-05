using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Postfix : Expression
    {
        public Postfix(ISpan span, Expression atom)
            : base(span)
        {
            Atom = atom;
        }

        public Expression Atom { get; }
    }
}
