using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Keyword : Base
    {
        public Keyword(ISpan span)
            : base(span)
        {
        }

        public bool Is(string kw)
        {
            return Span.Value.Equals(kw.AsSpan(), StringComparison.Ordinal);
        }
    }
}
