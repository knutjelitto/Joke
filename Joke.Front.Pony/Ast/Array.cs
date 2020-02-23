using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Array : Expression
    {
        public Array(TokenSpan span, Type? type, Expression? elements)
            : base(span)
        {
            Type = type;
            Elements = elements;
        }

        public Type? Type { get; }
        public Expression? Elements { get; }
    }
}
