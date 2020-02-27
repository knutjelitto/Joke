using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtChain : PtPostfixPart
    {
        public PtChain(PonyTokenSpan span, PtIdentifier method)
            : base(span)
        {
            Method = method;
        }

        public PtIdentifier Method { get; }
    }
}
