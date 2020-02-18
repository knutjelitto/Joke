using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Ref : Expression
    {
        public Ref(TSpan span, Identifier name)
            : base(span)
        {
            Name = name;
        }

        public Identifier Name { get; }
    }
}
