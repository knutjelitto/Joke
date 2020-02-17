using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class InfixType : Type
    {
        public InfixType(TSpan span, InfixTypeKind kind, IReadOnlyList<Type> types)
            : base(span)
        {
            Kind = kind;
            Types = types;
        }

        public InfixTypeKind Kind { get; }
        public IReadOnlyList<Type> Types { get; }
    }
}
