using System;
using System.Linq;

using Joke.Front.Pony.ParseTree;
using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class ClassType : IClass
    {
        private readonly Action<ClassType> discoverMembers;

        public ClassType(PtClass source, Unit unit, string name, Action<ClassType> discoverMembers)
        {
            Source = source;
            Unit = unit;
            Name = name;
            this.discoverMembers = discoverMembers;
        }

        public PtClass Source { get; }
        public Unit Unit { get; }
        public string Name { get; }
        public LookupList<string, IClassMember> Members { get; } = new LookupList<string, IClassMember>();

        private void AddMember(IClassMember member)
        {
            Members.Add(member.Name, member);
        }

        public void DiscoverMembers()
        {
            discoverMembers(this);
        }

        private void DiscoverFields()
        {
            foreach (var ptField in Source.Members.Fields.Items)
            {
                AddMember(Field.From(ptField, this));
            }
        }

        private void DiscoverMethods()
        {
            foreach (var ptMethod in Source.Members.Methods.Items)
            {
                AddMember(Method.From(ptMethod, this));
            }
        }

        private void Discover()
        {
            DiscoverFields();
            DiscoverMethods();
        }

        public static ClassType Actor(PtClass source, Unit unit, string name)
        {
            return new ClassType(source, unit, name,
                ct =>
                {
                    ct.Discover();
                });
        }

        public static ClassType Class(PtClass source, Unit unit, string name)
        {
            return new ClassType(source, unit, name,
                ct =>
                {
                    ct.Discover();
                    ct.NoBehaviours("class");
                });
        }

        public static ClassType Struct(PtClass source, Unit unit, string name)
        {
            return new ClassType(source, unit, name,
                ct =>
                {
                    ct.Discover();
                    ct.NoBehaviours("struct");
                });
        }

        public static ClassType Interface(PtClass source, Unit unit, string name)
        {
            return new ClassType(source, unit, name,
                ct =>
                {
                    ct.Discover();
                    ct.NoFields("interface");
                });
        }

        public static ClassType Trait(PtClass source, Unit unit, string name)
        {
            return new ClassType(source, unit, name,
                ct =>
                {
                    ct.Discover();
                    ct.NoFields("trait");
                });
        }

        public static ClassType Primitive(PtClass source, Unit unit, string name)
        {
            return new ClassType(source, unit, name,
                ct =>
                {
                    ct.Discover();
                    ct.NoFields("primitive");
                    ct.NoBehaviours("primitive");
                });
        }

        public static ClassType Alias(PtClass source, Unit unit, string name)
        {
            return new ClassType(source, unit, name,
                ct =>
                {
                    ct.Discover();
                    ct.NoMembers("type alias");
                });
        }

        private void NoMembers(string context)
        {
            foreach (var member in Members)
            {
                Unit.Errors().NoMemberAllowed(member, context);
            }
        }

        private void NoFields(string context)
        {
            foreach (var field in Members.OfType<Field>())
            {
                Unit.Errors().NoFieldAllowed(field, context);
            }
        }

        private void NoBehaviours(string context)
        {
            foreach (var be in Members.OfType<Method>().Where(m => m.Kind == MethodKind.Be))
            {
                Unit.Errors().NoBehaviourAllowed(be, context);
            }
        }
    }
}
