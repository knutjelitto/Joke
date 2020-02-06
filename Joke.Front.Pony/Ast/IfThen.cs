using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class IfThen : Base
    {
        public IfThen(ISpan span, Expression condition, Expression sequence)
            : base(span)
        {
            Condition = condition;
            Sequence = sequence;
        }

        public Expression Condition { get; }
        public Expression Sequence { get; }
    }
}
