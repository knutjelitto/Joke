using Joke.Front.Pony.Lexing;

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
