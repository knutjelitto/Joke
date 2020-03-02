using System.Collections.Generic;
using System.Linq;
using Joke.Front.Pony.ParseTree;
using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class Unit : IDiscover
    {
        public readonly List<IUse> Uses = new List<IUse>();

        public Unit(PtUnit source, FileRef unitFile, Package package)
        {
            Source = source;
            Package = package;
            UnitFile = unitFile;
        }

        public PtUnit Source { get; }
        public Package Package { get; }
        public FileRef UnitFile { get; }

        public LookupList<string, IPackageMember> Members { get; } = new LookupList<string, IPackageMember>();
        public LookupList<string, Package> Use { get; } = new LookupList<string, Package>();

        public void AddMember(IPackageMember member)
        {
            Members.Add(member.Name, member);
            Package.Members.Add(member.Name, member);
        }

        public void DiscoverMembers()
        {
            foreach (var use in Uses.OfType<UsePackage>())
            {
                if (use.Alias == null)
                {
                    Use.Add(use.Name, use.Package);
                }
                else
                {
                    Members.Add(use.Alias, use.Package);
                }
            }

            foreach (var cls in Source.Classes)
            {
                var name = cls.Name.Value;
                IClass member;

                switch (cls.Kind)
                {
                    case PtClassKind.Actor:
                        member = ClassType.Actor(cls, this, name);
                        break;
                    case PtClassKind.Class:
                        member = ClassType.Class(cls, this, name);
                        break;
                    case PtClassKind.Interface:
                        member = ClassType.Interface(cls, this, name);
                        break;
                    case PtClassKind.Primitive:
                        member = ClassType.Primitive(cls, this, name);
                        break;
                    case PtClassKind.Trait:
                        member = ClassType.Trait(cls, this, name);
                        break;
                    case PtClassKind.Struct:
                        member = ClassType.Struct(cls, this, name);
                        break;
                    case PtClassKind.Type:
                        member = ClassType.Alias(cls, this, name);
                        break;
                    default:
                        throw new System.NotImplementedException();
                }

                AddMember(member);
                member.DiscoverMembers();
            }
        }
    }
}