using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Class : Item
    {
        public Class(ISpan span, ClassKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public ClassKind Kind { get; }
    }
}
