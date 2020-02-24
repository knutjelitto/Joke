using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Type : Node
    {
        protected Type(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
