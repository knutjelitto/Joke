using System.Collections;
using System.Collections.Generic;

namespace Joke.Front.Pony.Lexing
{
    public class Tokens : IReadOnlyList<Token>
    {
        private readonly IReadOnlyList<Token> tokens;

        public Tokens(ISource source, IReadOnlyList<Token> tokens)
        {
            Source = source;
            this.tokens = tokens;
        }

        public ISource Source { get; }
        public Token this[int index] => tokens[index];

        public int Count => tokens.Count;
        public IEnumerator<Token> GetEnumerator() => tokens.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
