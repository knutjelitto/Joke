using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Case : Base
    {
        public Case(ISpan span, Expression? pattern, Expression? guard, Expression? body)
            : base(span)
        {
            Pattern = pattern;
            Guard = guard;
            Body = body;
        }

        public Expression? Pattern { get; }
        public Expression? Guard { get; }
        public Expression? Body { get; }
    }
}
