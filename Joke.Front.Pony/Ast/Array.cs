using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Array : Expression
    {
        public Array(TSpan span, Type? type, Expression? elements)
            : base(span)
        {
            Type = type;
            Elements = elements;
        }

        public Type? Type { get; }
        public Expression? Elements { get; }
    }
}
