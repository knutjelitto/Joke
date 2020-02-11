using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony
{
    public class PonyParserBase : Parser<PonyScanner>
    {
        private static string punctuation = ".,(){}|";
        public PonyParserBase(PonyScanner scanner)
            : base(scanner)
        {
        }

        /*
         * Matching
         */
        protected bool TryMatchPunctuation(char ch)
        {
            Debug.Assert(punctuation.Contains(ch));

            if (At(0) == ch)
            {
                Eat(1);
                return true;
            }

            return false;
        }

        protected void MatchPunctuation(char ch)
        {
            Debug.Assert(punctuation.Contains(ch));

            if (At(0) == ch)
            {
                Eat(1);
                return;
            }

            throw NoParse($"punctuation '{ch}'");
        }

        protected void SkipMatchPunctuation(char ch)
        {
            Skip();
            MatchPunctuation(ch);
        }

        protected bool IsPunctuation(char ch)
        {
            Debug.Assert(punctuation.Contains(ch));

            if (At(0) == ch)
            {
                return true;
            }

            return false;
        }

        private (int, string) IdAlike()
        {
            Skip();
            var start = GetStart();
            if (scanner.IsLetter_())
            {
                Eat(1);
                while (scanner.IsLetterOrDigit_())
                {
                    Eat(1);
                }
            }

            return (start, Span(start).ToString());
        }

        protected void MatchKeyword(string keyword)
        {
            var (start, prefix) = IdAlike();

            if (prefix != keyword)
            {
                SetStart(start);

                throw new NotImplementedException();
            }
        }

        protected bool IsKeyword(string keyword)
        {
            var (start, prefix) = IdAlike();

            Debug.Assert(Keywords.IsKeyword(keyword));

            var check = prefix == keyword;

            SetStart(start);

            return check;
        }

        protected bool TryMatchKeyword(string keyword)
        {
            var (start, prefix) = IdAlike();

            if (prefix == keyword)
            {
                return true;
            }

            SetStart(start);

            return false;
        }

        protected T Iff<T>(bool iff, Func<T> then, T @else = default)
        {
            if (iff)
            {
                return then();
            }

            return @else;
        }

        private T SkipIff<T>(bool iff, Func<T> then, T @else = default)
        {
            Skip();
            if (iff)
            {
                return then();
            }

            return @else;
        }

        protected bool SkipIff(char ch)
        {
            return SkipIff(At() == ch, Eat, false);
        }



        /*
         * Errros
         */
        protected NotYetException NotYet(string message)
        {
            return new NotYetException(message);
        }

        protected NoParseException NoParse(string message)
        {
            return new NoParseException(message);
        }

    }
}
