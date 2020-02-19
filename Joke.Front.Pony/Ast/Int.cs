using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Int : Literal
    {
        public Int(TokenSpan span)
            : base(span)
        {
        }
    }
}
