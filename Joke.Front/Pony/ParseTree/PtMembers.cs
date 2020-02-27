using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtMembers : PtNode
    {
        public PtMembers(PonyTokenSpan span, PtFields fields, PtMethods methods)
            : base(span)
        {
            Fields = fields;
            Methods = methods;
        }

        public PtFields Fields { get; }
        public PtMethods Methods { get; }
    }
}
