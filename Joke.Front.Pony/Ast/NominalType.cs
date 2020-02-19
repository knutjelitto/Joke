﻿using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class NominalType : Type
    {
        public NominalType(TokenSpan span, Identifier name, TypeArguments? typeArguments, Cap? cap, EphemAlias? ea)
            : base(span)
        {
            Name = name;
            TypeArguments = typeArguments;
            Cap = cap;
            Ea = ea;
        }

        public Identifier Name { get; }
        public TypeArguments? TypeArguments { get; }
        public Cap? Cap { get; }
        public EphemAlias? Ea { get; }
    }
}
