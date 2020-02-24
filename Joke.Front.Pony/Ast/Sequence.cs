using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Sequence : Expression
    {
        public Sequence(PonyTokenSpan span, Expression first, Expression next)
            : base(span)
        {
            First = first;
            Next = next;
        }

        public Expression First { get; }
        public Expression Next { get; }
    }
}
