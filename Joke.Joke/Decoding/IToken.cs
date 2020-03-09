namespace Joke.Joke.Decoding
{
    public interface IToken
    {
        ISource Source { get; }
        int ClutterOffset { get; }
        int PayloadOffset { get; }
        int NextOffset { get; }
        ISourceSpan PayloadSpan { get; }

        string Clutter { get; }
        string Payload { get; }
    }
}
