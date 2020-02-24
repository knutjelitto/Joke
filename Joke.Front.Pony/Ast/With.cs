﻿using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class With : Expression
    {
        public With(PonyTokenSpan span, Annotations? annotations, WithElements elements, Expression body, Else? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Elements = elements;
            Body = body;
            ElsePart = elsePart;
        }

        public Annotations? Annotations { get; }
        public WithElements Elements { get; }
        public Expression Body { get; }
        public Else? ElsePart { get; }
    }
}
