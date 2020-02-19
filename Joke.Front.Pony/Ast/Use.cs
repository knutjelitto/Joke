using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class Use : Node
    {
        public Use(TokenSpan span, Identifier? name)
            : base(span)
        {
            Name = name;
        }

        public Identifier? Name { get; }
    }
}
