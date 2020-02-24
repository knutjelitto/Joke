using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Identifier : Literal
    {
        public Identifier(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
