using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Tree
{
    public class Dot : Expression
    {
        public Dot(TSpan span, Expression left, Identifier member)
            : base(span)
        {
            Left = left;
            Member = member;
        }

        public Expression Left { get; }
        public Identifier Member { get; }
    }
}
