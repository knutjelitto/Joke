using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class GroupedExpression : Expression
    {
        public GroupedExpression(TokenSpan span, IReadOnlyList<Expression> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Expression> Items { get; }
    }
}
