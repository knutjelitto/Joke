using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public enum JumpKind
    {
        Missing,

        Return,
        Break,
        Continue,
        Error,
        CompileIntrinsic,
        CompileError,
    }
}
