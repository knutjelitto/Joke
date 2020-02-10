using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class ViewPoint : Type
    {
        public ViewPoint(ISpan span, Type view, Type type)
            : base(span)
        {
            View = view;
            Type = type;
        }

        public Type View { get; }
        public Type Type { get; }
    }
}
