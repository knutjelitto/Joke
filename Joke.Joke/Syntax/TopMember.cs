using Joke.Joke.Err;

namespace Joke.Joke.Syntax
{
    public class TopMember
    {
        public TopMember(Unit unit, Tree.ClassType source)
        {
            Unit = unit;
            Source = source;
        }

        public Unit Unit { get; }
        public Package Package => Unit.Package;
        public Errors Errors => Package.Errors;

        public Tree.ClassType Source { get; }
    }
}
