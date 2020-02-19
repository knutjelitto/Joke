using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class UseUri : Use
    {
        public UseUri(TokenSpan span, Identifier? name, String uri)
            : base(span, name)
        {
            Uri = uri;
        }

        public String Uri { get; }
    }
}
