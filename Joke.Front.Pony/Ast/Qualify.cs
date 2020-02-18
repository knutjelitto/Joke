using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Qualify : PostfixPart
    {
        public Qualify(TSpan span, TypeArguments arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public TypeArguments Arguments { get; }
    }
}
