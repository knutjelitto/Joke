using System.Collections.Generic;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtUnit : PtNode
    {
        public PtUnit(PonyTokenSpan span, PtString? doc, IReadOnlyList<PtUse> uses, IReadOnlyList<PtClass> classes)
            : base(span)
        {
            Doc = doc;
            Uses = uses;
            Classes = classes;
        }

        public PtString? Doc { get; }
        public IReadOnlyList<PtUse> Uses { get; }
        public IReadOnlyList<PtClass> Classes { get; }
    }
}
