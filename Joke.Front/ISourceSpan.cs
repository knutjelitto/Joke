using System;

namespace Joke.Front
{
    public interface ISourceSpan
    {
        ISource Source { get; }
        int Start { get; }
        int Length { get; }

        ReadOnlySpan<char> Value { get; }
    }
}