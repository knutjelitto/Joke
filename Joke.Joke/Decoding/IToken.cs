namespace Joke.Joke.Decoding
{
    public interface IToken
    {
        ISource Source { get; }
        int Clutter { get; }
        int Payload { get; }
        int Next { get; }
        ISourceSpan PayloadSpan { get; }

        string GetClutter();
        string GetPayload();
    }
}
