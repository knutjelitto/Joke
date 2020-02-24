using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class IdsMulti : Ids
    {
        public IdsMulti(PonyTokenSpan span, IReadOnlyList<Ids> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<Ids> Names { get; }
    }
}
