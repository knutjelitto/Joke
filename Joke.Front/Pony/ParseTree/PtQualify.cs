using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtQualify : PtPostfixPart
    {
        public PtQualify(PonyTokenSpan span, PtTypeArguments arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public PtTypeArguments Arguments { get; }
    }
}
