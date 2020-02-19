﻿using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lex;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Front.Pony.Syntax
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

        private void Begin(TokenSet set)
        {
            if (next < limit)
            {
                marks.Push(next);
            }
            Match(set);
        }

        private void Match(TokenSet set)
        {
            if (next < limit && set[Tokens[next].Kind])
            {
                next += 1;
                return;
            }

            throw NoParse("expected something (not EOF)");
        }

        private void Begin(params TK[] kinds)
        {
            Debug.Assert(kinds.Length >= 2);

            marks.Push(next);
            Match(kinds);
        }

        private bool MayBegin(params TK[] kinds)
        {
            if (System.Array.IndexOf(kinds, TokenKind) >= 0)
            {
                marks.Push(next);
                next += 1;
                return true;
            }
            return false;
        }

        private TokenSpan End()
        {
            Debug.Assert(next <= limit);
            Debug.Assert(marks.Count > 0);
            return new TokenSpan(this, marks.Pop(), next);
        }

        private TokenSpan Mark(Node node)
        {
            Debug.Assert(next <= limit);
            Debug.Assert(marks.Count > 0);
            return new TokenSpan(this, node.Span.Start, next);
        }

        private void Drop()
        {
            marks.Pop();
        }
    }
}
