using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Qualify : PostfixPart
    {
        public Qualify(PonyTokenSpan span, TypeArguments arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public TypeArguments Arguments { get; }
    }
}
