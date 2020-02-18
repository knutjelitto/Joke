using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class UseFfi : Use
    {
        public UseFfi(TSpan span, Identifier? name, FfiName ffiName, TypeArguments typeArguments, Parameters parameters, bool partial)
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
        public bool Partial { get; }
    }
}
