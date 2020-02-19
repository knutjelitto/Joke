using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Object : Expression
    {
        public Object(TokenSpan span, Annotations? annotations, Cap? cap, Type? provides, Members members)
            : base(span)
        {
            Annotations = annotations;
            Cap = cap;
            Provides = provides;
            Members = members;
        }

        public Annotations? Annotations { get; }
        public Cap? Cap { get; }
        public Type? Provides { get; }
        public Members Members { get; }
    }
}
