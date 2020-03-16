using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Joke.Joke.Err;

namespace Joke.Joke.Syntax
{
    public class Package
    {
        public Package(IReadOnlyList<Tree.Unit> source)
        {
            Units = new List<Unit>();
            Members = new TopMemberlist();
            Errors = new Errors();
            Source = source;
        }

        public List<Unit> Units { get; }
        public TopMemberlist Members { get; }
        public Errors Errors { get; }
        public IReadOnlyList<Tree.Unit> Source { get; }

        public void Populate()
        {
            foreach (var sourceUnit in Source)
            {
                var unit = new Unit(this, sourceUnit);

                foreach (var pkgMember in sourceUnit.Members.OfType<Tree.ClassType>())
                {
                    var name = pkgMember.Name;
                    var member = new TopMember(unit, pkgMember);

                    if (!Members.TryAdd(name, member))
                    {
                        AlreadyError(name);
                    }

                    foreach (var classMember in pkgMember.Members)
                    {
                        if (classMember is Tree.Field field)
                        {

                        }
                        else if (classMember is Tree.Method method)
                        {

                        }
                        else
                        {
                            Debug.Assert(false);
                        }
                    }
                }

                Units.Add(unit);
            }
        }

        private void AlreadyError(Tree.Identifier name)
        {
            var already = Members[name].Source.Name;
            Errors.AtToken(ErrNo.Syntax001, name, $"``{name}´´ defined here ...");
            Errors.AtToken(ErrNo.Syntax002, already, $"... is already defined here");
        }
    }
}
