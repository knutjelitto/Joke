using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Joke.Joke.Err;
using Joke.Joke.Tools;

namespace Joke.Joke.Syntax
{
    public class Package : DistinctList<string, Unit>
    {
        public Package()
        {
            Units = new List<Unit>();
            Members = new DistinctList<Tree.Identifier, Class>();
            Errors = new Errors();
        }

        public List<Unit> Units { get; }
        public DistinctList<Tree.Identifier, Class> Members { get; }
        public Errors Errors { get; }

        public void Populate(IEnumerable<Tree.Unit> source)
        {
            foreach (var unitSource in source)
            {
                var unit = new Unit(this, unitSource);

                var noNotNamed = unitSource.Members.All(m => m is Tree.INamedMember);
                Debug.Assert(noNotNamed);

                Populate(unit, unitSource.Members.OfType<Tree.INamedMember>());

                Units.Add(unit);
            }
        }

        private void Populate(Unit unit, IEnumerable<Tree.INamedMember> members)
        {
            foreach (var classSource in members.OfType<Tree.ClassType>())
            {
                var @class = new Class(unit, classSource);

                var name = classSource.Name;
                var member = new Class(unit, classSource);

                if (!MaybeAlreadyError(Members, name))
                {
                    unit.Add(name, member);
                }

                Populate(@class, classSource.Members.OfType<Tree.INamedMember>().ToList());
            }
        }

        private void Populate(Class @class, IReadOnlyList<Tree.INamedMember> sources)
        {
            foreach (var source in sources)
            {
                if (source is Tree.Field fieldSource)
                {
                    if (!MaybeAlreadyError(@class, fieldSource.Name))
                    {
                        var field = new Field(@class, fieldSource);
                        @class.Add(field.Name, field);
                    }
                }
                else if (source is Tree.Method methodSource)
                {
                    if (!MaybeAlreadyError(@class, methodSource.Name))
                    {
                        var method = new Method(@class, methodSource);
                        @class.Add(method.Name, method);
                    }
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        private bool MaybeAlreadyError<T>(DistinctList<Tree.Identifier,T> list, Tree.Identifier name)
            where T : class, ISourcedName
        {
            if (list.ContainsKey(name))
            {
                var already = list[name].Source.Name;
                Errors.AtToken(ErrNo.Syntax001, name, $"``{name}´´ defined here ...");
                Errors.AtToken(ErrNo.Syntax002, already, $"... is already defined here");
                return true;
            }
            return false;
        }
    }
}
