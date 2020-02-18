using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Call : PostfixPart
    {
        public Call(TSpan span, Arguments arguments, bool partial)
            : base(span)
        {
            Arguments = arguments;
            Partial = partial;
        }

        public Arguments Arguments { get; }
        public bool Partial { get; }
    }
}
