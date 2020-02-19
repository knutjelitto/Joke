using System;
using System.IO;

namespace Joke.Front.Pony.Err
{
    public class AtOffset : IVisualize
    {
        public AtOffset(Source source, int offset, string msg)
        {
            Source = source;
            Offset = offset;
            Msg = msg;
        }

        public Source Source { get; }
        public int Offset { get; }
        public string Msg { get; }

        public virtual void Visualize(TextWriter writer)
        {
            var (line, col) = Source.GetLineCol(Offset);
            var msg = string.IsNullOrWhiteSpace(Msg) ? string.Empty : $"{Msg}";
            writer.WriteLine($"({line},{col}): can't continue -- {msg}");
            var arrow = new string('-', col - 1) + "^";
            if (line > 2) Console.WriteLine($" |{Source.GetLine(line - 2).ToString()}");
            if (line > 1) Console.WriteLine($" |{Source.GetLine(line - 1).ToString()}");
            writer.WriteLine($" |{Source.GetLine(line).ToString()}");
            writer.WriteLine($" |{arrow} {msg}");
            writer.WriteLine($" |{Source.GetLine(line + 1).ToString()}");
        }
    }
}
