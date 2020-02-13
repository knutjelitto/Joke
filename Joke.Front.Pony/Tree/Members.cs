using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Members : Node
    {
        public Members(TSpan span, Fields fields, Methods methods)
            : base(span)
        {
            Fields = fields;
            Methods = methods;
        }

        public Fields Fields { get; }
        public Methods Methods { get; }
    }
}
