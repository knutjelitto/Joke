using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public enum JumpKind
    {
        Return,
        Break,
        Continue,
        Error,
        CompileIntrinsic,
        CompileError,
    }
}
