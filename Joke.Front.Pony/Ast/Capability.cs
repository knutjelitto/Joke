using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Capability : Type
    {
        public Capability(ISpan span, RefCapability cap)
            : base (span)
        {
            Cap = cap;
        }

        public RefCapability Cap { get; }
    }
}
