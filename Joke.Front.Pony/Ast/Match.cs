using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Match : Expression
    {
        public Match(ISpan span, Expression toMatch, IEnumerable<Case> cases, Expression? @else)
            : base(span)
        {
            ToMatch = toMatch;
            Cases = cases;
            Else = @else;
        }

        public Expression ToMatch { get; }
        public IEnumerable<Case> Cases { get; }
        public Expression? Else { get; }
    }
}
