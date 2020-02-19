using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class LambdaCapture : Node
    {
        protected LambdaCapture(TokenSpan span)
            : base(span)
        {
        }
    }
}
