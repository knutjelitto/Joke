using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class AsPart : InfixPart
    {
        public AsPart(TSpan span, Type type)
            : base(span, BinaryKind.As)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
