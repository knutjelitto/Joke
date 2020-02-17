using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class IdsMulti : Ids
    {
        public IdsMulti(TSpan span, IReadOnlyList<Ids> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<Ids> Names { get; }
    }
}
