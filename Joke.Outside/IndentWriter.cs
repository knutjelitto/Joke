using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            this.tab = tab;
            prefix = string.Empty;
            current = null;
        }

        public IEnumerable<string> Lines => lines;

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
            if (body == null) throw new ArgumentNullException(nameof(body));

            using (Indent())
            {
                body();
            }
        }

        public void Indent(string head, Action body)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));

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
            current = null;
        }

        private void Add(string line)
        {
            Begin();
            current += line;
        }

        private void Begin()
        {
            if (current == null)
            {
                current = prefix;
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

        public void Dump(IWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }
}
