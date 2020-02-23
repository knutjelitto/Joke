using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Repeat : Expression
    {
        public Repeat(TokenSpan span, Annotations? annotations, Expression body, Expression condition, Else? elsePart)
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
