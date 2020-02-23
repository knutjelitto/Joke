using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Ref : Expression
    {
        public Ref(TokenSpan span, Identifier name)
            : base(span)
        {
            Name = name;
        }

        public Identifier Name { get; }
    }
}
