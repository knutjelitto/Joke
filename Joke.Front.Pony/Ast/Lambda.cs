using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Lambda : Expression
    {
        public Lambda(PonyTokenSpan span,
            bool bare,
            Annotations? annotations,
            Cap? receiverCap,
            Identifier? name,
            TypeParameters? typeParameters,
            LambdaParameters parameters,
            LambdaCaptures? captures,
            Type? returnType,
            bool partial,
            Expression? body,
            Cap? referenceCap)
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
        public Annotations? Annotations { get; }
        public Cap? ReceiverCap { get; }
        public Identifier? Name { get; }
        public TypeParameters? TypeParameters { get; }
        public LambdaParameters Parameters { get; }
        public LambdaCaptures? Captures { get; }
        public Type? ReturnType { get; }
        public bool Partial { get; }
        public Expression? Body { get; }
        public Cap? ReferenceCap { get; }
    }
}
