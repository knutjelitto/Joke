using Joke.Front.Pony.Lex;

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
