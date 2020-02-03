using System;
using System.Text;

namespace Joke.Outside.Build
{
    public class Runner
    {
        public Capture Run(string description, string program, string arguments, params (string name, string value)[] environment)
        {
            Console.WriteLine(description);
            var capture = MakeProcess(program, arguments, environment);

            RunProcess(capture);

            return capture;
        }

        protected Capture MakeProcess(string image, string arguments, params (string name, string value)[] environment)
        {
            var capture = new Capture();
            var process = capture.Process;

            foreach (var (name, value) in environment)
            {
                process.StartInfo.Environment[name] = value;
            }

            process.StartInfo.FileName = image;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;

            process.EnableRaisingEvents = true;

            process.OutputDataReceived += (s, e) =>
            {
                capture.AddOut(e.Data);
            };

            process.ErrorDataReceived += (s, e) =>
            {
                capture.AddErr(e.Data);
            };

            return capture;
        }

        protected void RunProcess(Capture capture)
        {
            var process = capture.Process;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            capture.Done();
        }
    }
}
