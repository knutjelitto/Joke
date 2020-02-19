using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class For : Expression
    {
        public For(TokenSpan span, Annotations? annotations, Ids names, Expression iterator, Expression body, Expression? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Names = names;
            Iterator = iterator;
            Body = body;
            ElsePart = elsePart;
        }

        public Annotations? Annotations { get; }
        public Ids Names { get; }
        public Expression Iterator { get; }
        public Expression Body { get; }
        public Expression? ElsePart { get; }
    }
}
