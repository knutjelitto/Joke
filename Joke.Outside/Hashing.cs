using System;
using System.Collections.Generic;

namespace Joke.Outside
{
    public static class Hashing
    {
        public static int Hash<T>(this IEnumerable<T> values)
        {
            var hash = new HashCode();

            foreach (var value in values)
            {
                hash.Add(value);
            }

            return hash.ToHashCode();
        }
    }
}
