using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Ast.DocString? doc = null;

            while (scanner.Current < scanner.Limit)
            {
                var (start, prefix) = KeywordPrefix();

                switch (prefix)
                {
                    case "class":
                        items.Add(Class(Ast.ClassKind.Class));
                        break;
                    case "primitive":
                        items.Add(Class(Ast.ClassKind.Primitive));
                        break;
                    default:
                        if (scanner.Check("\"\"\""))
                        {
                            doc = DocString();
                        }
                        else
                        {
                            scanner.Current = start;
                            throw new NotImplementedException();
                        }
                        break;
                }
            }

            return new Ast.Unit(scanner.Span(0), doc, items);
        }

        private Ast.Class Class(Ast.ClassKind kind)
        {
            var cap = TryCapability();

            Skip();

            var id = Identifier();

            var docString = TryDocString();

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
                        members.Add(MethodMember(Ast.MemberKind.New));
                        break;
                    case "fun":
                        members.Add(MethodMember(Ast.MemberKind.Fun));
                        break;
                    default:
                        scanner.Current = start;
                        done = true;
                        break;
                }
            }

            return members;
        }

        private Ast.Member MethodMember(Ast.MemberKind kind)
        {
            var cap = TryCapability();
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = Parameters();

            Ast.Type? returnType = null;

            if (SkipMatch(':'))
            {
                returnType = Type();
            }

            var errors = SkipMatch('?');
            var docs = TryDocString();

            Ast.Expression? body = null;

            if (SkipMatch("=>"))
            {
                var docs2 = TryDocString();

                body = RawSeq();

            }

            var start = scanner.Current;

            return new Ast.MethodMember(scanner.Span(start), kind);
        }

        private Ast.Expression RawSeq()
        {
            return TryJump() ?? ExprSeq();
        }

        private Ast.Expression? TryJump()
        {
            var (start, prefix) = KeywordPrefix();

            Ast.Expression? value;

            switch (prefix)
            {
                case "return":
                    value = TryRawSeq();
                    return new Ast.Jump(scanner.Span(start), Ast.JumpKind.Return, value);
                case "break":
                    value = TryRawSeq();
                    return new Ast.Jump(scanner.Span(start), Ast.JumpKind.Break, value);
                case "continue":
                    value = TryRawSeq();
                    return new Ast.Jump(scanner.Span(start), Ast.JumpKind.Continue, value);
                case "error":
                    value = TryRawSeq();
                    return new Ast.Jump(scanner.Span(start), Ast.JumpKind.Error, value);
                case "compile_intrinsic":
                    value = TryRawSeq();
                    return new Ast.Jump(scanner.Span(start), Ast.JumpKind.CompileIntrinsic, value);
                case "compile_error":
                    value = TryRawSeq();
                    return new Ast.Jump(scanner.Span(start), Ast.JumpKind.CompileError, value);
            }

            return null;
        }

        private Ast.Expression? TryRawSeq()
        {
            try
            {
                return RawSeq();
            }
            catch
            {
                return null;
            }
        }

        private Ast.Expression ExprSeq()
        {
            var assign = Assignment();

            Skip();
            if (At() == ';')
            {
                SemiExpr();
            }
            else
            {
                NoSemi();
            }

            throw new NotImplementedException();
        }

        private Ast.Sequence Jump()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Assignment()
        {
            var infix = Infix();

            return infix;
            //TODO:
            //throw new NotImplementedException();
        }

        private Ast.Sequence SemiExpr()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression NoSemi()
        {
            return TryJump() ?? NextExprSeq();
        }

        private Ast.Expression NextExprSeq()
        {
            var assign = NextAssignment();

            throw new NotImplementedException();
        }

        private Ast.Sequence Semi()
        {
            throw new NotImplementedException();
        }

        private Ast.Sequence NextAssignment()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Infix()
        {
            var term = Term();

            return term;
            //TODO: throw new NotImplementedException();
        }

        private Ast.Expression Term()
        {
            var (start, prefix) = KeywordPrefix();

            switch (prefix)
            {
                case "if":
                    return Cond();
                case "ifdef":
                    return IfDef();
                case "iftype":
                case "match":
                case "while":
                case "repeat":
                case "for":
                case "with":
                case "try":
                case "recover":
                case "consume":
                    scanner.Current = start;
                    throw new NotImplementedException();

            }

            if (At() == '#')
            {
                return ConstExpr();
            }

            return Pattern(start, prefix);
        }

        private Ast.Expression ConstExpr()
        {
            var start = scanner.Current;

            Match('#');

            var expression = Postfix();

            return new Ast.Const(scanner.Span(start), expression);
        }

        private Ast.Expression ParamPattern()
        {
            var (start, prefix) = KeywordPrefix();

            return ParamPattern(start, prefix);
        }

        private Ast.Expression ParamPattern(int start, string prefix)
        {
            switch (prefix)
            {
                case "var":
                    return Local(start, Ast.MemberKind.Var);
                case "let":
                    return Local(start, Ast.MemberKind.Let);
                case "embed":
                    return Local(start, Ast.MemberKind.Embed);
            }

            return Pattern(start, prefix);
        }

        private Ast.Expression Pattern()
        {
            var (start, prefix) = KeywordPrefix();

            return Pattern(start, prefix);
        }

        private Ast.Expression Pattern(int start, string prefix)
        {
            switch (prefix)
            {
                case "addressof":
                    return Prefix(start, Ast.PrefixOp.AddressOf);
                case "digestof":
                    return Prefix(start, Ast.PrefixOp.DigestOf);
                case "not":
                    return Prefix(start, Ast.PrefixOp.Not);
            }

            switch (At())
            {
                case '-':
                    Match();
                    if (At() == '~')
                    {
                        Match();
                        return Prefix(start, Ast.PrefixOp.NegUnsafe);
                    }
                    return Prefix(start, Ast.PrefixOp.Neg);
            }

            return Postfix(start, prefix);
        }

        private Ast.Expression Prefix(int start, Ast.PrefixOp op)
        {
            var expression = ParamPattern();

            return new Ast.Prefix(scanner.Span(start), op, expression);
        }

        private Ast.Expression Postfix()
        {
            var (start, prefix) = KeywordPrefix();

            return Postfix(start, prefix);
        }

        private Ast.Expression Postfix(int start, string prefix)
        {
            var expression = Atom(start, prefix);

            var done = false;
            while (!done)
            {
                Skip();

                switch (At())
                {
                    case '.':
                        if (At(1) == '>')
                        {
                            Chain();
                        }
                        else
                        {
                            Dot();
                        }
                        break;
                    case '~':
                        Tilde();
                        break;
                    case '(':
                        expression = Call(expression);
                        break;
                    case '[':
                        Qualify();
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return expression;
        }

        private Ast.Expression Dot()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Chain()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Tilde()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Call(Ast.Expression atom)
        {
            Match('(');
            var positional = Positional();
            var named = Named();
            Match(')');

            var partial = SkipMatch('?');

            return new Ast.Call(scanner.Span(atom.Span.Start), atom, positional, named);
        }

        private IReadOnlyList<Ast.Argument> Positional()
        {
            var arguments = new List<Ast.Argument>();

            while (!CheckKeyword("where"))
            {
                if (At() == ')')
                {
                    break;
                }

                var expr = RawSeq();

                arguments.Add(new Ast.Argument(expr.Span, null, expr));

                if (!SkipMatch(','))
                {
                    break;
                }
            }

            return arguments;
        }

        private IReadOnlyList<Ast.Argument> Named()
        {
            var arguments = new List<Ast.Argument>();

            if (CheckKeyword("where"))
            {
                Match("where");

                Debug.Assert(false);

                arguments.Add(NamedArg());

                while (SkipMatch(','))
                {
                    arguments.Add(NamedArg());
                }
            }

            return arguments;
        }

        private Ast.Argument NamedArg()
        {
            var name = Identifier();
            Skip();
            Match("=");
            var value = RawSeq();

            return new Ast.Argument(scanner.Span(name.Span.Start), name, value);
        }

        private Ast.Expression Qualify()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Atom()
        {
            var (start, prefix) = KeywordPrefix();

            return Atom(start, prefix);
        }

        private Ast.Expression Atom(int start, string prefix)
        {
            if (prefix.Length == 0)
            {
                switch (At())
                {
                    case '\"':
                        return String();
                    case '(':
                        return GroupedExpr();
                    case '[':
                        return Array();
                    case '{':
                        return Lambda();
                    case '@':
                        if (At(1) == '{')
                        {
                            return BareLambda();
                        }
                        return FFI();

                }
            }

            switch (prefix)
            {
                case "this":
                    return new Ast.This(scanner.Span(start));
                case "object":
                    return Object();
                case "if":
                    return Cond();
                case "while":
                    return WhileLoop();
            }

            scanner.Current = start;

            return Ref();

        }

        private Ast.Expression Ref()
        {
            var name = Identifier();

            return new Ast.Ref(name.Span, name);
        }

        private Ast.Expression FFI()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression BareLambda()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Lambda()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Array()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression GroupedExpr()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression Object()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression? TryLocal()
        {
            return null;

            throw new NotImplementedException();
        }

        private Ast.Expression Local(int start, Ast.MemberKind kind)
        {
            var name = Identifier();
            Ast.Type? type = null;
            if (SkipMatch(':'))
            {
                type = Type();
            }

            return new Ast.Local(scanner.Span(start), name, type);
        }

        private Ast.Expression Cond()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression IfDef()
        {
            var condition = Infix();

            MatchKeyword("then");

            var thenExpression = Seq();

            //TODO:
            throw new NotImplementedException();
        }

        private Ast.Expression Seq()
        {
            return RawSeq();
        }

        private Ast.Expression WhileLoop()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression ForLoop()
        {
            throw new NotImplementedException();
        }

        private Ast.Sequence NextInfix()
        {
            throw new NotImplementedException();
        }

        private Ast.Sequence AssignOp()
        {
            throw new NotImplementedException();
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
            Skip();
            var start = scanner.Current;

            var name = Identifier();
            Skip();
            Match(':');
            var type = Type();
            Ast.Expression? defaultArg = null;

            if (SkipMatch('='))
            {
                defaultArg = DefaultArg();
            }

            return new Ast.Parameter(scanner.Span(start), name, type, defaultArg);
        }

        private Ast.Expression DefaultArg()
        {
            return Infix();
        }

        private Ast.Member LetMember()
        {
            Skip();

            var start = scanner.Current;

            var id = Identifier();

            Skip();

            Match(":");

            var type = Type();

            var docs = TryDocString();

            return new Ast.FieldMember(scanner.Span(start), Ast.MemberKind.Let, id, type,  docs);
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

        private (int, string) IdAlike(bool hash = false)
        {
            Skip();
            var start = scanner.Current;
            if (hash && At() == '#')
            {
                Match();
            }
            if (scanner.IsLetter_())
            {
                Match();
                while (scanner.IsLetterOrDigit_())
                {
                    Match();
                }
            }

            return (start, scanner.Span(start).ToString());
        }

        private void MatchKeyword(string keyword)
        {
            var (start, prefix) = IdAlike();

            if (prefix != keyword)
            {
                scanner.Current = start;

                throw new NotImplementedException();
            }
        }

        private bool CheckKeyword(string keyword)
        {
            var (start, prefix) = IdAlike();

            var check = prefix == keyword;

            scanner.Current = start;

            return check;
        }


        private (int, string) KeywordPrefix(bool hash = false)
        {
            Skip();
            var start = scanner.Current;
            if (hash && At() == '#')
            {
                Match();
            }
            while (scanner.IsLetter_())
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

                IReadOnlyList<Ast.Type> arguments = System.Array.Empty<Ast.Type>();

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

                IReadOnlyList<Ast.Type>? argumentTypes = System.Array.Empty<Ast.Type>();

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

        private Ast.DocString? TryDocString()
        {
            Skip();
            if (Check("\"\"\""))
            {
                return DocString();
            }

            return null;
        }

        private Ast.Expression String()
        {
            if (Check("\"\"\""))
            {
                return DocString();
            }

            //TODO: not properly implemented
            var start = scanner.Current;

            Match("\"");
            while (true)
            {
                if (At() != '\"')
                {
                    MatchAny();
                    continue;
                }
                else
                {
                    break;
                }
            }
            Match("\"");

            return new Ast.String(scanner.Span(start));
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
