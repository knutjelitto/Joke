using Joke.Joke.Decoding;
using Joke.Joke.Tools;

namespace Joke.Joke.Tree
{
    public class Method : INamedMember
    {
        public Method(TokenSpan span, MethodKind kind, Identifier name, TypeParameterList? typeParameters, ParameterList valueParameters, IType? @return, Throws? throws, String? doc, IExpression? body)
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

        public void Accept(IVisitor visitor) => visitor.Visit(this);

        public override string ToString()
        {
            var text = CharRep.InText(Span.ToString());
            if (text.Length > 80)
            {
                return text.Substring(0, 80) + "...";
            }
            return text;
        }
    }
}
