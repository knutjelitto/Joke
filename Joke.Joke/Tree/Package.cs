namespace Joke.Joke.Tree
{
    public class Package
    {
        public Package(string name, UnitList units)
        {
            Name = name;
            Units = units;
        }

        public string Name { get; }
        public UnitList Units { get; }
    }
}
