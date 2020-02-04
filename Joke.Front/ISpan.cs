using System;

namespace Joke.Front
{
    public interface ISpan
    {
        public int Start { get; }
        ReadOnlySpan<char> Value { get; }
    }
}