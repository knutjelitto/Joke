namespace Joke.Joke.Decoding
{
    public interface ITokenSpan
    {
        IToken First { get; }
        ISourceSpan PayloadSpan { get; }
    }
}
