using System.Collections.Generic;

namespace Joke.Front.Pony.Lexing
{
    public class PonyTokens : Tokens<PonyToken>
    {
        public PonyTokens(ISource source, IReadOnlyList<PonyToken> tokens)
            : base(source, tokens)
        {
        }
    }
}
