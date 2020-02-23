using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class IdsSingle : Ids
    {
        public IdsSingle(TokenSpan span, Identifier name)
            : base(span)
        {
        }
    }
}
