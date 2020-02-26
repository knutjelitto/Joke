namespace Joke.Front
{
    public abstract class Scanner
    {
        public const char NoCharacter = '\uFFFF';

        protected readonly string content;

        public int Current { get; set; }

        public int Limit;

        public Scanner(ISource source)
        {
            content = source.Content;
            Current = 0;
            Limit = content.Length;
        }

        public char At()
        {
            return Current < Limit ? content[Current] : NoCharacter;
        }

        public char At(int offset)
        {
            return Current + offset < Limit ? content[Current + offset] : NoCharacter;
        }
    }
}
