using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class ViewpointType : Type
    {
        public ViewpointType(TSpan span, Type type, ArrowType arrow)
            : base(span)
        {
            Type = type;
            Arrow = arrow;
        }

        public Type Type { get; }
        public ArrowType Arrow { get; }
    }
}
