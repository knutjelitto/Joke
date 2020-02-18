using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Bool : Literal
    {
        public Bool(TSpan span, bool value)
            : base(span)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
