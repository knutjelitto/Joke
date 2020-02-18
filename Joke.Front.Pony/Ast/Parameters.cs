using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Parameters : Node
    {
        public Parameters(TSpan span, IReadOnlyList<Parameter> parameters)
            : base(span)
        {
            Items = parameters;
        }

        public IReadOnlyList<Parameter> Items { get; }
    }
}
