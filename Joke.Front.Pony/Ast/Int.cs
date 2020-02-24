using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Int : Literal
    {
        public Int(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
