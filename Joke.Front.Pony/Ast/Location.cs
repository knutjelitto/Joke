using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Location : Expression
    {
        public Location(TokenSpan span)
            : base(span)
        {
        }
    }
}
