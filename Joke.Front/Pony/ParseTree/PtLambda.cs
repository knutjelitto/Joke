using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLambda : PtExpression
    {
        public PtLambda(PonyTokenSpan span,
            bool bare,
            PtAnnotations? annotations,
            PtCap? receiverCap,
            PtIdentifier? name,
            PtTypeParameters? typeParameters,
            PtLambdaParameters parameters,
            PtLambdaCaptures? captures,
            PtType? returnType,
            bool partial,
            PtExpression? body,
            PtCap? referenceCap)
            : base(span)
        {
            Bare = bare;
            Annotations = annotations;
            ReceiverCap = receiverCap;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            Captures = captures;
            ReturnType = returnType;
            Partial = partial;
            Body = body;
            ReferenceCap = referenceCap;
        }

        public bool Bare { get; }
        public PtAnnotations? Annotations { get; }
        public PtCap? ReceiverCap { get; }
        public PtIdentifier? Name { get; }
        public PtTypeParameters? TypeParameters { get; }
        public PtLambdaParameters Parameters { get; }
        public PtLambdaCaptures? Captures { get; }
        public PtType? ReturnType { get; }
        public bool Partial { get; }
        public PtExpression? Body { get; }
        public PtCap? ReferenceCap { get; }
    }
}
