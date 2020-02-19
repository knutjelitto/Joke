using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Annotations : Node
    {
        public Annotations(TokenSpan span, IReadOnlyList<Identifier> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<Identifier> Names { get; }
    }
}
