using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Object : Expression
    {
        public Object(ISpan span, Capability? capbility, Type? provides, IReadOnlyList<Member> members)
            : base(span)
        {
            Capbility = capbility;
            Provides = provides;
            Members = members;
        }

        public Capability? Capbility { get; }
        public Type? Provides { get; }
        public IReadOnlyList<Member> Members { get; }
    }
}
