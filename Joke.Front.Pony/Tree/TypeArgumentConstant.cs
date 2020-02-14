using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class TypeArgumentConstant : TypeArgument
    {
        public TypeArgumentConstant(TSpan span, Expression constant)
            : base(span)
        {
        }
    }
}
