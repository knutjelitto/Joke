using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtIdsSingle : PtIds
    {
        public PtIdsSingle(PonyTokenSpan span, PtIdentifier name)
            : base(span)
        {
        }
    }
}
