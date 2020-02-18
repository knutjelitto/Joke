using System.Collections.Generic;

using Joke.Front.Pony.Err;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        public List<Error> Messages = new List<Error>();

        public int Offset => Tokens[next].Payload;

        protected NotYetException NotYet(string message)
        {
            return new NotYetException(message);
        }

        public NoParseException NoParse(string message)
        {
            return new NoParseException(message);
        }

        public void AddError(Token token, string msg)
        {
            Messages.Add(new AtTokenError(ErrorKind.Error, token, msg));
        }
    }
}
