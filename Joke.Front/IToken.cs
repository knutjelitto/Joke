namespace Joke.Front
{
    public interface IToken
    {
        ISourceSpan PayloadSpan { get; }

        string GetClutter();
        string GetPayload();
    }
}
