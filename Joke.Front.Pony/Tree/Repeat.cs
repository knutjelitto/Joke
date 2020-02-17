using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Repeat : Expression
    {
        public Repeat(TSpan span, Annotations? annotations, Expression body, Expression condition, Else? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
            Condition = condition;
            ElsePart = elsePart;
        }

        public Annotations? Annotations { get; }
        public Expression Body { get; }
        public Expression Condition { get; }
        public Else? ElsePart { get; }
    }
}
