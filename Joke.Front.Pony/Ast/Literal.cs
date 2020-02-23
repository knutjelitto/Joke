using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Literal : Expression
    {
        public Literal(TokenSpan span) : base(span)
        {
        }

        public override string? ToString()
        {
            return Span.ToString();
        }
    }
}
