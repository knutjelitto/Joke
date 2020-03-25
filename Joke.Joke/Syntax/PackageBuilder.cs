using System.Diagnostics;
using System.Linq;

using Joke.Joke.Err;
using Joke.Joke.Tools;
using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public class PackageBuilder : Visitor
    {
        public PackageBuilder(Package package)
        {
            Package = package;
        }

        public Package Package { get; }
        public Errors Errors => Package.Errors;

        public void Build()
        {
            foreach (var unit in Package.Units)
            {
                var noNotNamed = unit.Items.All(m => m is INamedMember);
                Debug.Assert(noNotNamed);

                unit.Accept(this);
            }

            foreach (var @class in Package.Members.Cast<Class>())
            {
                new ClassBuilder(Package, @class).Build();
            }
        }

        public override void Visit(Unit unit)
        {
            foreach (var @class in unit.Items.OfType<Class>())
            {
                var name = @class.Name;

                if (!MaybeAlreadyError(Package.Members, @class))
                {
                    unit.Members.Add(@class, @class);
                    Package.Members.Add(@class);

                }

                @class.Accept(this);
            }
        }
        public override void Visit(Class @class)
        {
            foreach (var source in @class.Items)
            {
                if (source is Field field)
                {
                    if (!MaybeAlreadyError(@class.Members, field))
                    {
                        @class.Members.Add(field);
                        @class.Fields.Add(field);
                    }
                }
                else if (source is Method method)
                {
                    if (!MaybeAlreadyError(@class.Members, method))
                    {
                        @class.Members.Add(method);
                        @class.Methods.Add(method);
                    }
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        private bool MaybeAlreadyError<T>(NamedList<T> members, INamed named)
            where T : class, INamedMember
        {
            if (members.ContainsKey(named))
            {
                var already = members[named].Name;
                Errors.AtToken(ErrNo.Syntax001, named.Name, $"``{named.Name}´´ defined here ...");
                Errors.AtToken(ErrNo.Syntax002, already, $"... is already defined here");
                return true;
            }
            return false;
        }
    }
}
