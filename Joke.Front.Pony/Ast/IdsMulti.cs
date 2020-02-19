using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class IdsMulti : Ids
    {
        public IdsMulti(TokenSpan span, IReadOnlyList<Ids> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<Ids> Names { get; }
    }
}
