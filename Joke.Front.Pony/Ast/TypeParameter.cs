using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class TypeParameter : Node
    {
        public TypeParameter(PonyTokenSpan span, Identifier name, Type? type, Type? defaultType)
            : base(span)
        {
            Name = name;
            Type = type;
            DefaultType = defaultType;
        }

        public Identifier Name { get; }
        public Type? Type { get; }
        public Type? DefaultType { get; }
    }
}
