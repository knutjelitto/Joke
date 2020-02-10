using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class IdSeqMulti : IdSeq
    {
        public IdSeqMulti(ISpan span, IReadOnlyList<IdSeq> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<IdSeq> Names { get; }
    }
}
