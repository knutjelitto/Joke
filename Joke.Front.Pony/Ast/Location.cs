using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Location : Expression
    {
        public Location(TSpan span)
            : base(span)
        {
        }
    }
}
