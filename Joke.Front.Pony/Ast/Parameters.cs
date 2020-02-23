using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Parameters : Node
    {
        public Parameters(TokenSpan span, IReadOnlyList<Parameter> parameters)
            : base(span)
        {
            Items = parameters;
        }

        public IReadOnlyList<Parameter> Items { get; }
    }
}
