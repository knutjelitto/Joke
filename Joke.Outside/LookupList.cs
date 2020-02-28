using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Joke.Outside
{
    public class LookupList<K, V> : IReadOnlyList<V>, IReadOnlyDictionary<K, V>
        where K : notnull
        where V : notnull
    {
        private readonly Dictionary<K, V> lookup = new Dictionary<K, V>();
        private readonly List<V> items = new List<V>();

        public V this[int index] => items[index];

        public V this[K key] => lookup[key];

        public int Count => items.Count;

        public IEnumerable<K> Keys => lookup.Keys;

        public IEnumerable<V> Values => items;

        public void Add(K key, V value)
        {
            lookup.Add(key, value);
            items.Add(value);
        }

        public bool ContainsKey(K key)
        {
            return lookup.ContainsKey(key);
        }

        public IEnumerator<V> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            return lookup.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            return lookup.GetEnumerator();
        }
    }
}
