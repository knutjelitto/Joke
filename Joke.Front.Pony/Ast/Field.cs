﻿using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Field : Node
    {
        public Field(TSpan span, FieldKind kind, Identifier name, Type type, Expression? value, String? doc)
            : base(span)
        {
            Kind = kind;
            Name = name;
            Type = type;
            Value = value;
        }

        public FieldKind Kind { get; }
        public Identifier Name { get; }
        public Type Type { get; }
        public Expression? Value { get; }
    }
}