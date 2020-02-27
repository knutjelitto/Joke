using Joke.Front.Err;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing
{
    partial class PonyParser
    {
        protected JokeException NotYet(string message)
        {
            return new JokeException(new JokeError(new AtToken(Token, "not implemented: " + message)));
        }

        private JokeException NoParse(string message)
        {
            return new JokeException(new JokeError(new AtToken(Token, message)));
        }

        private void AddError(PonyToken token, string msg)
        {
            Errors.Add(new JokeError(new AtToken(token, msg)));
        }
    }
}
