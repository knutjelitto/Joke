using Joke.Front.Pony.Err;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        protected JokeException NotYet(string message)
        {
            return new JokeException(new JokeError(new AtToken(Source, Token, "not implemented: " + message)));
        }

        public JokeException NoParse(string message)
        {
            return new JokeException(new JokeError(new AtToken(Source, Token, message)));
        }

        public void AddError(Token token, string msg)
        {
            Errors.Add(new JokeError(new AtToken(Source, token, msg)));
        }
    }
}
