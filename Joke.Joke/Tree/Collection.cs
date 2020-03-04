using System.Collections;
using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Collection<T>: IReadOnlyList<T>
    {
        public Collection(TokenSpan span, IReadOnlyList<T> items)
        {
            Span = span;
            Items = items;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<T> Items { get; }
        public T this[int index] => Items[index];
        public int Count => Items.Count;
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
