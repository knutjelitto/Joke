using System.Diagnostics;

namespace Joke.Front.Pony.Lexing
{
    public struct Token
    {
        public Token(TK kind, int clutter, int payload, int next)
        {
            Debug.Assert(clutter <= payload);
            Debug.Assert(payload <= next);

            Kind = kind;
            Clutter = clutter;
            Payload = payload;
            Next = next;
        }

        public readonly TK Kind;
        public readonly int Clutter;
        public readonly int Payload;
        public readonly int Next;

        public string GetClutter(ISource source)
        {
            return source.GetText(Clutter, Payload - Clutter);
        }

        public string GetPayload(ISource source)
        {
            return source.GetText(Payload, Next - Payload);
        }
    }
}
