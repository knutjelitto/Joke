using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Partial : Node
    {
        public Partial(TokenSpan span)
            : base(span)
        {
        }
    }
}
