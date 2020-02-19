using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class LambdaParameter : Parameter
    {
        public LambdaParameter(TokenSpan span, Identifier name, Type? type, Expression? value)
            : base(span)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public Identifier Name { get; }
        public Type? Type { get; }
        public Expression? Value { get; }
    }
}
