﻿using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class AsPart : InfixPart
    {
        public AsPart(PonyTokenSpan span, Type type)
            : base(span, BinaryKind.As)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
