using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Class : Item
    {
        public Class(ISpan span, ClassKind kind, Capability? capability, Identifier name, DocString? doc, IReadOnlyList<Member> members)
            : base(span)
        {
            Kind = kind;
            Name = name;
            Doc = doc;
            Members = members;
        }

        public ClassKind Kind { get; }
        public Identifier Name { get; }
        public DocString? Doc { get; }
        public IReadOnlyList<Member> Members { get; }
    }
}
