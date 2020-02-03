using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LetMember : Member
    {
        public LetMember(ISpan span, Identifier ident, Type type, IEnumerable<DocString> docStrings)
            : base(span)
        {
            Ident = ident;
            Type = type;
            DocStrings = docStrings.ToArray();
        }

        public Identifier Ident { get; }
        public Type Type { get; }
        public IReadOnlyList<DocString> DocStrings { get; }
    }
}
