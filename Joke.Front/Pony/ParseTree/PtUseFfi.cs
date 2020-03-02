using Joke.Front.Pony.Lexing;
using System.Diagnostics;

namespace Joke.Front.Pony.ParseTree
{
    public sealed class PtUseFfi : PtUse
    {
        public PtUseFfi(PonyTokenSpan span, PtIdentifier? alias, PtFfiName ffiName, PtTypeArguments typeArguments, PtParameters parameters, bool partial, PtGuard? guard)
            : base(span, alias, guard)
        {
            Debug.Assert(alias == null);
            Debug.Assert(!partial);

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
