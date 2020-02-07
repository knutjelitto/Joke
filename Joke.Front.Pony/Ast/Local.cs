using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Local : Expression
    {
        public Local(ISpan span, MemberKind kind, Identifier name, Type? type)
            : base(span)
        {
            Kind = kind;
            Name = name;
            Type = type;
        }

        public MemberKind Kind { get; }
        public Identifier Name { get; }
        public Type? Type { get; }
    }
}
