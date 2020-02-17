using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class Node
    {
        protected Node(TSpan span)
        {
            Span = span;
        }

        public TSpan Span { get; }

        protected void Check(IReadOnlyCollection<Node> nodes)
        {
        }

        public void CheckStart(Node other)
        {
            if (Span.Start != other.Span.Start)
            {
                throw Span.Parser.NoParse($"{this}.@{Span.Start} doesn't match {other}@{other.Span.Start}");
            }
        }
    }
}
