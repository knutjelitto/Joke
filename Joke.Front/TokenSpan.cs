using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Front
{
    public class TokenSpan<T> where T: IToken
    {
        public TokenSpan(IReadOnlyList<T> tokens, int start, int next)
        {
            Tokens = tokens;
            Start = start;
            Next = next;
            Length = next - start;
        }

        public readonly int Start;
        public readonly int Next;

        public int Length { get; }

        public IReadOnlyList<T> Tokens { get; }

        public T this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && Start + index < Next);
                return Tokens[Start + index];
            }
        }

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

        public override string ToString()
        {
            return string.Join("-", GetPayloads());
        }
    }
}
