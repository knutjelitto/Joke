using System;

using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
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
            if (type.Name.Names.Count != 1)
            {
                throw new NotImplementedException();
            }
            if (type.Arguments != null)
            {
                throw new NotImplementedException();
            }
        }
    }
}
