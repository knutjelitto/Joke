using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Call : PostfixPart
    {
        public Call(PonyTokenSpan span, Arguments arguments, bool partial)
            : base(span)
        {
            Arguments = arguments;
            Partial = partial;
        }

        public Arguments Arguments { get; }
        public bool Partial { get; }
    }
}
