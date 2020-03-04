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
            Clutter = clutter;
            Payload = payload;
            Next = next;
            Nl = nl;
        }

        public readonly TK Kind;

        public ISourceSpan PayloadSpan => new SourceSpan(Source, Payload, Next - Payload);

        public ISource Source { get; }
        public int Clutter { get; }
        public int Payload { get; }
        public int Next { get; }
        public bool Nl { get; }

        public string GetClutter()
        {
            return Source.GetText(Clutter, Payload - Clutter);
        }

        public string GetPayload()
        {
            return Source.GetText(Payload, Next - Payload);
        }

        public override string ToString()
        {
            return $"[{Kind}/{GetPayload()}]";
        }
    }
}
