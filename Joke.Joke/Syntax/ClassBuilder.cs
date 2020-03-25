using Joke.Joke.Tree;
using System.Diagnostics;

namespace Joke.Joke.Syntax
{
    public class ClassBuilder : Visitor
    {
        public ClassBuilder(Package package, Class @class)
        {
            Package = package;
            Class = @class;
        }

        public Package Package { get; }
        public Class Class { get; }

        public void Build()
        {
            var scope = new ClassScope(Package.Scope);
            if (Class.TypeParameters != null)
            {
                foreach (TypeParameter parameter in Class.TypeParameters.Items)
                {
                    if (parameter.Type != null)
                    {
                        new TypeBuilder(Package, parameter.Type).Build();
                    }
                    Debug.Assert(parameter.Default == null);
                }
            }

        }
    }
}
