using System;

using Joke.Joke.Tree;

namespace Joke.Joke.Syntaxn
{
    public class TypeBuilder : Visitor
    {
        public TypeBuilder(Package package, IType type)
        {
            Package = package;
            Type = type;
        }

        public Package Package { get; }
        public IType Type { get; }

        public void Build()
        {
            Type.Accept(this);
        }

        public override void Visit(NominalType type)
        {
            Console.WriteLine($"T: {type.Name}");
            if (type.Name.Names.Count != 1)
            {
                throw new NotImplementedException();
            }
            if (type.Arguments != null)
            {
                foreach (var arg in type.Arguments.Items)
                {
                    arg.Accept(this);
                }
            }
        }
    }
}
