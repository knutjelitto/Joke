using Joke.Joke.Decoding;
using Joke.Joke.Tools;

namespace Joke.Joke.Tree
{
    public class Class : INamedMember, INamedType
    {
        public Class(TokenSpan span, ClassKind kind, Identifier name, TypeParameterList? typeParameters, IType? provides, String? doc, MemberList items)
        {
            Span = span;
            Kind = kind;
            Name = name;
            TypeParameters = typeParameters;
            Provides = provides;
            Doc = doc;
            Items = items;

            Members = new NamedList<INamedMember>();
            Fields = new NamedList<Field>();
            Methods = new NamedList<Method>();
        }

        public TokenSpan Span { get; }
        public ClassKind Kind { get; }
        public String? Doc { get; }
        public Identifier Name { get; }
        public TypeParameterList? TypeParameters { get; }
        public IType? Provides { get; }
        public MemberList Items { get; }

        // -- 
        public NamedList<INamedMember> Members { get; }
        public NamedList<Field> Fields { get; }
        public NamedList<Method> Methods { get; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
