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
            Skip();

            var start = scanner.Current;
            var items = new List<Ast.Item>();

            while (true)
            {
                var kw = TryKeyword();

                if (kw != null)
                {
                    if (kw.Is("class"))
                    {
                        items.Add(Class());
                    }
                }
                else
                {
                    break;
                }
            }

            return new Ast.Unit(scanner.Span(start), items);
        }

        private Ast.Class Class()
        {
            var cap = TryRefCapablility();

            Skip();

            var id = Identifier();

            var docStrings = DocStrings().ToArray();

            var members = ClassMembers().ToArray();

            throw new NotImplementedException();
        }

        private IEnumerable<Ast.Member> ClassMembers()
        {
            while (true)
            {
                Skip();

                var keyword = TryKeyword();

                if (keyword != null)
                {
                    if (keyword.Is("let"))
                    {
                        yield return Let();
                        continue;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                throw new NotImplementedException();
            }
        }

        private Ast.Member Let()
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
                TryRefCapablility();
            }
            
            return null;
        }

        private Ast.Capability? TryCapability()
        {
            Skip();

            var start = scanner.Current;
            var kw = TryKeyword();

            if (kw != null)
            {
                if (kw.Is("iso"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Iso);
                }
                else if (kw.Is("val"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Val);
                }
                else if (kw.Is("ref"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Ref);
                }
                else if (kw.Is("box"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Box);
                }
                else if (kw.Is("trn"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Trn);
                }
                else if (kw.Is("tag"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Tag);
                }
            }

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

                SkipMatch(')');

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
                var capability = TryCapability();
                var name = TryIdentifier();
                var parameters = TryTypeParameters();
                SkipMatch('}');
            }

            return null;
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

                SkipMatch(']');
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
                @default = TypeArgument();
            }

            return new Ast.TypeParameter(scanner.Span(start), identifier, constraint, @default);
        }



        private Ast.Type BareLambdaType()
        {
            throw new NotImplementedException();
        }

        private Ast.Type FunType()
        {
            Match("{");
            Skip();
            var types = TypeArguments("(", ")").ToArray();
            Skip();
            Match("}");
            throw new NotImplementedException();
        }

        private IReadOnlyList<Ast.Type> TypeList()
        {
            var arguments = new List<Ast.Type>() { TypeArgument() };
            while (SkipMatch(','))
            {
                arguments.Add(TypeArgument());
            }

            return arguments;
        }

        private IReadOnlyList<Ast.Type> TypeArguments(string begin, string end)
        {
            Match(begin);

            var arguments = new List<Ast.Type>() { TypeArgument() };
            while (SkipMatch(','))
            {
                arguments.Add(TypeArgument());
            }
            Match(end);
            return arguments;
        }

        private Ast.Type TypeArgument()
        {
            return Type();
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

        private Ast.Capability? TryRefCapablility()
        {
            Skip();

            var start = scanner.Current;
            var kw = TryKeyword();

            if (kw != null)
            {
                if (kw.Is("iso"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Iso);
                }
                else if (kw.Is("val"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Val);
                }
                else if (kw.Is("ref"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Ref);
                }
                else if (kw.Is("box"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Box);
                }
                else if (kw.Is("trn"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Trn);
                }
                else if (kw.Is("tag"))
                {
                    return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Tag);
                }
            }

            return null;
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

                return new Ast.Identifier(scanner.Span(start));
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
