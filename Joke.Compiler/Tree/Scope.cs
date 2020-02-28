using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class Scope
    {
        private readonly LookupList<string, string> Members = new LookupList<string, string>();

        public Scope(Scope? upper)
        {
            Upper = upper;
        }

        public Scope? Upper { get; }
    }
}
