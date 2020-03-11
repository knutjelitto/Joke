using System;
using System.Collections.Generic;
using System.Diagnostics;

using Joke.Joke.Err;
using Joke.Joke.Tree;

namespace Joke.Joke.Decoding
{
    public sealed partial class Parser
    {
        private void Begin()
        {
            markers.Push(next);
        }

        private bool IsBeginMatch(TK match)
        {
            if (Is(match))
            {
                BeginMatch(match);
                return true;
            }
            return false;
        }

        private void BeginMatch(TK match)
        {
            Begin();
            Match(match);
        }

        private TokenSpan End()
        {
            Debug.Assert(next <= limit);
            Debug.Assert(markers.Count > 0);
            return new TokenSpan(Tokens, markers.Pop(), next);
        }

        private T Scrap<T>(T what)
        {
            Debug.Assert(next <= limit);
            Debug.Assert(markers.Count > 0);
            markers.Pop();
            return what;
        }

        private T? Singularize<T>(IReadOnlyList<T> what) where T : class
        {
            if (what.Count == 1)
            {
                return Scrap(what[0]);
            }
            return null;
        }

        private void Inconclusive()
        {
            Debug.Assert(next <= limit);
            Debug.Assert(markers.Count > 0);
            next = markers.Pop();
        }

        private TokenSpan Mark(IAny node)
        {
            Debug.Assert(next <= limit);
            Debug.Assert(markers.Count > 0);
            return new TokenSpan(Tokens, node.Span.Start, next);
        }

        private bool Is(TK token)
        {
            return next < limit && Tokens[next].Kind == token;
        }

        private bool IsMatch(TK token)
        {
            if (next < limit && Tokens[next].Kind == token)
            {
                next += 1;
                return true;
            }
            return false;
        }

        private void MatchAny()
        {
            if (next < limit)
            {
                next += 1;
            }
            return;
        }

        private void Match(TK kind)
        {
            if (next < limit && Tokens[next].Kind == kind)
            {
                next += 1;
                return;
            }

            Errors.AtToken(ErrNo.Scan001, Current, $"unknown token in token stream, expected ``{Keywords.String(kind)}´´ but got ``{Keywords.String(Current.Kind)}´´");
            throw new NotImplementedException();
        }

        private IReadOnlyList<T> Collect<T>(Func<T> collect, TK token)
        {
            var list = new List<T>();

            while (true)
            {
                list.Add(collect());
                if (!IsMatch(token))
                {
                    break;
                }
            }

            return list;
        }

        private IReadOnlyList<T> Collect<T>(T first, Func<T?> collect) where T : class
        {
            var list = new List<T>() { first };

            while (true)
            {
                var item = collect();
                if (item != null)
                {
                    list.Add(item);
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        private IReadOnlyList<T> Collect<T>(Func<T?> collect) where T : class
        {
            var list = new List<T>();

            while (true)
            {
                var item = collect();
                if (item != null)
                {
                    list.Add(item);
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        private IReadOnlyList<T> CollectOptional<T>(Func<T?> collect, TK token) where T : class
        {
            var list = new List<T>();

            while (true)
            {
                var item = collect();
                if (item != null)
                {
                    list.Add(item);
                }
                else
                {
                    break;
                }
                if (!IsMatch(token))
                {
                    break;
                }
            }

            return list;
        }
    }
}
