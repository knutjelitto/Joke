using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
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
