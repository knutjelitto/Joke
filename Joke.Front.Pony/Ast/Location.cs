using Joke.Front.Pony.Lexing;

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
