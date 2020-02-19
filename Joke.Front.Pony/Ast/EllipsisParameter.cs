using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class EllipsisParameter : Parameter
    {
        public EllipsisParameter(TokenSpan span)
            : base(span)
        {
        }
    }
}
