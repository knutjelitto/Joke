using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtFfiCall : PtExpression
    {
        public PtFfiCall(PonyTokenSpan span, PtExpression name, PtTypeArguments? returnType, PtArguments arguments, bool partial)
            : base(span)
        {
            Name = name;
            ReturnType = returnType;
            Arguments = arguments;
            Partial = partial;
        }

        public PtExpression Name { get; }
        public PtTypeArguments? ReturnType { get; }
        public PtArguments Arguments { get; }
        public bool Partial { get; }
    }
}
