﻿using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Use : Node
    {
        public Use(PonyTokenSpan span, Identifier? name, Guard? guard)
            : base(span)
        {
            Name = name;
            Guard = guard;
        }

        public Identifier? Name { get; }
        public Guard? Guard { get; }
    }
}
