namespace Joke.Outside.Build
{
    public class IoLine
    {
        public IoLine(StdIo std, string text)
        {
            Std = std;
            Text = text;
        }

        public StdIo Std { get; }
        public string Text { get; }

        public static IoLine Out(string text)
        {
            return new IoLine(StdIo.Out, text);
        }
        public static IoLine Err(string text)
        {
            return new IoLine(StdIo.Err, text);
        }

        public override string ToString()
        {
            return $"{Std}: {Text}";
        }
    }
}
