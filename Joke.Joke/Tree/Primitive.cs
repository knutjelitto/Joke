using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Primitive : IMember
    {
        public Primitive(TokenSpan span, Identifier name, IType? provides, Members members)
        {
            Span = span;
            Name = name;
            Provides = provides;
            Members = members;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IType? Provides { get; }
        public Members Members { get; }
    }
}
