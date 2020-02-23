using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class UseUri : Use
    {
        public UseUri(TokenSpan span, Identifier? name, String uri, Guard? guard)
            : base(span, name, guard)
        {
            Uri = uri;
        }

        public String Uri { get; }
    }
}
