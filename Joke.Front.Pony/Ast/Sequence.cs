using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Sequence : Expression
    {
        public Sequence(ISpan span, IReadOnlyList<Expression> expressions)
            : base(span)
        {
            Expressions = expressions;
        }

        public IReadOnlyList<Expression> Expressions { get; }
    }
}
