using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Bool : Literal
    {
        public Bool(TokenSpan span, bool value)
            : base(span)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
