using System.Collections.Generic;

namespace Joke.Front
{
    public class TokenSpan<T> where T: IToken
    {
        public TokenSpan(IReadOnlyList<T> tokens, int start, int next)
        {
            Tokens = tokens;
            Start = start;
            Next = next;
        }

        public readonly int Start;
        public readonly int Next;

        public IReadOnlyList<T> Tokens { get; }

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
