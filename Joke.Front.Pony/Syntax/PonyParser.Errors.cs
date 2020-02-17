using Joke.Front.Pony.Ast;

namespace Joke.Front.Pony.Syntax
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
