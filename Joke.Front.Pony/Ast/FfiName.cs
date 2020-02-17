using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
