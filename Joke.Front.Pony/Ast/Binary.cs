using Joke.Front.Pony.Lexing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Front.Pony.Ast
{
    public class Binary : Expression
    {
        public Binary(TokenSpan span, BinaryKind kind, IReadOnlyList<Expression> operands)
            : base(span)
        {
            Debug.Assert(operands.Count >= 2);

            Kind = kind;
            Operands = operands;
        }
        public Binary(TokenSpan span, BinaryKind kind, params Expression[] operands)
            : this(span, kind, (IReadOnlyList<Expression>)operands)
        {
        }

        public BinaryKind Kind { get; }
        public IReadOnlyList<Expression> Operands { get; }
    }
}
