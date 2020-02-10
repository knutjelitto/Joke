using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class UseUrl : Use
    {
        public UseUrl(ISpan span, Identifier? name, String url, Expression? condition)
            : base(span, name)
        {
            Url = url;
            Condition = condition;
        }

        public String Url { get; }
        public Expression? Condition { get; }
    }
}
