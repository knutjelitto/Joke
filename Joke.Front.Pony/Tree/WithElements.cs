using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class WithElements : Node
    {
        public WithElements(TSpan span, IReadOnlyList<WithElement> elements)
            : base(span)
        {
            Check(elements);
            Elements = elements;
        }

        public IReadOnlyList<WithElement> Elements { get; }
    }
}
