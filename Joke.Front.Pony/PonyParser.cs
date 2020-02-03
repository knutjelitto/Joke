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

            var type = CapType();

            var docs = DocStrings();

            return new Ast.LetMember(scanner.Span(start), id, type, docs);
        }

        private Ast.Type CapType()
        {
            var start = scanner.Current;

            var type = Type();
            var capability = TryRefCapablility();
            return new Ast.CapType(scanner.Span(start), type, capability); ;
        }

        private Ast.Type Type()
        {
            return UnionType();
        }

        private Ast.Type UnionType()
        {
            var start = scanner.Current;

            var first = IntersectionType();
            Skip();
            if (At() == '|')
            {
                var intersections = new List<Ast.Type>() { first };
                do
                {
                    Match("|");
                    intersections.Add(IntersectionType());
                    Skip();
                }
                while (At() == '|');

                return new Ast.UnionType(scanner.Span(start), intersections);
            }

            return first;
        }

        private Ast.Type IntersectionType()
        {
            var start = scanner.Current;

            var first = PrimaryType();
            Skip();
            if (At() == '&')
            {
                var primaries = new List<Ast.Type>() { first };
                do
                {
                    Match("&");
                    primaries.Add(PrimaryType());
                    Skip();
                }
                while (At() == '&');

                return new Ast.IntersectionType(scanner.Span(start), primaries);

            }

            return first;
        }

        private Ast.Type PrimaryType()
        {
            Skip();

            var id = TryIdentifier();

            if (id != null)
            {
                IEnumerable<Ast.Type> arguments = Enumerable.Empty<Ast.Type>();

                if (At() == '[')
                {
                    arguments = TypeArguments();
                }

                return new Ast.TypeName(id.Span, id, arguments);

            }
            if (At() == '(')
            {
                Match("(");
                var type = CapType();
                Skip();
                Match(")");

                return type;
            }

            throw new NotImplementedException();
        }

        private IEnumerable<Ast.Type> TypeArguments()
        {
            Match("[");
            while (true)
            {
                Skip();
                yield return TypeArgument();
                Skip();
                if (At() == ',')
                {
                    continue;
                }
                break;
            }
            Match("]");
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

        private Ast.Capability TryRefCapablility()
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

                throw new NotImplementedException();
            }

            return new Ast.Capability(scanner.Span(start), Ast.RefCapability.Default);
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
