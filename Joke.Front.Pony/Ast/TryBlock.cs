using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class TryBlock : Expression
    {
        public TryBlock(ISpan span, Expression body, Expression? @else, Expression? then)
            : base(span)
        {
            Body = body;
            Else = @else;
            Then = then;
        }

        public Expression Body { get; }
        public Expression? Else { get; }
        public Expression? Then { get; }
    }
}
