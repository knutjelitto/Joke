using Joke.Joke.Tree;

namespace Joke.Joke.Tools
{
    public class NamedList<V> : DistinctList<INamed, V>
        where V : class, INamed
    {
        public void Add(INamed named)
        {
            base.Add(named, (V)named);
        }
        public bool TryAdd(INamed named)
        {
            return base.TryAdd(named, (V)named);
        }
    }
}
