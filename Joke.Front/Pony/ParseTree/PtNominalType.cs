using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtNominalType : PtType
    {
        public PtNominalType(PonyTokenSpan span, PtIdentifier name, PtTypeArguments? typeArguments, PtCap? cap, PtEphemAlias? ea)
            : base(span)
        {
            Name = name;
            TypeArguments = typeArguments;
            Cap = cap;
            Ea = ea;
        }

        public PtIdentifier Name { get; }
        public PtTypeArguments? TypeArguments { get; }
        public PtCap? Cap { get; }
        public PtEphemAlias? Ea { get; }
    }
}
