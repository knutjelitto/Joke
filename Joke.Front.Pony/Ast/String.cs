using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class String : Literal
    {
        public String(TokenSpan span)
            : base(span)
        {
        }
    }
}
