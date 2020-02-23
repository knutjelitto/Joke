using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Argument : Expression
    {
        public Argument(TokenSpan span)
            : base(span)
        {
        }
    }
}
