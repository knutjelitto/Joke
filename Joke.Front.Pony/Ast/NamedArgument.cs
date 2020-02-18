using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class NamedArgument : Argument
    {
        public NamedArgument(TSpan span, Identifier name, Expression value)
            : base(span)
        {
            Name = name;
            Value = value;
        }

        public Identifier Name { get; }
        public Expression Value { get; }
    }
}
