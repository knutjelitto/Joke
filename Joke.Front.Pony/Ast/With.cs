using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class With : Expression
    {
        public With(TSpan span, Annotations? annotations, WithElements elements, Expression body, Else? elsePart)
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
