using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Location : Expression
    {
        public Location(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
