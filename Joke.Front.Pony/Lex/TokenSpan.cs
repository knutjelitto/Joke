using Joke.Front.Pony.Syntax;
using System.Collections.Generic;

namespace Joke.Front.Pony.Lex
{
    public struct TokenSpan
    {
        public TokenSpan(PonyParser parser, int start, int next)
        {
            Parser = parser;
            Start = start;
            Next = next;
        }

        public readonly int Start;
        public readonly int Next;

        public PonyParser Parser { get; }

        public IEnumerable<Token> GetTokens()
        {
            for (var i = Start; i < Next; ++i)
            {
                yield return Parser.Tokens[i];
            }
        }

        public IEnumerable<string> GetPayloads()
        {
            foreach (var token in GetTokens())
            {
                yield return token.GetPayload(Parser.Source);
            }
        }

        public override string? ToString()
        {
            return string.Join("-", GetPayloads());
        }
    }
}
