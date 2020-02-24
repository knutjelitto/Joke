using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Ids : Node
    {
        protected Ids(PonyTokenSpan span) : base(span)
        {
        }
    }
}
