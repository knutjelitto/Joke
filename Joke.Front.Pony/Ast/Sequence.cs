using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Sequence : Expression
    {
        public Sequence(TSpan span, Expression first, Expression next)
            : base(span)
        {
            First = first;
            Next = next;
        }

        public Expression First { get; }
        public Expression Next { get; }
    }
}
