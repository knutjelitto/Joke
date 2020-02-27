using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtClass : PtNode
    {
        public PtClass(PonyTokenSpan span,
            PtClassKind kind,
            PtAnnotations? annotations,
            bool bare,
            PtCap? cap,
            PtIdentifier name,
            PtTypeParameters? typeParameters,
            PtType? provides,
            PtString? doc,
            PtMembers members)
            : base(span)
        {
            Kind = kind;
            Annotations = annotations;
            Bare = bare;
            Cap = cap;
            Name = name;
            TypeParameters = typeParameters;
            Provides = provides;
            Doc = doc;
            Members = members;
        }

        public PtClassKind Kind { get; }
        public PtAnnotations? Annotations { get; }
        public bool Bare { get; }
        public PtCap? Cap { get; }
        public PtIdentifier Name { get; }
        public PtTypeParameters? TypeParameters { get; }
        public PtType? Provides { get; }
        public PtString? Doc { get; }
        public PtMembers Members { get; }
    }
}
