using System;

namespace Joke.Joke.Decoding
{
    public struct SourceSpan : ISourceSpan
    {
        public SourceSpan(ISource source, int offset, int length)
        {
            Source = source;
            Offset = offset;
            Length = length;
        }

        public ISource Source { get; }
        public int Offset { get; }
        public int Length { get; }

        public ReadOnlySpan<char> Value => Source.Content.AsSpan(Offset, Length);

        public override string ToString()
        {
            return Source.Content.Substring(Offset, Length);
        }
    }
}
