using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Literal : Expression
    {
        public Literal(TSpan span) : base(span)
        {
        }

        public override string? ToString()
        {
            return Span.ToString();
        }
    }
}
