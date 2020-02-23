using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class EphemAlias : Node
    {
        public EphemAlias(TokenSpan span, EAKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public EAKind Kind { get; }
    }
}
