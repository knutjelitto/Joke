using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtGroupedExpression : PtExpression
    {
        public PtGroupedExpression(PonyTokenSpan span, IReadOnlyList<PtExpression> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<PtExpression> Items { get; }
    }
}
