using System;
using System.Collections.Generic;
using System.Linq;

namespace Joke.Front.Pony
{
    public class PonyParser : Parser<PonyScanner>
    {
        public PonyParser(PonyScanner scanner)
            : base(scanner)
        {
        }

        public Ast.Unit Parse()
        {
            return Unit();
        }

        private Ast.Unit Unit()
        {
            var items = new List<Ast.Item>();

            while (scanner.Current < scanner.Limit)
            {
                var (start, prefix) = KeywordPrefix();

                switch (prefix)
                {
                    case "class":
                        items.Add(Class());
                        break;
                    default:
                        scanner.Current = start;
                        throw new NotImplementedException();
                }
            }

            return new Ast.Unit(scanner.Span(0), items);
        }

        private Ast.Class Class()
        {
            var cap = TryCapability();

            Skip();

            var id = Identifier();

            var docStrings = DocStrings().ToArray();

            var members = ClassMembers();

            throw new NotImplementedException();
        }

        private IReadOnlyList<Ast.Member> ClassMembers()
        {
            var done = false;

            var members = new List<Ast.Member>();

            while (!done)
            {
                var (start, prefix) = KeywordPrefix();

                switch(prefix)
                {
                    case "let":
                        members.Add(LetMember());
                        break;
                    case "new":
                        members.Add(NewMember());
                        break;
                    default:
                        scanner.Current = start;
                        done = true;
                        break;
                }
            }

            return members;
        }

        private Ast.Member NewMember()
        {
            var cap = TryCapability();
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = Parameters();

            var start = scanner.Current;

            return new Ast.NewMember(scanner.Span(start));
        }

        private IReadOnlyList<Ast.Parameter> Parameters()
        {
            Skip();

            var start = scanner.Current;
            var parameters = new List<Ast.Parameter>();

            Match('(');
            Skip();
            if (At() != ')')
            {
                do
                {
                    parameters.Add(Parameter());
                }
                while (SkipMatch(','));
            }
            Match(')');

            return parameters;
        }

        private Ast.Parameter Parameter()
        {
            throw new NotImplementedException();
        }

        private Ast.Member LetMember()
        {
            Skip();

            var start = scanner.Current;

            var id = Identifier();

            Skip();

            Match(":");

            var type = Type();

            var docs = DocStrings();

            return new Ast.LetMember(scanner.Span(start), id, type, docs);
        }

        private Ast.Type Type()
        {
            var atom = AtomType();

            if (atom == null)
            {
                throw new NotImplementedException();
            }

            return atom;
        }

        private Ast.Type? AtomType()
        {
            Ast.Type? type;

            type = TryThisType()
                ?? TryCapability()
                ?? TryGroupedType()
                ?? TryNominalType()
                ?? TryLambdaType()
                ;


            return type;
        }

        private Ast.Type? TryThisType()
        {
            Skip();
            if (Check("this"))
            {
                TryCapability();
            }
            
            return null;
        }

        private (int, string) KeywordPrefix(bool hash = false)
        {
            Skip();
            var start = scanner.Current;
            if (hash && At() == '#')
            {
                Match();
            }
            while (scanner.IsLetter())
            {
                Match();
            }

            return (start, scanner.Span(start).ToString());
        }

