using System.Collections;
using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Members : IReadOnlyList<IMember>
    {
        public Members(TokenSpan span, IReadOnlyList<IMember> items)
        {
            Span = span;
            Items = items;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<IMember> Items { get; }
        public IMember this[int index] => Items[index];
        public int Count => Items.Count;
        public IEnumerator<IMember> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
