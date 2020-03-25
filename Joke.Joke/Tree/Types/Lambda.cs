using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Lambda : IExpression
    {
        public Lambda(TokenSpan span, Identifier? name, TypeParameterList? typeParameters, ParameterList parameters, CaptureList? captures, IType? result, Throws? throws, IExpression body)
        {
            Span = span;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            Captures = captures;
            Result = result;
            Throws = throws;
            Body = body;
        }

        public TokenSpan Span { get; }
        public Identifier? Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public ParameterList Parameters { get; }
        public CaptureList? Captures { get; }
        public IType? Result { get; }
        public Throws? Throws { get; }
        public IExpression Body { get; }
    }
}
