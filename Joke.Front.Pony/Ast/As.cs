using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class As : Expression
    {
        public As(PonyTokenSpan span, Expression value, Type type)
            : base(span)
        {
            Value = value;
            Type = type;
        }

        public Expression Value { get; }
        public Type Type { get; }
    }
}
