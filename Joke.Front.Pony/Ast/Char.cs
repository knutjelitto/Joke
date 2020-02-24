using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Char : Expression
    {
        public Char(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
