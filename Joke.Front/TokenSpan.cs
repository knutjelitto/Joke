using System.Collections.Generic;

namespace Joke.Front.Pony.Lexing
{
    public class TokenSpan<T,N> where T: IToken where N : Tokens<T>
    {
        public TokenSpan(N tokens, int start, int next)
        {
            Tokens = tokens;
            Start = start;
            Next = next;
        }

        public readonly int Start;
        public readonly int Next;

        public N Tokens { get; }

        public IEnumerable<T> GetTokens()
        {
            for (var i = Start; i < Next; ++i)
            {
                yield return Tokens[i];
            }
        }

        public IEnumerable<string> GetPayloads()
        {
            foreach (var token in GetTokens())
            {
                yield return token.GetPayload();
            }
        }

        public override string? ToString()
        {
            return string.Join("-", GetPayloads());
        }
    }
}
