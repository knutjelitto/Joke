using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony
{
    partial class PonyParser
    {
        private readonly Stack<int> marks = new Stack<int>();

        private void Begin()
        {
            marks.Push(next);
        }

        private TSpan End()
        {
            Debug.Assert(next <= limit);

            return new TSpan(toks, marks.Pop(), next);
        }

        private TSpan Span(int start)
        {
            Debug.Assert(next <= limit);

            return new TSpan(toks, start, next);
        }
    }
}
