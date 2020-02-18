using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class ThisLiteral : Expression
    {
        public ThisLiteral(TSpan span)
            : base(span)
        {
        }
    }
}
