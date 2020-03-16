using Joke.Joke.Err;

namespace Joke.Joke.Syntax
{
    public class ClassMember
    {
        public ClassMember(Unit unit, Tree.INamedMember source)
        {
            Unit = unit;
            Source = source;
        }

        public Unit Unit { get; }
        public Tree.INamedMember Source { get; }
        public Package Package => Unit.Package;
        public Errors Errors => Package.Errors;
    }
}
