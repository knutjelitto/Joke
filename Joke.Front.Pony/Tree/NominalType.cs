using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class NominalType : Type
    {
        public NominalType(TSpan span, Identifier name, TypeArguments typeArguments, Cap cap, EphemAlias ea)
            : base(span)
        {
            Name = name;
            TypeArguments = typeArguments;
            Cap = cap;
            Ea = ea;
        }

        public Identifier Name { get; }
        public TypeArguments TypeArguments { get; }
        public Cap Cap { get; }
        public EphemAlias Ea { get; }
    }
}
