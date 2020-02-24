using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Recover : Expression
    {
        public Recover(PonyTokenSpan span, Annotations? annotations, Cap? cap, Expression body) : base(span)
        {
            Annotations = annotations;
            Cap = cap;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Cap? Cap { get; }
        public Expression Body { get; }
    }
}
