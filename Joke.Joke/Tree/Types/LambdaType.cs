using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class LambdaType : IType
    {
        public LambdaType(TokenSpan span, Identifier? name, TypeParameterList? typeParameters, TypeList parameters, IType? result, Throws? throws)
        {
            Span = span;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            Result = result;
            Throws = throws;
        }

        public TokenSpan Span { get; }
        public Identifier? Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public TypeList Parameters { get; }
        public IType? Result { get; }
        public Throws? Throws { get; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
