using Joke.Front.Err;

namespace Joke.Front.Pony.Parsing
{
    partial class PonyParser
    {
        protected JokeException NotYet(string message)
        {
            return new JokeException(new JokeError(ErrNo.NotYetParse, new AtToken(Token, "not implemented: " + message)));
        }

        private JokeException NoParse(string message)
        {
            return new JokeException(new JokeError(ErrNo.NoScanParse, new AtToken(Token, message)));
        }
    }
}
