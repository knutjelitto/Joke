using Joke.Joke.Decoding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Joke.Joke.Tree
{
    public class Types : IReadOnlyList<IType>
    {
        public Types(TokenSpan span, IReadOnlyList<IType> items)
        {
            Span = span;
            Items = items;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<IType> Items { get; }
        public IType this[int index] => Items[index];
        public int Count => Items.Count;
        public IEnumerator<IType> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
