using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Assignment : Expression
    {
        public Assignment(TokenSpan span, Expression left, Expression right)
            : base(span)
        {
            Left = left;
            Right = right;
        }

        public Expression Left { get; }
        public Expression Right { get; }
    }
}
