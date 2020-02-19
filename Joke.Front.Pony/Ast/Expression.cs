using Joke.Front.Pony.Lex;

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
