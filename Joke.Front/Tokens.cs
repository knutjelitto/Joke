using System.Collections;
using System.Collections.Generic;

namespace Joke.Front
{
    public class Tokens<T> : IReadOnlyList<T> where T : IToken
    {
        private readonly IReadOnlyList<T> tokens;

        public Tokens(ISource source, IReadOnlyList<T> tokens)
        {
            Source = source;
            this.tokens = tokens;
        }

        public ISource Source { get; }
        public T this[int index] => tokens[index];

        public int Count => tokens.Count;
        public IEnumerator<T> GetEnumerator() => tokens.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
