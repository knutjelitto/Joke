using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Else : Expression
    {
        public Else(TokenSpan span, Annotations? annotations, Expression body)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Expression Body { get; }
    }
}
