using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Local : Expression
    {
        public Local(TSpan span, LocalKind kind, Identifier name, Type? type)
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
