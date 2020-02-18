using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class FfiCall : Expression
    {
        public FfiCall(TSpan span, Expression name, TypeArguments? returnType, Arguments arguments, bool partial)
            : base(span)
        {
            Name = name;
            ReturnType = returnType;
            Arguments = arguments;
            Partial = partial;
        }

        public Expression Name { get; }
        public TypeArguments? ReturnType { get; }
        public Arguments Arguments { get; }
        public bool Partial { get; }
    }
}
