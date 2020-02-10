using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class IsSeqSingle : IdSeq
    {
        public IsSeqSingle(ISpan span, Identifier name)
            : base(span)
        {
            Name = name;
        }

        public Identifier Name { get; }
    }
}
