using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class InfixType : Type
    {
        public InfixType(TokenSpan span, InfixTypeKind kind, IReadOnlyList<Type> types)
            : base(span)
        {
            Kind = kind;
            Types = types;
        }

        public InfixTypeKind Kind { get; }
        public IReadOnlyList<Type> Types { get; }
    }
}
