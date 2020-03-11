using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ClassType : IType, IMember
    {
        public ClassType(TokenSpan span, ClassKind kind, String? doc, Cap? cap, Identifier name, TypeParameterList? typeParameters, IType? provides, MemberList members)
        {
            Span = span;
            Kind = kind;
            Doc = doc;
            Cap = cap;
            Name = name;
            TypeParameters = typeParameters;
            Provides = provides;
            Members = members;
        }

        public TokenSpan Span { get; }
        public ClassKind Kind { get; }
        public String? Doc { get; }
        public Cap? Cap { get; }
        public Identifier Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public IType? Provides { get; }
        public MemberList Members { get; }
    }
}
