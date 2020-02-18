using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class ArrayType : Type
    {
        public ArrayType(TSpan span, Type type)
            : base(span)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
