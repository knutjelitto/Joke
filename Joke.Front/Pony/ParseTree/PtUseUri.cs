using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public sealed class PtUseUri : PtUse
    {
        public PtUseUri(PonyTokenSpan span, PtIdentifier? name, PtString uri, PtGuard? guard)
            : base(span, name, guard)
        {
            Uri = uri;
        }

        public PtString Uri { get; }
    }
}
