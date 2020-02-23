using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Expression : Node
    {
        protected Expression(TokenSpan span)
            : base(span)
        {
        }
    }
}
