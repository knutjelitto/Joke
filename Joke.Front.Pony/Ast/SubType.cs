using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class SubType : Expression
    {
        public SubType(TokenSpan span, Type sub, Type super)
            : base(span)
        {
            Sub = sub;
            Super = super;
        }

        public Type Sub { get; }
        public Type Super { get; }
    }
}
