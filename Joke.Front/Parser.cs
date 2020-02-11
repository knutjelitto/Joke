using System;
using System.Diagnostics;

namespace Joke.Front
{
    public class Parser<T> where T : Scanner
    {
        protected readonly T scanner;

        public Parser(T scanner)
        {
            this.scanner = scanner;
        }

        public bool Eat(int n)
        {
            return scanner.Eat(n);
        }

        public bool Eat()
        {
            return scanner.Eat(1);
        }

        public bool Skip()
        {
            return scanner.Skip();
        }

        public void SkipMatch(char ch)
        {
            Debug.Assert(ch != Scanner.NoCharacter);

            scanner.Skip();
            Match(ch);
        }

        public bool TryMatch(char ch)
        {
            Debug.Assert(ch != Scanner.NoCharacter);

            if (At() == ch)
            {
                scanner.Current += 1;

                return true;
            }
            return false;
        }

        public bool TrySkipMatch(char ch)
        {
            Debug.Assert(ch != Scanner.NoCharacter);

            scanner.Skip();
            if (At() == ch)
            {
                scanner.Current += 1;

                return true;
            }
            return false;
        }

        public bool TryMatch(string str)
        {
            if (scanner.Check(str))
            {
                scanner.Current += str.Length;
                return true;
            }

            return false;
        }

        public bool TrySkipMatch(string str)
        {
            scanner.Skip();
            if (scanner.Check(str))
            {
                scanner.Current += str.Length;
                return true;
            }

            return false;
        }

        public char At()
        {
            return scanner.At();
        }

        public char At(int offset)
        {
            return scanner.At(offset);
        }

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

        public char Match(char ch)
        {
            Debug.Assert(ch != Scanner.NoCharacter);
            if (At() == ch)
            {
                scanner.Current += 1;
                return ch;
            }
            throw new NotImplementedException();
        }

        protected SourceSpan Span(int start)
        {
            return scanner.Span(start);
        }

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

        public int GetStart()
        {
            return scanner.Current;
        }

        public void SetStart(int start)
        {
            scanner.Current = start;
        }

        public bool More()
        {
            return scanner.Current < scanner.Limit;
        }

        public bool IsHexDigit()
        {
            var digit = At();

            return '0' <= digit && digit <= '9' || 'a' <= digit && digit <= 'f' || 'A' <= digit && digit <= 'F';
        }

        public char MatchHexDigit()
        {
            if (IsHexDigit())
            {
                return MatchAny();
            }

            throw new NotImplementedException();
        }

        public bool IsDigit()
        {
            var digit = At();

            return '0' <= digit && digit <= '9';
        }

        public char MatchDigit()
        {
            if (IsDigit())
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

        public bool Check(char ch)
        {
            return scanner.Check(ch);
        }

        public bool Check(string str)
        {
            return scanner.Check(str);
        }

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
