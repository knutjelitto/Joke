using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class DotIdentifier : Identifier
    {
        public DotIdentifier(PonyTokenSpan span, Identifier before, Identifier after)
            : base(span)
        {
            Before = before;
            After = after;
        }

        public Identifier Before { get; }
        public Identifier After { get; }
    }
}
