using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLambdaType : PtType
    {
        public PtLambdaType(PonyTokenSpan span,
            bool bare,
            PtCap? receiverCap,
            PtIdentifier? name,
            PtTypeParameters? typeParameters,
            PtLambdaTypeParameters parameters,
            PtType? returnType,
            bool partial,
            PtCap? referenceCap,
            PtEphemAlias? ea)
            : base(span)
        {
            Bare = bare;
            ReceiverCap = receiverCap;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            ReturnType = returnType;
            Partial = partial;
            ReferenceCap = referenceCap;
            Ea = ea;
        }

        public bool Bare { get; }
        public PtCap? ReceiverCap { get; }
        public PtIdentifier? Name { get; }
        public PtTypeParameters? TypeParameters { get; }
        public PtLambdaTypeParameters Parameters { get; }
        public PtType? ReturnType { get; }
        public bool Partial { get; }
        public PtCap? ReferenceCap { get; }
        public PtEphemAlias? Ea { get; }
    }
}
