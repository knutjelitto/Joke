using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class FieldMember : Member
    {
        public FieldMember(ISpan span, MemberKind kind, Identifier ident, Type type, DocString? docStrings)
            : base(span, kind)
        {
            Ident = ident;
            Type = type;
            DocStrings = docStrings;
        }

        public Identifier Ident { get; }
        public Type Type { get; }
        public DocString? DocStrings { get; }
    }
}
