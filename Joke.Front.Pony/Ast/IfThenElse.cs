using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class IfThenElse : Expression
    {
        public IfThenElse(ISpan span, IReadOnlyList<IfThen> guarded, Expression? elseCase)
            : base(span)
        {
            Guarded = guarded;
            ElseCase = elseCase;
        }

        public IReadOnlyList<IfThen> Guarded { get; }
        public Expression? ElseCase { get; }
    }
}
