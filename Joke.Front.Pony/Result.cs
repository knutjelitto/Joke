using System.Collections.Generic;

using Joke.Front.Pony.Lexing;
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
        public ISource Source => Parser.Source;
        public IReadOnlyList<Token> Tokens => Parser.Tokens;
    }
}
