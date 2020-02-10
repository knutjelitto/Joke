using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Tuple : Expression
    {
        public Tuple(ISpan span, IReadOnlyList<Expression> values)
            : base(span)
        {
            Values = values;
        }

        public IReadOnlyList<Expression> Values { get; }
    }
}
