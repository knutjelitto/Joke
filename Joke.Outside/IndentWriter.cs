using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Joke.Outside
{
    public class IndentWriter : IWriter
    {
        private readonly List<string> lines;
        private readonly string tab;
        private string prefix;
        private string? current;

        public IndentWriter(string tab = "    ")
        {
            lines = new List<string>();
            Writer = null;
            this.tab = tab;
            prefix = string.Empty;
            current = null;
        }

        public IndentWriter(TextWriter writer, string tab = "    ")
        {
            lines = new List<string>();
            Writer = writer;
            this.tab = tab;
            prefix = string.Empty;
            current = null;
        }

        public IEnumerable<string> Lines => lines;

        public TextWriter? Writer { get; }

        public void WriteLine(string line)
        {
            AddLine(line);
        }

        public void WriteLine()
        {
            AddLine(string.Empty);
        }

        public void Write(string line)
        {
            Add(line);
        }

        public void Indent(Action body)
        {
            using (Indent())
            {
                body();
            }
        }

        public void Indent(string head, Action body)
        {
            AddLine(head);
            using (Indent())
            {
                body();
            }
        }

        public void AddLine(string line)
        {
            Begin();
            lines.Add(current + line);
            WL(line);
            current = null;
        }

        public void Dump(IWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }

        public IDisposable Indent()
        {
            var prevPrefix = prefix;
            prefix += tab;
            return new Disposable(() =>
            {
                prefix = prevPrefix;
            });
        }

        public int Extend()
        {
            return current == null ? 0 : current.Length;
        }

        public void Persist(string path)
        {
            while (true)
            {
                try
                {
                    File.WriteAllLines(path, lines);
                    return;
                }
                finally
                {
                    Thread.Sleep(42);
                }
            }
        }

        private void Add(string line)
        {
            Begin();
            current += line;
            W(line);
        }

        private void Begin()
        {
            if (current == null)
            {
                current = prefix;
                W(prefix);
            }
        }

        private void W(string text)
        {
            if (Writer != null)
            {
                Writer.Write(text);
            }
        }

        private void WL(string text)
        {
            if (Writer != null)
            {
                Writer.WriteLine(text);
            }
        }
    }
}
