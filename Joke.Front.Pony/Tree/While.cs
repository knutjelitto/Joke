using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class While : Expression
    {
        public While(TSpan span, Annotations? annotations, Expression condition, Expression body, Expression? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Condition = condition;
            Body = body;
            ElsePart = elsePart;
        }

        public Annotations? Annotations { get; }
        public Expression Condition { get; }
        public Expression Body { get; }
        public Expression? ElsePart { get; }
    }
}
