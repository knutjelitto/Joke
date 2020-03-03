using System;

namespace Joke.Joke.Decoding
{
    public interface ISourceSpan
    {
        ISource Source { get; }
        int Offset { get; }
        int Length { get; }

        ReadOnlySpan<char> Value { get; }
    }
}