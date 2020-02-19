using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class As : Expression
    {
        public As(TokenSpan span, Expression value, Type type)
            : base(span)
        {
            Value = value;
            Type = type;
        }

        public Expression Value { get; }
        public Type Type { get; }
    }
}
