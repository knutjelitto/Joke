namespace Joke.Front.Pony.Ast
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
