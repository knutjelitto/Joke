using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtIdentifier : PtLiteral<string>
    {
        public PtIdentifier(PonyTokenSpan span)
            : base(span, span.ToString())
        {
        }
    }
}
