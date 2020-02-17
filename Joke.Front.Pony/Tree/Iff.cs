using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Iff : Expression
    {
        public Iff(TSpan span, IffKind kind, Annotations? annotations, Expression condition, Expression thenPart, Expression? elsePart)
            : base(span)
        {
            Kind = kind;
            Annotations = annotations;
            Condition = condition;
            ThenPart = thenPart;
            ElsePart = elsePart;
        }

        public IffKind Kind { get; }
        public Annotations? Annotations { get; }
        public Expression Condition { get; }
        public Expression ThenPart { get; }
        public Expression? ElsePart { get; }
    }
}
