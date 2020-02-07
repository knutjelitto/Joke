using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class TypeArgType : TypeArgument
    {
        public TypeArgType(ISpan span, Type type)
            : base(span)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
