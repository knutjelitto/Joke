﻿using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class FfiName : Identifier
    {
        public FfiName(TSpan span, Expression name)
            : base(span)
        {
            Name = name;
        }

        public Expression Name { get; }

        public override string? ToString()
        {
            return $"@{Name}";
        }
    }
}
