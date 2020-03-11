using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Method : IMember
    {
        public Method(TokenSpan span, MethodKind kind, String? doc, Cap? cap, Identifier name, TypeParameterList? typeParameters, ValueParameterList valueParameters, IType? @return, Throws? throws, IExpression? body)
        {
            Span = span;
            Kind = kind;
            Doc = doc;
            Cap = cap;
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
        public Cap? Cap { get; }
        public Identifier Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public ValueParameterList ValueParameters { get; }
        public IType? Return { get; }
        public Throws? Throws { get; }
        public IExpression? Body { get; }
    }
}
