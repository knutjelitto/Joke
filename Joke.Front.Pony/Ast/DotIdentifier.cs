using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class DotIdentifier : Identifier
    {
        public DotIdentifier(TokenSpan span, Identifier before, Identifier after)
            : base(span)
        {
            Before = before;
            After = after;
        }

        public Identifier Before { get; }
        public Identifier After { get; }
    }
}
