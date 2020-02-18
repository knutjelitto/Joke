using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Binary : Expression
    {
        public Binary(TSpan span, BinaryKind kind, IReadOnlyList<Expression> operands)
            : base(span)
        {
            Debug.Assert(operands.Count >= 2);

            Kind = kind;
            Operands = operands;
        }
        public Binary(TSpan span, BinaryKind kind, params Expression[] operands)
            : this(span, kind, (IReadOnlyList<Expression>)operands)
        {
        }

        public BinaryKind Kind { get; }
        public IReadOnlyList<Expression> Operands { get; }
    }
}
