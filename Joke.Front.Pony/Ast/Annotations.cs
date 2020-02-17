using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Annotations : Node
    {
        public Annotations(TSpan span, IReadOnlyList<Identifier> names)
            : base(span)
        {
        }
    }
}
