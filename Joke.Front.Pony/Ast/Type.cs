using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class Type : Node
    {
        protected Type(TSpan span)
            : base(span)
        {
        }
    }
}
