using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class EphemAlias : Node
    {
        public EphemAlias(TSpan span, EAKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public EAKind Kind { get; }
    }
}
