﻿using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Local : Expression
    {
        public Local(PonyTokenSpan span, LocalKind kind, Identifier name, Type? type)
            : base(span)
        {
            Kind = kind;
            Name = name;
            Type = type;
        }

        public LocalKind Kind { get; }
        public Identifier Name { get; }
        public Type? Type { get; }
    }
}
