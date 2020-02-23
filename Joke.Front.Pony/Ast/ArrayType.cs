using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class ArrayType : Type
    {
        public ArrayType(TokenSpan span, Type type)
            : base(span)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
