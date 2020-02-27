using Joke.Front.Pony.Lexing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Front.Pony.ParseTree
{
    public class PtBinary : PtExpression
    {
        public PtBinary(PonyTokenSpan span, PtBinaryKind kind, IReadOnlyList<PtExpression> operands)
            : base(span)
        {
            Debug.Assert(operands.Count >= 2);

            Kind = kind;
            Operands = operands;
        }
        public PtBinary(PonyTokenSpan span, PtBinaryKind kind, params PtExpression[] operands)
            : this(span, kind, (IReadOnlyList<PtExpression>)operands)
        {
        }

        public PtBinaryKind Kind { get; }
        public IReadOnlyList<PtExpression> Operands { get; }
    }
}
