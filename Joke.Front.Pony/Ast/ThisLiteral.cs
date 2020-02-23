using Joke.Front.Pony.Lexing;

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
