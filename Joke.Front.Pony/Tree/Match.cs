using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Match : Expression
    {
        public Match(TSpan span, Annotations? annotations, Expression toMatch, Cases cases, Expression? elsePart)
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
