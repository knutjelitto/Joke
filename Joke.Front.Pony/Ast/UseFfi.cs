using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class UseFfi : Use
    {
        public UseFfi(ISpan span, Identifier? name, Expression ffiName, TypeArguments typeArguments, IReadOnlyList<Parameter> parameters, Boolean partial)
            : base(span, name)
        {
            FfiName = ffiName;
            TypeArguments = typeArguments;
            Parameters = parameters;
            Partial = partial;
        }

        public Expression FfiName { get; }
        public TypeArguments TypeArguments { get; }
        public IReadOnlyList<Parameter> Parameters { get; }
        public Boolean Partial { get; }
    }
}
