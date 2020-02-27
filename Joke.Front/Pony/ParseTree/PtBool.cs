using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtBool : PtLiteral<bool>
    {
        public PtBool(PonyTokenSpan span, bool value)
            : base(span, value)
        {
        }
    }
}
