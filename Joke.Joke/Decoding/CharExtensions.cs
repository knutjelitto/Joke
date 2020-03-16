namespace Joke.Joke.Decoding
{
    public static class CharExtensions
    {
        public static bool IsLetter(this char ch)
        {
            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z';
        }
        public static bool IsDigit(this char ch)
        {
            return '0' <= ch && ch <= '9';
        }
        public static bool IsLetterOrUnderscore(this char ch)
        {
            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_';
        }
        public static bool IsLetterOrDigit(this char ch)
        {
            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || '0' <= ch && ch <= '9';
        }
        public static bool IsUnderscore(this char ch)
        {
            return ch == '_';
        }
        public static bool Is(this char ch, char ch1, char ch2)
        {
            return ch == ch1 || ch == ch2; ;
        }
        public static bool IsHexDigit(this char ch)
        {
            return '0' <= ch && ch <= '9' || 'a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'F';
        }
    }
}
