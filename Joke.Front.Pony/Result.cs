using System.Collections.Generic;

using Joke.Front.Pony.Lex;
using Joke.Front.Pony.Syntax;

namespace Joke.Front.Pony
{
    public class Result
    {
        public Result(PonyParser parser)
        {
            Parser = parser;
        }

        public PonyParser Parser { get; }
        public Source Source => Parser.Source;
        public IReadOnlyList<Token> Tokens => Parser.Tokens;
    }
}
