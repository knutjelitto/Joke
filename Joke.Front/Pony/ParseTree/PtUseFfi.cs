using System.Diagnostics;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtUseFfi : PtUse
    {
        public PtUseFfi(PonyTokenSpan span, PtIdentifier? name, PtFfiName ffiName, PtTypeArguments typeArguments, PtParameters parameters, bool partial, PtGuard? guard)
            : base(span, name, guard)
        {
            Debug.Assert(name == null);

            FfiName = ffiName;
            TypeArguments = typeArguments;
            Parameters = parameters;
            Partial = partial;
        }

        public PtFfiName FfiName { get; }
        public PtTypeArguments TypeArguments { get; }
        public PtParameters Parameters { get; }
        public bool Partial { get; }
    }
}
