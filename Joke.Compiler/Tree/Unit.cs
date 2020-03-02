using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Joke.Front.Pony.ParseTree;
using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class Unit : IDeclare
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

                switch (cls.Kind)
                {
                    case PtClassKind.Actor:
                        AddMember(new Actor(cls, this, name));
                        break;
                    case PtClassKind.Class:
                        AddMember(new Class(cls, this, name));
                        break;
                    case PtClassKind.Interface:
                        AddMember(new Interface(cls, this, name));
                        break;
                    case PtClassKind.Primitive:
                        AddMember(new Primitive(cls, this, name));
                        break;
                    case PtClassKind.Trait:
                        AddMember(new Trait(cls, this, name));
                        break;
                    case PtClassKind.Struct:
                        AddMember(new Struct(cls, this, name));
                        break;
                    case PtClassKind.Type:
                        AddMember(new TypeAlias(cls, this, name));
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
            }
        }
    }
}