using System.Collections;
using System.Collections.Generic;

namespace Joke.Joke.Tree
{
    public class Collection<T>: IReadOnlyList<T>
    {
        public Collection(IReadOnlyList<T> items)
        {
            Items = items;
        }

        public IReadOnlyList<T> Items { get; }
        public T this[int index] => Items[index];
        public int Count => Items.Count;
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
