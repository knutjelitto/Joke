using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class With : Expression
    {
        public With(ISpan span, IReadOnlyList<WithElement> elements, Expression body, Expression? @else)
            : base(span)
        {
            Elements = elements;
            Body = body;
            Else = @else;
        }

        public IReadOnlyList<WithElement> Elements { get; }
        public Expression Body { get; }
        public Expression? Else { get; }
    }
}
