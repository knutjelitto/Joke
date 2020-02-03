using System;

namespace Joke.Front
{
    public interface ISpan
    {
        ReadOnlySpan<char> Value { get; }
    }
}