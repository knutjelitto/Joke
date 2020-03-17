namespace Joke.Joke.Syntax
{
    public class Method : Member
    {
        public Method(Class @class, Tree.Method source)
        {
            Class = @class;
            Source = source;
        }

        public Class Class { get; }
        public override Tree.INamedMember Source { get; }
        public override Tree.Identifier Name => Source.Name;
    }
}
