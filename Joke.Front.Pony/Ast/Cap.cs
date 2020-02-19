using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Cap : Type
    {
        public Cap(TokenSpan span, CapKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public CapKind Kind { get; }
    }
}
