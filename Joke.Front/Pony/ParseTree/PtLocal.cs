using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLocal : PtExpression
    {
        public PtLocal(PonyTokenSpan span, PtLocalKind kind, PtIdentifier name, PtType? type)
            : base(span)
        {
            Kind = kind;
            Name = name;
            Type = type;
        }

        public PtLocalKind Kind { get; }
        public PtIdentifier Name { get; }
        public PtType? Type { get; }
    }
}
