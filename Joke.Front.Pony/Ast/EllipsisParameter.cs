using Joke.Front.Pony.Lexing;

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
