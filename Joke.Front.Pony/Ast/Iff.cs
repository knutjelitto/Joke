using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
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
