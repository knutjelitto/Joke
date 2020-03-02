using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtMethod : PtNode
    {
        public PtMethod(PonyTokenSpan span,
            PtMethodKind kind,
            PtAnnotations? annotations,
            bool bare,
            PtCap? receiverCap,
            PtIdentifier name,
            PtTypeParameters? typeParameters,
            PtParameters parameters,
            PtType? returnType,
            bool partial,
            PtString? doc,
            PtExpression? body)
            : base(span)
        {
            Kind = kind;
            Annotations = annotations;
            Bare = bare;
            ReceiverCap = receiverCap;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            ReturnType = returnType;
            Partial = partial;
            Doc = doc;
            Body = body;
        }

        public PtMethodKind Kind { get; }
        public PtAnnotations? Annotations { get; }
        public bool Bare { get; }
        public PtCap? ReceiverCap { get; }
        public PtIdentifier Name { get; }
        public PtTypeParameters? TypeParameters { get; }
        public PtParameters Parameters { get; }
        public PtType? ReturnType { get; }
        public bool Partial { get; }
        public PtString? Doc { get; }
        public PtExpression? Body { get; }
    }
}
