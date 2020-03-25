using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Joke.Joke.Tools
{
    public class DistinctList<K, V> : IReadOnlyList<V>, IReadOnlyDictionary<K, V>
        where K : notnull
        where V : class        
    {
        private readonly Dictionary<K, V> lookup = new Dictionary<K, V>();
        private readonly List<V> items = new List<V>();

        public V this[int index] => items[index];

        public V this[K key] => lookup[key];

        public int Count => items.Count;

        public IEnumerable<K> Keys => lookup.Keys;

        public IEnumerable<V> Values => items;

        public V? TryFind(K key)
        {
            if (TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }

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

        public bool TryAdd(K key, V value)
        {
            if (lookup.TryAdd(key, value))
            {
                items.Add(value);
                return true;
            }

            return false;
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
