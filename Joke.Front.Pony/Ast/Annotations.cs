using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Annotations : Node
    {
        public Annotations(PonyTokenSpan span, IReadOnlyList<Identifier> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<Identifier> Names { get; }
    }
}
