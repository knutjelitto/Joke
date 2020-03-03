namespace Joke.Joke.Decoding
{
    public interface ISource
    {
        string Name { get; }
        string Content { get; }
        int this[int index] { get; }
        (int lineNo, int colNo) GetLineCol(int index);
        string GetText(int start, int length);
        bool AtEnd(int offset);
        string GetLine(int lineNo);
    }
}
