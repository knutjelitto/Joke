using Joke.Front.Pony.Tree;

namespace Joke.Front.Pony
{
    partial class PonyParser
    {
        public int Offset => Tokens[next].Payload;

        protected NotYetException NotYet(string message)
        {
            return new NotYetException(message);
        }

        public NoParseException NoParse(string message)
        {
            return new NoParseException(message);
        }
    }
}
