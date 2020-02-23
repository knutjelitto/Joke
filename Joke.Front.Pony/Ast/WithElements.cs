using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class WithElements : Node
    {
        public WithElements(TokenSpan span, IReadOnlyList<WithElement> elements)
            : base(span)
        {
            Elements = elements;
        }

        public IReadOnlyList<WithElement> Elements { get; }
    }
}
