using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Try : Expression
    {
        public Try(TSpan span, Annotations? annotations, Expression body, Expression? elsePart, Expression? thenPart)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
            ElsePart = elsePart;
            ThenPart = thenPart;
        }

        public Annotations? Annotations { get; }
        public Expression Body { get; }
        public Expression? ElsePart { get; }
        public Expression? ThenPart { get; }
    }
}
