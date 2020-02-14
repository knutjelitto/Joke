using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class UseUri : Use
    {
        public UseUri(TSpan span, Identifier? name, String uri)
            : base(span, name)
        {
            Uri = uri;
        }

        public String Uri { get; }
    }
}
