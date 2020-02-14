using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class TypeParameter : Node
    {
        public TypeParameter(TSpan span, Identifier name, Type? type, Type? defaultType)
            : base(span)
        {
            Name = name;
            Type = type;
            DefaultType = defaultType;
        }

        public Identifier Name { get; }
        public Type? Type { get; }
        public Type? DefaultType { get; }
    }
}
