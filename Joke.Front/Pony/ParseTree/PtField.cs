using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtField : PtNode
    {
        public PtField(PonyTokenSpan span, PtFieldKind kind, PtIdentifier name, PtType type, PtExpression? value, PtString? doc)
            : base(span)
        {
            Kind = kind;
            Name = name;
            Type = type;
            Value = value;
        }

        public PtFieldKind Kind { get; }
        public PtIdentifier Name { get; }
        public PtType Type { get; }
        public PtExpression? Value { get; }
    }
}
