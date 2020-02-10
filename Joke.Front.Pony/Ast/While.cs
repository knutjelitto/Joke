using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class While : Expression
    {
        public While(ISpan span, Expression condition, Expression body, Expression? @else)
            : base(span)
        {
            Condition = condition;
            Body = body;
            Else = @else;
        }

        public Expression Condition { get; }
        public Expression Body { get; }
        public Expression? Else { get; }
    }
}
