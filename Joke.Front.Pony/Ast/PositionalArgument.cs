using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class PositionalArgument : Argument
    {
        public PositionalArgument(PonyTokenSpan span, Expression value)
            : base(span)
        {
            Value = value;
        }

        public Expression Value { get; }
    }
}
