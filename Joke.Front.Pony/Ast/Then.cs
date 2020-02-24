using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Then : Expression
    {
        public Then(PonyTokenSpan span, Annotations? annotations, Expression body)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Expression Body { get; }
    }
}
