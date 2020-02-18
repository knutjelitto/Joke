using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Else : Expression
    {
        public Else(TSpan span, Annotations? annotations, Expression body)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Expression Body { get; }
    }
}
