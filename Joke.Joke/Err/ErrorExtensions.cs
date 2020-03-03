using Joke.Joke.Decoding;

namespace Joke.Joke.Err
{
    public static class ErrorExtensions
    {
        public static void AtOffset(this Errors errors, ErrNo no, ISource source, int offset, string message)
        {
            errors.Help.Add(new SourceSpan(source, offset, 0), no, message);
        }
    }
}
