using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Expression : Node
    {
        protected Expression(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
