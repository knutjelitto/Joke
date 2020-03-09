namespace Joke.Joke.Decoding
{
    public interface ISource
    {
        string Name { get; }
        string Content { get; }
        (int lineNo, int colNo) GetLineCol(int index);
        string GetLine(int lineNo);
    }
}
