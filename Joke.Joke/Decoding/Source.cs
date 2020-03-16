using Joke.Joke.Tools;
using System;
using System.Collections.Generic;

namespace Joke.Joke.Decoding
{
    public class Source : ISource
    {
        private static readonly string lineEndings = "\u000A\u000B\u000C\u000D\u0085\u2028\u2029";
        private static readonly string otherLineEndings = "\u000B\u000C\u0085\u2028\u2029";

        private readonly Lazy<List<int>> lazyLines;

        private Source(string fullName, string name, string content)
        {
            FullName = fullName;
            Name = name;
            Content = content;

            lazyLines = new Lazy<List<int>>(() =>
            {
                return FindLines();
            });
        }

        public string Content { get; }
        public string FullName { get; }
        public string Name { get; }
        public int Length => Content.Length;
        protected List<int> Lines => lazyLines.Value;
        public int LineCount => Lines.Count;
        public int this[int index] => Content[index];
        public bool AtEnd(int index) => index >= Content.Length;

        
        public static Source FromFile(FileRef file, string name)
        {
            return new Source(file, name, file.GetContent());
        }

        public (int lineNo, int colNo) GetLineCol(int index)
        {
            var lineNo = GetLineNoFromIndex(index);
            var colNo = index - GetIndexFromLineNo(lineNo) + 1;

            return (lineNo, colNo);
        }

        public string GetLine(int lineNo)
        {
            if (lineNo == LineCount)
            {
                var start1 = GetIndexFromLineNo(lineNo);
                var next1 = GetIndexFromLineNo(lineNo + 1);
                return GetSpan(start1, next1 - start1);
            }
            var start = GetIndexFromLineNo(lineNo);
            var next = GetIndexFromLineNo(lineNo + 1);
            while (next > start && lineEndings.Contains(Content[next - 1]))
            {
                next -= 1;
            }

            return GetSpan(start, next - start);
        }

        public IEnumerable<string> GetLines()
        {
            for (var lineNo = 1; lineNo <= LineCount; ++lineNo)
            {
                yield return GetLine(lineNo);
            }
        }

        public int GetIndexFromLineNo(int lineNo)
        {
            if (lineNo > LineCount)
            {
                return Content.Length;
            }

            var lineIdx = Math.Max(0, Math.Min(lineNo - 1, Lines.Count - 1));
            return Lines[lineIdx];
        }

        public int GetLineNoFromIndex(int index)
        {
            var line = Lines.BinarySearch(Math.Max(0, index));
            if (line < 0)
            {
                return ~line;
            }
            return line + 1;
        }

        public string GetSpan(int start, int length)
        {
            return Content.Substring(start, length);
        }

        public string GetText(int start, int length)
        {
            var end = Math.Min(Length, start + length);
            return new string(Content.AsSpan(start, end - start));
        }

        private List<int> FindLines()
        {
            var lines = new List<int>(10000);
            lines.Add(0);
            var c2 = '\0';
            for (int i = 0; i != Content.Length; i++)
            {
                var c1 = c2;
                c2 = Content[i];

                // is c1 c2 a line ending sequence?
                if (IsLineEnding(c1, c2))
                {
                    // are we late to detect MacOS style?
                    if (c1 == '\u000D' && c2 != '\u000A')
                        AddLine(i);
                    else
                        AddLine(i + 1);
                }
            }

            bool IsLineEnding(char c1, char c2)
            {
                return otherLineEndings.Contains(c2) || c1 == '\u000D' || c2 == '\u000A';
            }

            void AddLine(int index)
            {
                if (lines.Count >= lines.Capacity)
                {
                    lines.Capacity = (lines.Capacity * 3) / 2;
                }

                lines.Add(index);
            }

            return lines;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}
