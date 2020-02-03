using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Outside.Build
{
    public sealed class Capture
    {
        private readonly bool verbose = true;

        public Capture()
            : this(new Process(), -1)
        {
        }

        private Capture(Process process, int exitCode)
        {
            Process = process;
            ExitCode = exitCode;
        }

        public Process Process { get; private set; }
        public int ExitCode { get; set; }
        public List<IoLine> StdOut { get; private set; } = new List<IoLine>();
        public List<IoLine> StdErr { get; } = new List<IoLine>();

        public void AddOut(string text)
        {
            Add(StdOut, IoLine.Out(text));
        }

        public void AddErr(string text)
        {
            Add(StdErr, IoLine.Err(text));
        }

        private void Add(List<IoLine> lines, IoLine line)
        {
            if (lines.Count > 0 || !string.IsNullOrEmpty(line.Text))
            {
                if (verbose) Console.WriteLine(line.ToString());
                lines.Add(line);
            }
        }

        public bool IsOk => ExitCode == 0;

        //public static implicit operator bool(Capture capture) => capture.Ok;

        public void Done()
        {
            ExitCode = Process.ExitCode;
            Process.Close();
        }
    }
}
