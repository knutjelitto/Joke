using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Class : Item
    {
        public Class(ISpan span, ClassKind kind, Capability? capability, Identifier name, TypeParameters typeParameters, Type? provides, DocString? doc, IReadOnlyList<Member> members)
            : base(span)
        {
            Kind = kind;
            Capability = capability;
            Name = name;
            TypeParameters = typeParameters;
            Provides = provides;
            Doc = doc;
            Members = members;
        }

        public ClassKind Kind { get; }
        public Capability? Capability { get; }
        public Identifier Name { get; }
        public TypeParameters TypeParameters { get; }
        public Type? Provides { get; }
        public DocString? Doc { get; }
        public IReadOnlyList<Member> Members { get; }
    }
}
