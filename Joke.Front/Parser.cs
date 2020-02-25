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

        public char At()
        {
            return scanner.At();
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

        public bool IsHexDigit()
        {
            var digit = At();

            return '0' <= digit && digit <= '9' || 'a' <= digit && digit <= 'f' || 'A' <= digit && digit <= 'F';
        }

        public bool IsDigit()
        {
            var digit = At();

            return '0' <= digit && digit <= '9';
        }
    }
}
