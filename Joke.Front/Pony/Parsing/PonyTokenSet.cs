using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing
{
    public class PonyTokenSet : TokenSet<PonyTokenSet, TK>
    {
        public PonyTokenSet()
            : base(new  PonyTokenSet[] { })
        {
        }
        public PonyTokenSet(params PonyTokenSet[] tokens)
            : base(tokens)
        {
        }

        public PonyTokenSet(params TK[] tokens)
            : base(tokens)
        {
        }

    }
}
