using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtPostfix : PtExpression
    {
        public PtPostfix(PonyTokenSpan span, PtExpression atom, IReadOnlyList<PtPostfixPart> parts)
            : base(span)
        {
            Atom = atom;
            Parts = parts;
        }

        public PtExpression Atom { get; }
        public IReadOnlyList<PtPostfixPart> Parts { get; }
    }
}
