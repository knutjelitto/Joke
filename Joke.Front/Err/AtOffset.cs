﻿using System;
using System.IO;

namespace Joke.Front.Err
{
    public class AtOffset : IDescription
    {
        public AtOffset(ISourceSpan span, string msg)
        {
            Span = span;
            Msg = msg;
        }

        public ISource Source => Span.Source;
        public int Start => Span.Start;
        public int Length => Span.Length;
        public ISourceSpan Span { get; }
        public string Msg { get; }

        public virtual void Describe(TextWriter writer)
        {
            var (line, col) = Source.GetLineCol(Start);
            var msg = string.IsNullOrWhiteSpace(Msg) ? string.Empty : $"{Msg}";
            writer.WriteLine($"({line},{col}): can't continue -- {msg}");
            var arrow = new string('-', col - 1) + new string('^', Math.Max(1, Length));
            if (line > 2) Console.WriteLine($" |{Source.GetLine(line - 2).ToString()}");
            if (line > 1) Console.WriteLine($" |{Source.GetLine(line - 1).ToString()}");
            writer.WriteLine($" |{Source.GetLine(line).ToString()}");
            writer.WriteLine($" |{arrow} {msg}");
            writer.WriteLine($" |{Source.GetLine(line + 1).ToString()}");
        }
    }
}
