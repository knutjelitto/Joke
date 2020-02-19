using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class PositionalArgument : Argument
    {
        public PositionalArgument(TokenSpan span, Expression value)
            : base(span)
        {
            Value = value;
        }

        public Expression Value { get; }
    }
}
