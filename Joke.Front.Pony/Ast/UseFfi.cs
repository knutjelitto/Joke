using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class UseFfi : Use
    {
        public UseFfi(ISpan span, Identifier? name, ExternIdentifier ffiName, TypeArguments typeArguments, Parameters parameters, Boolean partial, Expression? condition)
            : base(span, name)
        {
            FfiName = ffiName;
            TypeArguments = typeArguments;
            Parameters = parameters;
            Partial = partial;
            Condition = condition;
        }

        public ExternIdentifier FfiName { get; }
        public TypeArguments TypeArguments { get; }
        public Parameters Parameters { get; }
        public Boolean Partial { get; }
        public Expression? Condition { get; }
    }
}
