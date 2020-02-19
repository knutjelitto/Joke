﻿using System.Collections.Generic;
using System.Diagnostics;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        public Source Source { get; }
        public IReadOnlyList<Token> Tokens { get; }

        private int next;
        private readonly int limit;

        private TK TokenKind => next < limit ? Tokens[next].Kind : TK.Missing;
        private Token Token => next < limit ? Tokens[next] : new Token(TK.Missing, 0, 0, 0);

        private string Current
        {
            get
            {
                if (next < limit)
                {
                    return $"[{TokenKind}]";
                }
                return "[EOF>>]";
            }
        }

        private bool MayMatch(TK kind)
        {
            if (next < limit && Tokens[next].Kind == kind)
            {
                next += 1;
                return true;
            }

            return false;
        }

        private void Match(TK kind)
        {
            if (next < limit && Tokens[next].Kind == kind)
            {
                next += 1;
                return;
            }

            var fail = Keywords.String(kind);

            throw NoParse($"{fail} expected");
        }

        private void Match(params TK[] kinds)
        {
            Debug.Assert(kinds.Length >= 2);

            var ok = false;
            if (next < limit)
            {
                foreach (var kind in kinds)
                {
                    if (Tokens[next].Kind == kind)
                    {
                        ok = true;
                        break;
                    }
                }
            }

            if (!ok)
            {
                throw NoParse("expected something (not EOF)");
            }
            next += 1;
        }

        private bool Iss(TK kind)
        {
            return next < limit && Tokens[next].Kind == kind;
        }

        private bool Iss(params TK[] kinds)
        {
            if (next < limit)
            {
                for (var i = 0; i < kinds.Length; ++i)
                {
                    if (kinds[i] == Tokens[next].Kind)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool Issnt(params TK[] kinds)
        {
            if (next < limit)
            {
                for (var i = 0; i < kinds.Length; ++i)
                {
                    if (kinds[i] == Tokens[next].Kind)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private bool More()
        {
            return next < limit;
        }

        private void Ensure()
        {
            if (next >= limit)
            {
                throw NoParse("expected something (not EOF)");
            }
        }
    }
}
