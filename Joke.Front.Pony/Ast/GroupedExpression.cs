using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class GroupedExpression : Expression
    {
        public GroupedExpression(TSpan span, IReadOnlyList<Expression> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Expression> Items { get; }
    }
}
