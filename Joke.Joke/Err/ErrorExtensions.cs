using Joke.Joke.Decoding;
using Joke.Joke.Tree;

namespace Joke.Joke.Err
{
    public static class ErrorExtensions
    {
        public static void AtOffset(this Errors errors, ErrNo no, ISource source, int offset, string message)
        {
            errors.Help.Add(new SourceSpan(source, offset, 0), no, message);
        }

        public static void AtToken(this Errors errors, ErrNo no, IToken token, string message)
        {
            errors.Help.Add(token, no, message);
        }

        public static void AtToken(this Errors errors, ErrNo no, IAny any, string message)
        {
            errors.Help.Add(any.Span.First, no, message);
        }
    }
}
