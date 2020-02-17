using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class FfiCall : Expression
    {
        public FfiCall(TSpan span, Expression name, TypeArguments? returnType, Arguments arguments, Partial? partial)
            : base(span)
        {
            Name = name;
            ReturnType = returnType;
            Arguments = arguments;
            Partial = partial;
        }

        public Expression Name { get; }
        public TypeArguments? ReturnType { get; }
        public Arguments Arguments { get; }
        public Partial? Partial { get; }
    }
}
