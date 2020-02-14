using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony
{
    partial class PonyParser
    {
        private readonly ISource source;
        private readonly IReadOnlyList<Token> toks;

        private int next;
        private readonly int limit;

        private TK Kind => toks[next].Kind;

        private string Current
        {
            get
            {
                if (next < limit)
                {
                    return $"[{Kind}]";
                }
                return "[EOF>>]";
            }
        }

        private bool MayMatch(TK kind)
        {
            if (next < limit && toks[next].Kind == kind)
            {
                next += 1;
                return true;
            }

            return false;
        }

        private void Match(string fail, TK kind)
        {
            if (next < limit && toks[next].Kind == kind)
            {
                next += 1;
                return;
            }

            throw NoParse($"{fail} expected");
        }

        private void Match(string fail, params TK[] kinds)
        {
            if (next < limit)
            {
                for (var i = 0; i < kinds.Length; ++i)
                {
                    if (kinds[i] == toks[next].Kind)
                    {
                        next += 1;
                        return;
                    }
                }
            }

            throw NoParse($"{fail} expected");
        }

        private bool Iss(TK kind)
        {
            return next < limit && toks[next].Kind == kind;
        }

        private bool Iss(params TK[] kinds)
        {
            if (next < limit)
            {
                for (var i = 0; i < kinds.Length; ++i)
                {
                    if (kinds[i] == toks[next].Kind)
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
                    if (kinds[i] == toks[next].Kind)
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

        private void Next()
        {
            if (next < limit)
            {
                next += 1;
            }
        }

        private void Match()
        {
            if (next < limit)
            {
                next += 1;
                return;
            }

            throw NoParse("expected something (not EOF)");
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
