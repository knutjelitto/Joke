﻿using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Match : Expression
    {
        public Match(PonyTokenSpan span, Annotations? annotations, Expression toMatch, Cases cases, Expression? elsePart)
            : base(span)
        {
            Annotations = annotations;
            ToMatch = toMatch;
            Cases = cases;
            ElsePart = elsePart;
        }

        public Annotations? Annotations { get; }
        public Expression ToMatch { get; }
        public Cases Cases { get; }
        public Expression? ElsePart { get; }
    }
}