        private Ast.Capability? TryCapability()
        {
            (var start, var prefix) = KeywordPrefix();

            switch (prefix)
            {
                case "iso": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Iso);
                case "val": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Val);
                case "ref": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Ref);
                case "box": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Box);
                case "trn": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Trn);
                case "tag": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Tag);
            };

            scanner.Current = start;

            return null;
        }

        private Ast.Capability? TryHashCapability()
        {
            (var start, var prefix) = KeywordPrefix(true);

            switch (prefix)
            {
                case "#any": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.HashAny);
                case "#read": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.HashRead);
                case "#send": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.HashSend);
                case "#share": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.HashShare);
                case "#alias": return new Ast.Capability(scanner.Span(start), Ast.RefCapability.HashAlias);
            }

            scanner.Current = start;

            return null;
        }

        private Ast.Type? TryGroupedType()
        {
            if (SkipMatch('('))
            {
                var start = scanner.Current - 1;

                var types = new List<Ast.Type>() { InfixType() };

                while (SkipMatch(','))
                {
                    types.Add(InfixType());
                }

                Skip();
                Match(')');

                return new Ast.GroupedType(scanner.Span(start), types);
            }

            return null;
        }

        private Ast.Type InfixType()
        {
            return UnionType();
        }


        private Ast.Type UnionType()
        {
            var start = scanner.Current;

            var intersections = new List<Ast.Type>() { IntersectionType() };

            while (SkipMatch('|'))
            {
                intersections.Add(IntersectionType());
            }

            if (intersections.Count > 1)
            {
                return new Ast.UnionType(scanner.Span(start), intersections);
            }

            return intersections[0];
        }

        private Ast.Type IntersectionType()
        {
            var start = scanner.Current;

            var primaries = new List<Ast.Type>() { Type() };

            while (SkipMatch('&'))
            {
                primaries.Add(Type());
            }
            if (primaries.Count > 1)
            {
                return new Ast.IntersectionType(scanner.Span(start), primaries);
            }
            return primaries[0];
        }


        private Ast.Type? TryNominalType()
        {
            Skip();

            var start = scanner.Current;

            var id = TryIdentifier();
            if (id != null)
            {
                if (SkipMatch('.'))
                {
                    var id2 = Identifier();

                    id = new Ast.QualifiedIdentifier(scanner.Span(id.Span.Start), id, Enumerable.Repeat(id2, 1).ToArray());
                }

                IReadOnlyList<Ast.Type> arguments = Array.Empty<Ast.Type>();

                if (SkipMatch('['))
                {
                    arguments = TypeList();

                    SkipMatch(']');
                }

                var capability = TryCapability();

                return new Ast.NominalType(scanner.Span(start), id, arguments, capability);
            }

            return null;
        }

        private Ast.Type? TryLambdaType()
        {
            if (SkipMatch('{'))
            {
                var start = scanner.Current - 1;

                var capability = TryCapability();
                var identifier = TryIdentifier();
                var typeParameters = TryTypeParameters();
                
                Skip();
                Match('(');

                IReadOnlyList<Ast.Type>? argumentTypes = Array.Empty<Ast.Type>();

                if (!SkipMatch(')'))
                {
                    argumentTypes = TypeList();
                    Skip();
                    Match(')');
                }

                Ast.Type? @return = null;
                if (SkipMatch(':'))
                {
                    @return = Type();
                }

                SkipMatch('}');

                var cap = TryCapability();
                if (cap == null)
                {
                    cap = TryHashCapability();
                }

                return new Ast.LambdaType(
                    scanner.Span(start),
                    capability,
                    identifier,
                    typeParameters,
                    argumentTypes,
                    @return,
                    cap);
            }

            return null;
        }

        private Ast.Type BareLambdaType()
        {
            throw new NotImplementedException();
        }

        private IReadOnlyList<Ast.TypeParameter> TryTypeParameters()
        {
            var parameters = new List<Ast.TypeParameter>();

            if (SkipMatch('['))
            {
                parameters.Add(TypeParameter());

                while (SkipMatch(','))
                {
                    parameters.Add(TypeParameter());
                }

                Skip();
                Match(']');
            }

            return parameters;
        }

        private Ast.TypeParameter TypeParameter()
        {
            var start = scanner.Current;

            var identifier = Identifier();

            Ast.Type? constraint = null;

            if (SkipMatch(':'))
            {
                constraint = Type();
            }

            Ast.Type? @default = null;

            if (SkipMatch('='))
            {
                @default = Type();
            }

            return new Ast.TypeParameter(scanner.Span(start), identifier, constraint, @default);
        }

        private IReadOnlyList<Ast.Type> TypeList()
        {
            var arguments = new List<Ast.Type>() { Type() };
            while (SkipMatch(','))
            {
                arguments.Add(Type());
            }

            return arguments;
        }

        private IEnumerable<Ast.DocString> DocStrings()
        {
            Skip();

            while (scanner.Check("\"\"\""))
            {
                yield return DocString();

                Skip();
            }
        }

        private Ast.DocString DocString()
        {
            //TODO: not properly implemented
            var start = scanner.Current;

            Match("\"\"\"");
            while (true)
            {
                if (At() != '\"')
                {
                    MatchAny();
                    continue;
                }
                if (scanner.Check("\"\"\""))
                {
                    break;
                }
            }
            Match("\"\"\"");

            return new Ast.DocString(scanner.Span(start));
        }

        private Ast.Identifier? TryIdentifier()
        {
            if (scanner.IsLetter_())
            {
                var start = scanner.Current;
                do
                {
                    MatchAny();
                }
                while (scanner.IsLetterOrDigit_());

                var span = scanner.Span(start);

                if (!Keywords.IsKeyword(span))
                {
                    return new Ast.Identifier(scanner.Span(start));
                }

                scanner.Current = start;
            }

            return null;
        }

        private Ast.Keyword? TryKeyword()
        {
            if (scanner.IsLetter_())
            {
                var start = scanner.Current;
                do
                {
                    MatchAny();
                }
                while (scanner.IsLetter_());

                var span = scanner.Span(start);

                if (Keywords.IsKeyword(span))
                {
                    return new Ast.Keyword(scanner.Span(start));
                }

                scanner.Current = start;
            }

            return null;
        }

        private Ast.Identifier Identifier()
        {
            if (scanner.IsLetter_())
            {
                var start = scanner.Current;
                do
                {
                    MatchAny();
                }
                while (scanner.IsLetterOrDigit_());

                var span = scanner.Span(start);

                if (!Keywords.IsKeyword(span))
                {
                    return new Ast.Identifier(scanner.Span(start));
                }
            }

            throw new InvalidOperationException($"expected identifier");
        }
    }
}
