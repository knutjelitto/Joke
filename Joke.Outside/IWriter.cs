using System;

namespace Joke.Outside
{
    public interface IWriter
    {
        void Write(string text);

        void WriteLine(string text);

        void WriteLine();

        void Indent(Action body);
    }
}
