using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ClassType : IType, IMember, INamed
    {
        public ClassType(TokenSpan span, ClassKind kind, String? doc, Identifier name, TypeParameterList? typeParameters, IType? provides, MemberList members)
        {
            Span = span;
            Kind = kind;
            Doc = doc;
            Name = name;
            TypeParameters = typeParameters;
            Provides = provides;
            Members = members;
        }

        public TokenSpan Span { get; }
        public ClassKind Kind { get; }
        public String? Doc { get; }
        public Identifier Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public IType? Provides { get; }
        public MemberList Members { get; }
    }
}
