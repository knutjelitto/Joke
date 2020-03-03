using Joke.Joke.Decoding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Joke.Tree
{
    public class Primitive : IMember
    {
        public Primitive(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
