using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class PostfixPart : Expression
    {
        public PostfixPart(TokenSpan span) : base(span)
        {
        }
    }
}
