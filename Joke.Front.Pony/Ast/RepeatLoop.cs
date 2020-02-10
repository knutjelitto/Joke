using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class RepeatLoop : Expression
    {
        public RepeatLoop(ISpan span, Expression body, Expression condition, Expression? @else)
            : base(span)
        {
            Body = body;
            Condition = condition;
            Else = @else;
        }

        public Expression Body { get; }
        public Expression Condition { get; }
        public Expression? Else { get; }
    }
}
