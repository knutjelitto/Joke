using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class WithElements : Node
    {
        public WithElements(TSpan span, IReadOnlyList<WithElement> elements)
            : base(span)
        {
            Elements = elements;
        }

        public IReadOnlyList<WithElement> Elements { get; }
    }
}
