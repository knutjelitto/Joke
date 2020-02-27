using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtInfixType : PtType
    {
        public PtInfixType(PonyTokenSpan span, PtInfixTypeKind kind, IReadOnlyList<PtType> types)
            : base(span)
        {
            Kind = kind;
            Types = types;
        }

        public PtInfixTypeKind Kind { get; }
        public IReadOnlyList<PtType> Types { get; }
    }
}
