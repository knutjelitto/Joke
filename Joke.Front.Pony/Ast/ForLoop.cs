using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class ForLoop : Expression
    {
        public ForLoop(ISpan span, IdSeq ids, Expression iterator, Expression body, Expression? @else)
            : base(span)
        {
            Ids = ids;
            Iterator = iterator;
            Body = body;
            Else = @else;
        }

        public IdSeq Ids { get; }
        public Expression Iterator { get; }
        public Expression Body { get; }
        public Expression? Else { get; }
    }
}
