using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtUse : PtNode
    {
        public PtUse(PonyTokenSpan span, PtIdentifier? name, PtGuard? guard)
            : base(span)
        {
            Name = name;
            Guard = guard;
        }

        public PtIdentifier? Name { get; }
        public PtGuard? Guard { get; }
    }
}
