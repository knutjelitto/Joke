using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Field : IMember
    {
        public Field(TokenSpan span, FieldKind kind, String? doc, Identifier name, IType type, IExpression? init)
        {
            Span = span;
            Kind = kind;
            Doc = doc;
            Name = name;
            Type = type;
            Init = init;
        }

        public TokenSpan Span { get; }
        public FieldKind Kind { get; }
        public String? Doc { get; }
        public Identifier Name { get; }
        public IType Type { get; }
        public IExpression? Init { get; }
    }
}
