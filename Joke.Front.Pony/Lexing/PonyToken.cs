using System.Diagnostics;

namespace Joke.Front.Pony.Lexing
{
    public struct PonyToken : IToken
    {
        public PonyToken(TK kind, ISource source, int clutter, int payload, int next)
        {
            Debug.Assert(clutter <= payload);
            Debug.Assert(payload <= next);

            Kind = kind;
            Source = source;
            Clutter = clutter;
            Payload = payload;
            Next = next;
        }

        public readonly TK Kind;
        public readonly int Clutter;
        public readonly int Payload;
        public readonly int Next;

        public ISourceSpan PayloadSpan => new SourceSpan(Source, Payload, Next - Payload);

        public ISource Source { get; }

        public string GetClutter()
        {
            return Source.GetText(Clutter, Payload - Clutter);
        }

        public string GetPayload()
        {
            return Source.GetText(Payload, Next - Payload);
        }
    }
}
