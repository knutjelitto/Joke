using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Joke.Decoding
{
    public class TokenSpan : ITokenSpan
    {
        public TokenSpan(Tokens tokens, int start, int next)
        {
            Tokens = tokens;
            Start = start;
            Next = next;
            Length = next - start;
        }

        public readonly int Start;
        public readonly int Next;

        public int Length { get; }

        public Tokens Tokens { get; }

        public ISourceSpan PayloadSpan => new SourceSpan(Tokens[0].Source, Tokens[Start].PayloadOffset, Tokens[Next].ClutterOffset);

        public IToken First => Tokens[Start];

        public Token this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && Start + index < Next);
                return Tokens[Start + index];
            }
        }

        public IEnumerable<Token> GetTokens()
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
                yield return token.Payload;
            }
        }

        public override string ToString()
        {
            return string.Join(" ", GetPayloads());
        }
    }
}
