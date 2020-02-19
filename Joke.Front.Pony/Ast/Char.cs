using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Char : Expression
    {
        public Char(TokenSpan span)
            : base(span)
        {
        }
    }
}
