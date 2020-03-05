using Joke.Joke.Decoding;
using System.Collections.Generic;

namespace Joke.Joke.Tree
{
    public class If : IExpression
    {
        public If(TokenSpan span, IReadOnlyList<Conditional> conditionals, Else? elseBody)
        {
            Span = span;
            Conditionals = conditionals;
            ElseBody = elseBody;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<Conditional> Conditionals { get; }
        public Else? ElseBody { get; }
    }
}
