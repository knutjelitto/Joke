using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Case : Node
    {
        public Case(TokenSpan span, Annotations? annotations, Expression? pattern, Expression? guard, Expression? body)
            : base(span)
        {
            Annotations = annotations;
            Pattern = pattern;
            Guard = guard;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Expression? Pattern { get; }
        public Expression? Guard { get; }
        public Expression? Body { get; }
    }
}
