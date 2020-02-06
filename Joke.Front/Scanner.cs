using System;
using System.Runtime.CompilerServices;

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

        public SourceSpan Span(int start)
        {
            return new SourceSpan(source, start, Current - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip()
        {
            while (Current < Limit && CanSkip())
            {
                Current += 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanSkip()
        {
            return
                content[Current] == '\t' ||
                content[Current] == '\n' ||
                content[Current] == '\r' ||
                content[Current] == ' ';
        }

        public void Eat(int n)
        {
            while (n > 0 && Current < Limit)
            {
                Current += 1;
                n -= 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Match(char what)
        {
            return Current < Limit && content[Current] == what;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Check(string what)
        {
            return content.AsSpan(Current, what.Length).Equals(what.AsSpan(), StringComparison.Ordinal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char At()
        {
            return Current < Limit ? content[Current] : NoCharacter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char At(int offset)
        {
            return Current + offset < Limit ? content[Current + offset] : NoCharacter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
