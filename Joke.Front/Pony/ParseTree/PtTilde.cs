using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtTilde : PtPostfixPart
    {
        public PtTilde(PonyTokenSpan span, PtIdentifier method)
            : base(span)
        {
            Method = method;
        }

        public PtIdentifier Method { get; }
    }
}
