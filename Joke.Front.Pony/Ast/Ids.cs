using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class Ids : Node
    {
        protected Ids(TokenSpan span) : base(span)
        {
        }
    }
}
