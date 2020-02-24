using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Cap : Type
    {
        public Cap(PonyTokenSpan span, CapKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public CapKind Kind { get; }
    }
}
