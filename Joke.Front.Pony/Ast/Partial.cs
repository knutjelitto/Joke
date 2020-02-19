using Joke.Front.Pony.Lex;

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
