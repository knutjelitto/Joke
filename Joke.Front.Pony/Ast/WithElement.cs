using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class WithElement : Node
    {
        public WithElement(TSpan span, Ids names, Expression initializer)
            : base(span)
        {
            Names = names;
            Initializer = initializer;
        }

        public Ids Names { get; }
        public Expression Initializer { get; }
    }
}
