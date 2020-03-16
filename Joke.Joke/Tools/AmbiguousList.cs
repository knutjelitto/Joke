using System.Collections.Generic;

namespace Joke.Joke.Tools
{
    public class AmbiguousList<K,V> : DistinctList<K,IList<V>>
        where K : notnull
        where V : class
    {
    }
}
