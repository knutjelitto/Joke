using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Method : INamedMember
    {
        public Method(TokenSpan span, MethodKind kind, String? doc, Identifier name, TypeParameterList? typeParameters, ParameterList valueParameters, IType? @return, Throws? throws, IExpression? body)
        {
            Span = span;
            Kind = kind;
            Doc = doc;
            Name = name;
            TypeParameters = typeParameters;
            ValueParameters = valueParameters;
            Return = @return;
            Throws = throws;
            Body = body;
        }

        public TokenSpan Span { get; }
        public MethodKind Kind { get; }
        public String? Doc { get; }
        public Identifier Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public ParameterList ValueParameters { get; }
        public IType? Return { get; }
        public Throws? Throws { get; }
        public IExpression? Body { get; }
    }
}
