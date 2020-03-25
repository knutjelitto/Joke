using Joke.Joke.Decoding;
using Joke.Joke.Tools;

namespace Joke.Joke.Tree
{
    public class Field : INamedMember
    {
        public Field(TokenSpan span, FieldKind kind, Identifier name, IType type, IExpression? init, String? doc)
        {
            Span = span;
            Kind = kind;
            Name = name;
            Type = type;
            Init = init;
            Doc = doc;
        }

        public TokenSpan Span { get; }
        public FieldKind Kind { get; }
        public String? Doc { get; }
        public Identifier Name { get; }
        public IType Type { get; }
        public IExpression? Init { get; }

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
