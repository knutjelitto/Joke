using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class DefaultType : Type
    {
        public DefaultType(TSpan span, Type type) : base(span)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
