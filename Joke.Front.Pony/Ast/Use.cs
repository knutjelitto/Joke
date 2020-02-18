using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class Use : Node
    {
        public Use(TSpan span, Identifier? name)
            : base(span)
        {
            Name = name;
        }

        public Identifier? Name { get; }
    }
}
