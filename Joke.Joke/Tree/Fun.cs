using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Fun : IMember
    {
        public Fun(TokenSpan span, Identifier name, TypeParameterList? typeParameters, ValueParameterList valueParameters, IType? @return, IExpression? body)
        {
            Span = span;
            Name = name;
            TypeParameters = typeParameters;
            ValueParameters = valueParameters;
            Return = @return;
            Body = body;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public ValueParameterList ValueParameters { get; }
        public IType? Return { get; }
        public IExpression? Body { get; }
    }
}
