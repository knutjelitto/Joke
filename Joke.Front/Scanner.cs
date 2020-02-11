using System;

namespace Joke.Front
{
    public abstract class Scanner
    {
        public const char NoCharacter = '\uFFFF';

        private readonly ISource source;
        protected readonly string content;
        public int Current;
        public int Limit;

        public Scanner(ISource source)
        {
            this.source = source;
            content = source.Content;
            Current = 0;
            Limit = this.content.Length;
        }

        public int Line
        {
            get
            {
                var (line, _) = source.GetLineCol(Current);

                return line;
            }
        }

        public int Col
        {
            get
            {
                var (_, col) = source.GetLineCol(Current);

                return col;
            }
        }

        public SourceSpan Span(int start)
        {
            return new SourceSpan(source, start, Current - start);
        }

        public bool Skip()
        {
            var done = false;
            var nl = false;
            while (Current < Limit && !done)
            {
                switch (content[Current])
                {
                    case '\n':
                        nl = true;
                        Current += 1;
                        break;
                    case '\t':
                    case '\r':
                    case ' ':
                        Current += 1;
                        break;
                    case '/':
                        if (Current + 1 < Limit)
                        {
                            if (content[Current + 1] == '/')
                            {
                                Current += 2;
                                while (Current < Limit && content[Current] != '\n')
                                {
                                    Current += 1;
                                }
                                break;
                            }
                            if (content[Current + 1] == '*')
                            {
                                Current += 2;
                                while (Current + 1 < Limit && (content[Current] != '*' || content[Current+1] != '/'))
                                {
                                    Current += 1;
                                }
                                if (Current + 1 < Limit)
                                {
                                    Current += 2;
                                }
                                break;
                            }
                        }
                            done = true;
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return nl;
        }

        public virtual bool CanSkip()
        {
            if (Current < Limit)
            {
                switch (At())
                {
                    case '\t':
                    case '\n':
                    case '\r':
                    case ' ':
                        return true;
                    case '/':
                        return At(1) == '/';
                }
            }

            return false;
        }

        public bool Eat(int n)
        {
            while (n > 0 && Current < Limit)
            {
                Current += 1;
                n -= 1;
            }

            return n == 0;
        }

        public bool Check(char what)
        {
            return Current < Limit && content[Current] == what;
        }

        public bool TryMatch(string what)
        {
            return
                Current + what.Length <= Limit &&
                content.AsSpan(Current, what.Length)
                .Equals(what.AsSpan(), StringComparison.Ordinal);
        }

        public char At()
        {
            return Current < Limit ? content[Current] : NoCharacter;
        }

        public char At(int offset)
        {
            return Current + offset < Limit ? content[Current + offset] : NoCharacter;
        }

        public void EnsureMore()
        {
            if (Current < Limit)
            {
                return;
            }
            throw new NotImplementedException();
        }

        public bool IsLetter()
        {
            var ch = At();
            return
                'a' <= ch && ch <= 'z' ||
                'A' <= ch && ch <= 'Z';
        }

        public bool IsLetter_()
        {
            var ch = At();
            return
                'a' <= ch && ch <= 'z' ||
                'A' <= ch && ch <= 'Z' ||
                '_' == ch;
        }

        public bool IsDigit()
        {
            var ch = At();
            return
                '0' <= ch && ch <= '9';
        }

        public bool IsLetterOrDigit()
        {
            var ch = At();
            return
                '0' <= ch && ch <= '9' ||
                'a' <= ch && ch <= 'z' ||
                'A' <= ch && ch <= 'Z';
        }

        public bool IsLetterOrDigit_()
        {
            var ch = At();
            return
                '0' <= ch && ch <= '9' ||
                'a' <= ch && ch <= 'z' ||
                'A' <= ch && ch <= 'Z' ||
                '_' == ch;
        }

        public string Next
        {
            get
            {
                if (Current < Limit)
                {
                    return content.Substring(Current, Math.Min(Limit-Current, 20));
                }
                return string.Empty;
            }
        }
    }
}
