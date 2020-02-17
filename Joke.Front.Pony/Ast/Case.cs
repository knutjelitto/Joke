using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Case : Node
    {
        public Case(TSpan span, Annotations? annotations, Expression? pattern, Expression? guard, Expression? body)
            : base(span)
        {
            Annotations = annotations;
            Pattern = pattern;
            Guard = guard;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Expression? Pattern { get; }
        public Expression? Guard { get; }
        public Expression? Body { get; }
    }
}
