using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class String : Literal
    {
        public String(TSpan span)
            : base(span)
        {
        }
    }
}
