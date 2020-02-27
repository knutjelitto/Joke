namespace Joke.Front.Pony.ParseTree
{
    public enum PtJumpKind
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
