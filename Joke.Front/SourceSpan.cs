using System;

namespace Joke.Front
{
    public struct SourceSpan : ISourceSpan
    {
        public SourceSpan(ISource source, int start, int length)
        {
            Source = source;
            Start = start;
            Length = length;
        }

        public readonly ISource Source;
        public int Start { get; }
        public readonly int Length;

        public ReadOnlySpan<char> Value => Source.Content.AsSpan(Start, Length);

        public override string ToString()
        {
            return Source.Content.Substring(Start, Length);
        }
    }
}
