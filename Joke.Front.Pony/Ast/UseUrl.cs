using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class UseUrl : Use
    {
        public UseUrl(ISpan span, Identifier? name, String url)
            : base(span, name)
        {
            Url = url;
        }

        public String Url { get; }
    }
}
