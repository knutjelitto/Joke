using System.Diagnostics;

namespace Joke.Joke.Decoding
{
    public struct Token : IToken
    {
        public Token(TK kind, ISource source, int clutter, int payload, int next, bool nl)
        {
            Debug.Assert(clutter <= payload);
            Debug.Assert(payload <= next);

            Kind = kind;
            Source = source;
            ClutterOffset = clutter;
            PayloadOffset = payload;
            NextOffset = next;
            Nl = nl;
        }

        public readonly TK Kind;

        public ISourceSpan PayloadSpan => new SourceSpan(Source, PayloadOffset, PayloadLength);

        public ISource Source { get; }
        public int ClutterOffset { get; }
        public int PayloadOffset { get; }
        public int NextOffset { get; }
        public bool Nl { get; }
        public int ClutterLength => PayloadOffset - ClutterOffset;
        public int PayloadLength => NextOffset - PayloadOffset;

        public string Clutter => Source.Content.Substring(ClutterOffset, ClutterLength);
        public string Payload => Source.Content.Substring(PayloadOffset, PayloadLength);

        public override string ToString()
        {
            return $"[{Kind}/{Payload}]";
        }
    }
}
