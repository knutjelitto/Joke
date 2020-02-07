using System;

namespace Joke.Front
{
    public class Parser<T> where T : Scanner
    {
        protected readonly T scanner;

        public Parser(T scanner)
        {
            this.scanner = scanner;
        }

        public void Eat(int n = 1)
        {
            scanner.Eat(n);
        }

        public void Skip()
        {
            scanner.Skip();
        }

        public bool SkipMatch(char ch)
        {
            scanner.Skip();
            if (scanner.Current < scanner.Limit && scanner.At() == ch)
            {
                scanner.Current += 1;
                return true;
            }
            return false;
        }

        public bool SkipMatch(string str)
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

        public void Match()
        {
            if (scanner.Current < scanner.Limit)
            {
                scanner.Current += 1;
            }
        }

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

        public bool CheckHexDigit()
        {
            var digit = At();
            return '0' <= digit && digit <= '9' || 'a' <= digit && digit <= 'f' || 'A' <= digit && digit <= 'F';
        }

        public char MatchHexDigit()
        {
            if (CheckHexDigit())
            {
                return MatchAny();
            }
            throw new NotImplementedException();
        }

        public bool CheckDigit()
        {
            var digit = At();
            return '0' <= digit && digit <= '9';
        }

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

        public bool Check(char ch)
        {
            return At() == ch;
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
