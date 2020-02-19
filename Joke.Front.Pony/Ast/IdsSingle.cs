using Joke.Front.Pony.Lex;

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
