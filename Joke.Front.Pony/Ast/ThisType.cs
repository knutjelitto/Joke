using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class ThisType : Type
    {
        public ThisType(TokenSpan span)
            : base(span)
        {
        }
    }
}
