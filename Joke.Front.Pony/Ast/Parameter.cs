using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Parameter : Node
    {
        public Parameter(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
