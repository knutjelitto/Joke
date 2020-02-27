using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtWithElement : PtNode
    {
        public PtWithElement(PonyTokenSpan span, PtIds names, PtExpression initializer)
            : base(span)
        {
            Names = names;
            Initializer = initializer;
        }

        public PtIds Names { get; }
        public PtExpression Initializer { get; }
    }
}
