using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class ThisLiteral : Expression
    {
        public ThisLiteral(TokenSpan span)
            : base(span)
        {
        }
    }
}
