using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Term : Expression
    {
        public Term(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
