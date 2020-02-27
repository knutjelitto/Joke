using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtTypeParameter : PtNode
    {
        public PtTypeParameter(PonyTokenSpan span, PtIdentifier name, PtType? type, PtType? defaultType)
            : base(span)
        {
            Name = name;
            Type = type;
            DefaultType = defaultType;
        }

        public PtIdentifier Name { get; }
        public PtType? Type { get; }
        public PtType? DefaultType { get; }
    }
}
