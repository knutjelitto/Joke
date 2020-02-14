using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class UseFfi : Use
    {
        public UseFfi(TSpan span, Identifier? name, FfiName ffiName, TypeArguments typeArguments, Parameters parameters, Partial? partial)
            : base(span, name)
        {
            FfiName = ffiName;
            TypeArguments = typeArguments;
            Parameters = parameters;
            Partial = partial;
        }

        public FfiName FfiName { get; }
        public TypeArguments TypeArguments { get; }
        public Parameters Parameters { get; }
        public Partial? Partial { get; }
    }
}
