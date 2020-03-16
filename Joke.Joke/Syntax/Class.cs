using Joke.Joke.Err;

namespace Joke.Joke.Syntax
{
    public class Class
    {
        public Class(Package package, Tree.ClassType source)
        {
            Package = package;
            Source = source;
            Members = new ClassMemberList();
        }

        public Package Package { get; }
        public Errors Errors => Package.Errors;
        public Tree.ClassType Source { get; }

        public ClassMemberList Members { get; }
    }
}
