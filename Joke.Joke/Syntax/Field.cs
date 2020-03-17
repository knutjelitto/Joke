namespace Joke.Joke.Syntax
{
    public class Field : Member
    {
        public Field(Class @class, Tree.Field source)
        {
            Class = @class;
            Source = source;
        }

        public Class Class { get; }
        public override Tree.INamedMember Source { get; }
        public override Tree.Identifier Name => Source.Name;
    }
}
