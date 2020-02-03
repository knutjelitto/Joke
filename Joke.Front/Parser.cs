﻿using System;
using System.Runtime.CompilerServices;

namespace Joke.Front
{
    public class Parser<T> where T : Scanner
    {
        protected readonly T scanner;

        public Parser(T scanner)
        {
            this.scanner = scanner;
        }

        public void Skip()
        {
            scanner.Skip();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char At()
        {
            return scanner.At();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char At(int offset)
        {
            return scanner.At(offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char MatchAny()
        {
            var match = scanner.At();
            if (scanner.Current < scanner.Limit)
            {
                scanner.Current += 1;
                return match;
            }
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char Match(char ch)
        {
            if (scanner.Match(ch))
            {
                var match = scanner.At();
                scanner.Current += 1;
                return match;
            }
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Match(string vs)
        {
            if (scanner.Check(vs))
            {
                scanner.Current += vs.Length;
                return;
            }
            throw new NotImplementedException();
        }

        public void MatchOne(string of)
        {
            EnsureMore();
            if (of.Contains(At()))
            {
                return;
            }
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckHexDigit()
        {
            var digit = At();
            return '0' <= digit && digit <= '9' || 'a' <= digit && digit <= 'f' || 'A' <= digit && digit <= 'F';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char MatchHexDigit()
        {
            if (CheckHexDigit())
            {
                return MatchAny();
            }
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CheckDigit()
        {
            var digit = At();
            return '0' <= digit && digit <= '9';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char MatchDigit()
        {
            if (CheckDigit())
            {
                return MatchAny();
            }
            throw new NotImplementedException();
        }

        public bool CheckOneOf(params char[] vs)
        {
            var ch = At();
            foreach (var v in vs)
            {
                if (v == ch)
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureMore()
        {
            scanner.EnsureMore();
        }

        public bool HaveMore()
        {
            return scanner.Current < scanner.Limit;
        }
    }
}
