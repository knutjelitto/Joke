using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class InfixType : Type
    {
        public InfixType(TSpan span, Type left, IReadOnlyList<InfixTypePart> parts)
            : base(span)
        {
            Left = left;
            Parts = parts;
        }

        public Type Left { get; }
        public IReadOnlyList<InfixTypePart> Parts { get; }
    }
}
