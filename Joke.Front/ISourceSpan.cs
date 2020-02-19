using System;

namespace Joke.Front
{
    public interface ISourceSpan
    {
        public int Start { get; }
        ReadOnlySpan<char> Value { get; }
    }
}