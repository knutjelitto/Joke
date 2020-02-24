using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Float : Expression
    {
        public Float(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
