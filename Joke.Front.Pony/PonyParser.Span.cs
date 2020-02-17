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

        private void Begin(TK kind)
        {
            marks.Push(next);
            Match(kind);
        }

        private void Begin(params TK[] kinds)
        {
            Debug.Assert(kinds.Length >= 2);

            marks.Push(next);
            Match(kinds);
        }

        private bool MayBegin(params TK[] kinds)
        {
            if (More() && System.Array.IndexOf(kinds, TokenKind) >= 0)
            {
                marks.Push(next);
                next += 1;
                return true;
            }
            return false;
        }

        private TSpan End()
        {
            Debug.Assert(next <= limit);
            Debug.Assert(marks.Count > 0);
            return new TSpan(this, marks.Pop(), next);
        }

        private int Discard()
        {
            return marks.Pop();
        }
    }
}
