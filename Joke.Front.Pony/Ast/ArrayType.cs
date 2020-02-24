using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class ArrayType : Type
    {
        public ArrayType(PonyTokenSpan span, Type type)
            : base(span)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
