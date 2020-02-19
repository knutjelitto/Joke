using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class Parameter : Node
    {
        public Parameter(TokenSpan span)
            : base(span)
        {
        }
    }
}
