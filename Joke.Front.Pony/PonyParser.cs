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

        private enum IN
        {
            Plain,
            Next,
            Case
        }

        private Ast.Unit Unit()
        {
            var items = new List<Ast.Item>();
            Ast.DocString? doc = null;

            while (scanner.Current < scanner.Limit)
            {
                var (start, prefix) = KeywordPrefix();

                if (start >= scanner.Limit)
                {
                    break;
                }

                switch (prefix)
                {
                    case "class":
                        items.Add(Class(Ast.ClassKind.Class));
                        break;
                    case "actor":
                        items.Add(Class(Ast.ClassKind.Actor));
                        break;
                    case "primitive":
                        items.Add(Class(Ast.ClassKind.Primitive));
                        break;
                    case "interface":
                        items.Add(Class(Ast.ClassKind.Interface));
                        break;
                    case "type":
                        items.Add(Class(Ast.ClassKind.Type));
                        break;
                    case "use":
                        items.Add(Use());
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

            return new Ast.Unit(Span(0), doc, items);
        }

        private Ast.Use Use()
        {
            Skip();

            var start = scanner.Current;

            var name = TryIdentifier();

            if (name != null)
            {
                Skip();
                Match('=');
            }

            Skip();
            if (Check('@'))
            {
                Eat();
                Ast.Expression  ffiName;
                if (Check('"'))
                {
                    ffiName = String();
                }
                else
                {
                    ffiName = Identifier();
                }
                var typeArgs = TypeArgs();
                Skip();
                var parameters = Parameters();
                var partial = SkipMatch('?');

                return new Ast.UseFfi(Span(start), name, ffiName, typeArgs, parameters, partial);
            }
            else if (Check('"'))
            {
                var url = String();

                return new Ast.UseUrl(Span(start), name, url);
            }

            throw new NotImplementedException();
        }

        private Ast.Class Class(Ast.ClassKind kind)
        {
            Skip();

            var start = scanner.Current;

            var cap = TryCapability();

            Skip();

            var name = Identifier();

            var typeArguments = TryTypeParameters();

            Ast.Type? type = null;

            if (CheckKeyword("is"))
            {
                Eat(2);
                type = Type();
            }

            var docString = TryDocString();

            var members = ClassMembers();

            return new Ast.Class(Span(start), kind, cap, name, docString, members);
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
                        members.Add(FieldMember(start, Ast.MemberKind.Let));
                        break;
                    case "var":
                        members.Add(FieldMember(start, Ast.MemberKind.Var));
                        break;
                    case "embed":
                        members.Add(FieldMember(start, Ast.MemberKind.Embed));
                        break;
                    case "new":
                        members.Add(MethodMember(Ast.MemberKind.New));
                        break;
                    case "fun":
                        members.Add(MethodMember(Ast.MemberKind.Fun));
                        break;
                    case "be":
                        members.Add(MethodMember(Ast.MemberKind.Be));
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
            Skip();
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = Parameters();
            var returnType = TryColonType(); ;

            var partial = SkipMatch('?');
            var docs = TryDocString();

            Ast.Expression? body = null;

            if (SkipMatch("=>"))
            {
                var docs2 = TryDocString();

                body = RawSeq();

            }

            var start = scanner.Current;

            return new Ast.MethodMember(Span(start), kind);
        }

        private Ast.Expression? TryJump()
        {
            var (start, prefix) = KeywordPrefix();

            Ast.Expression? value;

            switch (prefix)
            {
                case "return":
                    value = TryRawSeq();
                    return new Ast.Jump(Span(start), Ast.JumpKind.Return, value);
                case "break":
                    value = TryRawSeq();
                    return new Ast.Jump(Span(start), Ast.JumpKind.Break, value);
                case "continue":
                    value = TryRawSeq();
                    return new Ast.Jump(Span(start), Ast.JumpKind.Continue, value);
                case "error":
                    value = TryRawSeq();
                    return new Ast.Jump(Span(start), Ast.JumpKind.Error, value);
                case "compile_intrinsic":
                    value = TryRawSeq();
                    return new Ast.Jump(Span(start), Ast.JumpKind.CompileIntrinsic, value);
                case "compile_error":
                    value = TryRawSeq();
                    return new Ast.Jump(Span(start), Ast.JumpKind.CompileError, value);
            }

            scanner.Current = start;

            return null;
        }

        private Ast.Expression RawSeq() => TryRawSeq() ?? throw NoParse("RawSeq");

        private Ast.Expression? TryRawSeq()
        {
            return TryExprSeq() ?? TryJump();
        }

        private Ast.Expression ExprSeq(IN next = IN.Plain) => TryExprSeq(next) ?? throw NoParse("ExprSeq");

        private Ast.Expression? TryExprSeq(IN next = IN.Plain)
        {
            var assign = TryAssignment(next);

            if (assign == null)
            {
                return null;
            }

            Skip();
            if (Check(';'))
            {
                var rest = SemiExpr();

                return new Ast.Sequence(Span(assign.Span.Start), assign, rest);
            }
            else if (!Check(','))
            {
                var rest = TryNoSemi();

                if (rest == null)
                {
                    return assign;
                }

                return new Ast.Sequence(Span(assign.Span.Start), assign, rest);
            }
            else
            {
                return assign;
            }
        }

        private Ast.Expression Assignment(IN next = IN.Plain) => TryAssignment(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryAssignment(IN next = IN.Plain)
        {
            var lhs = TryInfix(next);

            if (lhs == null)
            {
                return null;
            }

            if (SkipMatch('='))
            {
                var rhs = Assignment();

                return new Ast.Assignment(Span(lhs.Span.Start), lhs, rhs);
            }

            return lhs;
        }

        private Ast.Expression Infix(IN next = IN.Plain) => TryInfix(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryInfix(IN next = IN.Plain)
        {
            var term = TryTerm(next);

            if (term == null)
            {
                return null;
            }

            var done = false;
            while (!done)
            {
                Skip();

                var rhs = TryBinOp(term);
                if (rhs != null)
                {
                    term = rhs;
                }
                else
                {
                    if (CheckKeyword("as"))
                    {
                        MatchKeyword("as");
                        var type = Type();

                        term = new Ast.As(Span(term.Span.Start), term, type);
                    }
                    else if (CheckKeyword("is"))
                    {
                        MatchKeyword("is");
                        var term2 = Term();

                        term = new Ast.Is(Span(term.Span.Start), term, term2);
                    }
                    else
                    {
                        done = true;
                    }
                }
            }

            return term;
        }

        private Ast.Expression Term(IN next = IN.Plain) => TryTerm(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryTerm(IN next = IN.Plain)
        {
            var (start, prefix) = KeywordPrefix();

            switch (prefix)
            {
                case "if":
                    return Cond(start);
                case "ifdef":
                    return IfDef(start);
                case "match":
                    return MatchExpression(start);
                case "while":
                    return WhileLoop(start);
                case "recover":
                    return Recover(start);
                case "consume":
                    return Consume(start);
                case "try":
                    return TryBlock(start);
                case "for":
                    return ForLoop(start);
                case "repeat":
                    return RepeatLoop(start);
                case "iftype":
                case "with":
                    scanner.Current = start;
                    throw NotYet(prefix);
            }

            scanner.Current = start;

            if (Check('#'))
            {
                return ConstExpr();
            }

            return TryPattern(next);
        }

        private Ast.Expression TryBlock(int start)
        {
            var body = Seq();
            var @else = TryElse(); ;
            Ast.Expression? then = null;
            if (CheckKeyword("then"))
            {
                then = RawSeq();
            }
            MatchKeyword("end");

            return new Ast.TryBlock(Span(start), body, @else, then);
        }

        private Ast.Expression Recover(int start)
        {
            var caps = TryCapability();
            var body = Seq();
            MatchKeyword("end");

            return new Ast.Recover(Span(start), caps, body);
        }

        private Ast.Expression Consume(int start)
        {
            var caps = TryCapability();
            var term = Term();

            return new Ast.Consume(Span(start), caps, term);
        }

        private Ast.Expression? TryElse()
        {
            if (CheckKeyword("else"))
            {
                MatchKeyword("else");
                return RawSeq();
            }

            return null;
        }

        private Ast.Expression RepeatLoop(int start)
        {
            var body = Seq();
            MatchKeyword("until");
            var condition = RawSeq();
            var @else = TryElse();
            MatchKeyword("end");

            return new Ast.RepeatLoop(Span(start), body, condition, @else);
        }

        private Ast.Expression ForLoop(int start)
        {
            var ids = IdSeq();
            MatchKeyword("in");
            var iterator = RawSeq();
            MatchKeyword("do");
            var body = RawSeq();
            var @else = TryElse();
            MatchKeyword("end");

            return new Ast.ForLoop(Span(start), ids, iterator, body, @else);
        }

        private Ast.IdSeq IdSeq()
        {
            Skip();
            if (Check('('))
            {
                return IdSeqMulti();
            }

            var start = scanner.Current;

            var name = Identifier();

            return new Ast.IdSeqSingle(Span(start), name);
        }

        private Ast.IdSeq IdSeqMulti()
        {
            Debug.Assert(!scanner.CanSkip());

            var start = scanner.Current;

            Match('(');
            var names = new List<Ast.IdSeq>();
            do
            {
                var name = IdSeq();
                names.Add(name);
            }
            while (SkipMatch(','));
            Match(')');

            return new Ast.IdSeqMulti(Span(start), names);
        }


        private Ast.Expression WhileLoop(int start)
        {
            var condition = RawSeq();
            MatchKeyword("do");
            var body = Seq();
            var @else = TryElse();
            MatchKeyword("end");

            return new Ast.While(Span(condition.Span.Start), condition, body, @else);
        }

        private Ast.Match MatchExpression(int start)
        {
            var toMatch = RawSeq();
            var cases = Cases();
            var @else = TryElse();
            MatchKeyword("end");

            return new Ast.Match(Span(start), toMatch, cases, @else);
        }

        private IReadOnlyList<Ast.Case> Cases()
        {
            var cases = new List<Ast.Case>();

            while (true)
            {
                var @case = TryCase();
                if (@case != null)
                {
                    cases.Add(@case);
                }
                else
                {
                    return cases;
                }
            }
        }

        private Ast.Case? TryCase()
        {
            Skip();
            var start = scanner.Current;

            if (Check('|'))
            {
                Eat();

                var pattern = TryPattern(IN.Case);
                Ast.Expression? guard = null;
                Ast.Expression? body = null;
                if (SkipMatch("id"))
                {
                    guard = RawSeq();
                }
                if (SkipMatch("=>"))
                {
                    body = RawSeq();
                }

                return new Ast.Case(Span(start), pattern, guard, body);
            }

            return null; ;
        }

        private Ast.Expression ParamPattern(IN next = IN.Plain) => TryParamPattern(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryParamPattern(IN next = IN.Plain)
        {
            return TryPrefix() ?? TryPostfix();
        }

        private Ast.Expression? TryPrefix()
        {
            var (start, prefix) = KeywordPrefix();

            switch (prefix)
            {
                case "addressof":
                    return PrefixOp(start, Ast.PrefixOp.AddressOf);
                case "digestof":
                    return PrefixOp(start, Ast.PrefixOp.DigestOf);
                case "not":
                    return PrefixOp(start, Ast.PrefixOp.Not);
            }

            switch (At())
            {
                case '-':
                    Eat();
                    if (Check('~'))
                    {
                        Eat();
                        return PrefixOp(start, Ast.PrefixOp.NegUnsafe);
                    }
                    return PrefixOp(start, Ast.PrefixOp.Neg);
            }

            scanner.Current = start;

            return null;
        }

        private Ast.Expression Postfix(IN next = IN.Plain) => TryPostfix(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryPostfix(IN next = IN.Plain)
        {
            var expression = TryAtom(next);

            if (expression == null)
            {
                return null;
            }

            var done = false;
            while (!done)
            {
                var nl = Skip(); ;

                switch (At())
                {
                    case '.':
                        if (At(1) == '>')
                        {
                            expression = Chain(expression);
                        }
                        else
                        {
                            expression = Dot(expression);
                        }
                        break;
                    case '~':
                        Tilde();
                        break;
                    case '(' when !nl:
                        expression = Call(expression);
                        break;
                    case '[' when !nl:
                        Qualify();
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return expression;
        }

        private Ast.Expression SemiExpr()
        {
            Match(';');

            return RawSeq();
        }

        private Ast.Expression NoSemi() => TryNoSemi() ?? throw NoParse("NoSemi");

        private Ast.Expression? TryNoSemi()
        {
            return TryExprSeq(IN.Next) ?? TryJump();
        }

        private Ast.Sequence Semi()
        {
            throw new NotImplementedException();
        }

        private Ast.Expression? TryBinOp(Ast.Expression lhs)
        {
            Debug.Assert(!scanner.CanSkip());

            var start = scanner.Current;
            var op = Ast.InfixOp.NONE;

            switch (At())
            {
                case '+':
                    if (At(1) == '~')
                    {
                        Eat(2);
                        op = Ast.InfixOp.PlusUnsafe;
                    }
                    else
                    {
                        Eat(1);
                        op = Ast.InfixOp.Plus;
                    }
                    break;
                case '-':
                    if (At(1) == '~')
                    {
                        Eat(2);
                        op = Ast.InfixOp.MinusUnsafe;
                    }
                    else
                    {
                        Eat(1);
                        op = Ast.InfixOp.Minus;
                    }
                    break;
                case '*':
                    if (At(1) == '~')
                    {
                        Eat(2);
                        op = Ast.InfixOp.MultiplyUnsafe;
                    }
                    else
                    {
                        Eat(1);
                        op = Ast.InfixOp.Multiply;
                    }
                    break;
                case '/':
                    if (At(1) == '~')
                    {
                        Eat(2);
                        op = Ast.InfixOp.DivideUnsafe;
                    }
                    else
                    {
                        Eat(1);
                        op = Ast.InfixOp.Divide;
                    }
                    break;
                case '%':
                    if (At(1) == '~')
                    {
                        Eat(2);
                        op = Ast.InfixOp.RemUnsafe;
                    }
                    else if (At(1) == '%')
                    {
                        if (At(2) == '~')
                        {
                            Eat(3);
                            op = Ast.InfixOp.ModUnsafe;
                        }
                        else
                        {
                            Eat(2);
                            op = Ast.InfixOp.Mod;
                        }
                    }
                    else
                    {
                        Eat(1);
                        op = Ast.InfixOp.Rem;
                    }
                    break;
                case '=':
                    if (At(1) == '=')
                    {
                        if (At(2) == '~')
                        {
                            Eat(3);
                            op = Ast.InfixOp.EqUnsafe;
                        }
                        else
                        {
                            Eat(2);
                            op = Ast.InfixOp.Eq;
                        }
                    }
                    break;
                case '!':
                    if (At(1) == '=')
                    {
                        if (At(2) == '~')
                        {
                            Eat(3);
                            op = Ast.InfixOp.NeUnsafe;
                        }
                        else
                        {
                            Eat(2);
                            op = Ast.InfixOp.Ne;
                        }
                    }
                    break;
                case '<':
                    if (At(1) == '<')
                    {
                        if (At(2) == '~')
                        {
                            Eat(3);
                            op = Ast.InfixOp.LShiftUnsafe;
                        }
                        else
                        {
                            Eat(2);
                            op = Ast.InfixOp.LShift;
                        }
                    }
                    else if (At(1) == '~')
                    {
                        Eat(2);
                        op = Ast.InfixOp.LtUnsafe;
                    }
                    else if (At(1) == '=')
                    {
                        if (At(2) == '~')
                        {
                            Eat(3);
                            op = Ast.InfixOp.LeUnsafe;
                        }
                        else
                        {
                            Eat(2);
                            op = Ast.InfixOp.Le;
                        }
                    }
                    else
                    {
                        Eat(1);
                        op = Ast.InfixOp.Lt;
                    }
                    break;
                case '>':
                    if (At(1) == '>')
                    {
                        if (At(2) == '~')
                        {
                            Eat(3);
                            op = Ast.InfixOp.RShiftUnsafe;
                        }
                        else
                        {
                            Eat(2);
                            op = Ast.InfixOp.RShift;
                        }
                    }
                    else if (At(1) == '~')
                    {
                        Eat(2);
                        op = Ast.InfixOp.GtUnsafe;
                    }
                    else if (At(1) == '=')
                    {
                        if (At(2) == '~')
                        {
                            Eat(3);
                            op = Ast.InfixOp.GeUnsafe;
                        }
                        else
                        {
                            Eat(2);
                            op = Ast.InfixOp.Ge;
                        }
                    }
                    else
                    {
                        Eat(1);
                        op = Ast.InfixOp.Gt;
                    }
                    break;
                default:
                    if (CheckKeyword("and"))
                    {
                        Eat(3);
                        op = Ast.InfixOp.And;
                    }
                    if (CheckKeyword("or"))
                    {
                        Eat(2);
                        op = Ast.InfixOp.Or;
                    }
                    if (CheckKeyword("xor"))
                    {
                        Eat(3);
                        op = Ast.InfixOp.Xor;
                    }
                    break;

            }

            if (op != Ast.InfixOp.NONE)
            {
                var rhs = Term();

                return new Ast.Infix(Span(start), op, lhs, rhs);
            }

            return null;

        }

        private Ast.Expression ConstExpr()
        {
            var start = scanner.Current;

            Match('#');

            var expression = Postfix();

            return new Ast.Const(Span(start), expression);
        }

        private Ast.Expression? TryLocal()
        {
            var (start, prefix) = KeywordPrefix();

            switch (prefix)
            {
                case "var":
                    return LocalOp(start, Ast.MemberKind.Var);
                case "let":
                    return LocalOp(start, Ast.MemberKind.Let);
                case "embed":
                    return LocalOp(start, Ast.MemberKind.Embed);
            }

            scanner.Current = start;

            return null;
        }

        private Ast.Expression Pattern(IN next = IN.Plain) => TryPattern(next) ?? throw new NotImplementedException();


        private Ast.Expression? TryPattern(IN next = IN.Plain)
        {
            return TryLocal() ?? TryParamPattern(next);
        }

        private Ast.Expression PrefixOp(int start, Ast.PrefixOp op)
        {
            var expression = ParamPattern();

            return new Ast.Prefix(Span(start), op, expression);
        }


        private Ast.Postfix Dot(Ast.Expression postfixed)
        {
            var start = scanner.Current;

            Eat(1); // .

            Skip();
            var memberName = Identifier();

            return new Ast.Dot(Span(start), postfixed, memberName);
        }

        private Ast.Expression Chain(Ast.Expression postfixed)
        {
            var start = scanner.Current;

            Eat(2); // '.>'
            Skip();
            var memberName = Identifier();

            return new Ast.Chain(Span(start), postfixed, memberName);
        }

        private Ast.Expression Tilde()
        {
            throw NotYet("~");
        }

        private Ast.Expression Call(Ast.Expression atom)
        {
            Match('(');
            var positional = Positional();
            var named = Named();
            Match(')');

            var partial = SkipMatch('?');

            return new Ast.Call(Span(atom.Span.Start), atom, positional, named);
        }

        private IReadOnlyList<Ast.Argument> Positional()
        {
            var arguments = new List<Ast.Argument>();

            while (!CheckKeyword("where"))
            {
                if (Check(')'))
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

            return new Ast.Argument(Span(name.Span.Start), name, value);
        }

        private Ast.TypeArguments Qualify()
        {
            return TypeArgs();
        }

        private Ast.Expression Atom(IN next = IN.Plain) => TryAtom(next) ?? throw NoParse("Atom");

        private Ast.Expression? TryAtom(IN next = IN.Plain)
        {
            var (start, prefix) = KeywordPrefix();

            if (prefix.Length == 0)
            {
                switch (At())
                {
                    case '\"':
                        return String();
                    case '\'':
                        return Char();
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
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        return Number();
                }
            }

            switch (prefix)
            {
                case "this":
                    return new Ast.This(Span(start));
                case "object":
                    return Object();
                case "if":
                    return Cond(start);
                case "while":
                    return WhileLoop(start);
            }

            scanner.Current = start;

            return TryRef();
        }

        private Ast.Expression Number()
        {
            if (CheckDigit())
            {
                var start = scanner.Current;

                do
                {
                    Eat();
                }
                while (CheckDigit());

                if (At() == '.')
                {
                    do
                    {
                        Eat();
                    }
                    while (CheckDigit());
                }

                return new Ast.Integer(Span(start));
            }

            throw NoParse("Number");
        }

        private Ast.Expression Ref() => TryRef() ?? throw NoParse("Ref");

        private Ast.Expression? TryRef()
        {
            var name = TryIdentifier();

            if (name != null)
            {
                return new Ast.Ref(name.Span, name);
            }

            return null;
        }

        private Ast.Expression FFI()
        {
            Debug.Assert(!scanner.CanSkip());

            var start = scanner.Current;

            Match('@');

            var name = Identifier();

            var @return = TryTypeArgs();

            Skip();
            Match('(');
            var positional = Positional();
            var named = Named();
            Match(')');

            var partial = SkipMatch('?');

            return new Ast.FfiCall(Span(start), name, positional, named, partial);
        }

        private Ast.TypeArguments TypeArgs()
        {
            return TryTypeArgs() ?? throw NoParse("TypeArgs");
        }

        private Ast.TypeArguments? TryTypeArgs()
        {
            Skip();

            var start = scanner.Current;

            if (Check('['))
            {
                Eat();

                var args = new List<Ast.TypeArgument>();

                do
                {
                    args.Add(TypeArg());
                }
                while (SkipMatch(','));

                Match(']');

                return new Ast.TypeArguments(Span(start), args);
            }

            return null;
        }

        private Ast.TypeArgument TypeArg()
        {
            var type = Type();

            return new Ast.TypeArgType(type.Span, type);
        }

        private Ast.Expression BareLambda()
        {
            throw NotYet("bare lambda");
        }

        private Ast.Expression Lambda()
        {
            Debug.Assert(!scanner.CanSkip());

            var start = scanner.Current;

            Match('{');

            var recCap = TryCapability();
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaParameters();
            var captures = TryLambdaCaptures();
            var @return = TryColonType();
            var partial = SkipMatch('?');
            Match("=>");
            var body = RawSeq();
            Skip();
            Match('}');
            var refCap = TryCapability();

            return new Ast.Lambda(Span(start), recCap, name, typeParameters, parameters, captures, @return, partial, body, refCap);
            
            throw NotYet("lambda");
        }

        private IReadOnlyList<Ast.LambdaParameter> LambdaParameters()
        {
            SkipMatch('(');

            var parameters = new List<Ast.LambdaParameter>();

            if (!SkipMatch(')'))
            {
                do
                {
                    parameters.Add(LambdaParameter());
                }
                while (SkipMatch(','));
            }

            Match(')');

            return parameters;
        }

        private Ast.LambdaParameter LambdaParameter()
        {
            Skip();

            var start = scanner.Current;

            var name = Identifier();
            var type = TryColonType();
            var value = TryDefaultArg();

            return new Ast.LambdaParameter(Span(start), name, type, value);
        }

        private IReadOnlyList<Ast.LambdaCapture> TryLambdaCaptures()
        {
            if (SkipMatch('('))
            {
                var captures = new List<Ast.LambdaCapture>();

                if (!SkipMatch(')'))
                {
                    do
                    {
                        captures.Add(LambdaCapture());
                    }
                    while (SkipMatch(','));
                }

                Match(')');

                return captures;
            }

            return new List<Ast.LambdaCapture>();
        }


        private Ast.LambdaCapture LambdaCapture()
        {
            Skip();

            var start = scanner.Current;

            if (CheckKeyword("this"))
            {
                MatchKeyword("this");

                return new Ast.LambdaCaptureThis(Span(start));
            }

            var name = Identifier();
            var type = TryColonType();
            var value = TryDefaultArg();

            return new Ast.LambdaCaptureName(Span(start), name, type, value);
        }

        private Ast.Type ColonType() => TryColonType() ?? throw NoParse(": type");

        private Ast.Type? TryColonType()
        {
            if (SkipMatch(':'))
            {
                return Type();
            }

            return null;
        }

        private Ast.Expression Array(IN next = IN.Plain) => TryArray(next) ?? throw NoParse("array");

        private Ast.Expression? TryArray(IN next = IN.Plain)
        {
            if (Check('['))
            {
                var start = scanner.Current;

                Eat();

                var type = TryArrayType();

                var elements = RawSeq();

                Skip();
                Match(']');

                return new Ast.Array(Span(start), type, elements);
            }

            throw NotYet("Array");
        }

        private Ast.Type? TryArrayType()
        {
            if (CheckKeyword("as"))
            {
                MatchKeyword("as");

                var type = Type();

                Skip();
                Match(':');

                return type;
            }

            return null;
        }

        private Ast.Expression GroupedExpr(IN next = IN.Plain) => TryGroupedExpr(next) ?? throw NoParse("GroupedExpr");

        private Ast.Expression? TryGroupedExpr(IN next = IN.Plain)
        {
            Skip();

            var start = scanner.Current;

            if (Check('('))
            {
                Eat();

                var values = new List<Ast.Expression>();

                values.Add(RawSeq());

                while (SkipMatch(','))
                {
                    values.Add(RawSeq());
                }

                Match(')');

                return new Ast.Tuple(Span(start), values);
            }

            return null;
        }

        private Ast.Expression? TryTuple()
        {
            throw NotYet("TryTuple");
        }

        private Ast.Expression Object()
        {
            throw NotYet("Object");
        }

        private Ast.Expression LocalOp(int start, Ast.MemberKind kind)
        {
            Skip();
            var name = Identifier();
            var type = TryColonType();

            return new Ast.Local(Span(start), kind, name, type);
        }

        private Ast.Expression Cond(int start)
        {
            return Iff(start, RawSeq);
        }


        private Ast.Expression IfDef(int start)
        {
            return Iff(start, () => Infix());
        }

        private Ast.Expression Iff(int start, Func<Ast.Expression> parseCondition)
        {
            var condition = parseCondition();
            MatchKeyword("then");
            var thenExpression = Seq();

            var iffs = new List<Ast.IfThen>();

            iffs.Add(new Ast.IfThen(Span(start), condition, thenExpression));

            while (true)
            {
                var (_, prefix) = KeywordPrefix();

                switch (prefix)
                {
                    case "elseif":
                        condition = parseCondition();
                        MatchKeyword("then");
                        thenExpression = Seq();
                        iffs.Add(new Ast.IfThen(Span(start), condition, thenExpression));
                        break;
                    case "else":
                        thenExpression = Seq();
                        MatchKeyword("end");
                        return new Ast.IfThenElse(Span(start), iffs, thenExpression);
                    case "end":
                        return new Ast.IfThenElse(Span(start), iffs, null);
                }
            }
        }

        private Ast.Expression Seq()
        {
            return RawSeq();
        }

        private Ast.Expression ForLoop()
        {
            throw NotYet("ForLoop");
        }

        private Ast.Sequence AssignOp()
        {
            throw NotYet("AssignOp");
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
            var type = ColonType(); ;
            var defaultArg = TryDefaultArg();

            return new Ast.Parameter(Span(start), name, type, defaultArg);
        }

        private Ast.Expression DefaultArg => TryDefaultArg() ?? throw NoParse("default value");

        private Ast.Expression? TryDefaultArg()
        {
            if (SkipMatch('='))
            {
                return Infix();
            }

            return null;
        }

        private Ast.Member FieldMember(int start, Ast.MemberKind kind)
        {
            Skip();

            var id = Identifier();

            Skip();

            Match(":");

            var type = Type();

            Ast.Expression? value = null;

            if (SkipMatch('='))
            {
                value = Infix();
            }

            var docs = TryDocString();

            return new Ast.FieldMember(Span(start), kind, id, type, value, docs);
        }

        private Ast.Type Type()
        {
            var atom = AtomType();

            Skip();
            if (At() == '-' && At(1) == '>')
            {
                var start = scanner.Current;

                Eat(2);

                var type = Type();

                return new Ast.ViewPoint(Span(start), atom, type);
            }


            return atom;
        }

        private Ast.Type AtomType() => TryAtomType() ?? throw NoParse("type atom");

        private Ast.Type? TryAtomType()
        {
            return TryThisType()
                ?? TryCapability()
                ?? TryGroupedType()
                ?? TryNominalType()
                ?? TryLambdaType()
                ;
        }

        private Ast.Type? TryThisType()
        {
            Skip();

            var start = scanner.Current;

            if (CheckKeyword("this"))
            {
                MatchKeyword("this");

                return new Ast.ThisType(Span(start));
            }
            
            return null;
        }

        private (int, string) IdAlike(bool withHash = false)
        {
            Skip();
            var start = scanner.Current;
            if (withHash && Check('#'))
            {
                Eat();
            }
            if (scanner.IsLetter_())
            {
                Eat();
                while (scanner.IsLetterOrDigit_())
                {
                    Eat();
                }
            }

            return (start, Span(start).ToString());
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

        private (int, string) KeywordPrefix(bool withHash = false)
        {
            Skip();
            var start = scanner.Current;
            if (withHash && Check('#'))
            {
                Eat();
            }
            while (scanner.IsLetter_())
            {
                Eat();
            }

            return (start, Span(start).ToString());
        }

        private Ast.Capability? TryCapability()
        {
            (var start, var prefix) = KeywordPrefix();

            switch (prefix)
            {
                case "iso": return new Ast.Capability(Span(start), Ast.RefCapability.Iso);
                case "val": return new Ast.Capability(Span(start), Ast.RefCapability.Val);
                case "ref": return new Ast.Capability(Span(start), Ast.RefCapability.Ref);
                case "box": return new Ast.Capability(Span(start), Ast.RefCapability.Box);
                case "trn": return new Ast.Capability(Span(start), Ast.RefCapability.Trn);
                case "tag": return new Ast.Capability(Span(start), Ast.RefCapability.Tag);
            };

            scanner.Current = start;

            return null;
        }

        private Ast.Capability? TryHashCapability()
        {
            Debug.Assert(!scanner.CanSkip());

            if (Check('#'))
            {
                var start = scanner.Current;

                Eat();
                (var _, var prefix) = KeywordPrefix(true);

                switch (prefix)
                {
                    case "any": return new Ast.Capability(Span(start), Ast.RefCapability.HashAny);
                    case "read": return new Ast.Capability(Span(start), Ast.RefCapability.HashRead);
                    case "send": return new Ast.Capability(Span(start), Ast.RefCapability.HashSend);
                    case "share": return new Ast.Capability(Span(start), Ast.RefCapability.HashShare);
                    case "alias": return new Ast.Capability(Span(start), Ast.RefCapability.HashAlias);
                }

                scanner.Current = start;
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

                Skip();
                Match(')');

                return new Ast.GroupedType(Span(start), types);
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
                return new Ast.UnionType(Span(start), intersections);
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
                return new Ast.IntersectionType(Span(start), primaries);
            }
            return primaries[0];
        }


        private Ast.Type? TryNominalType()
        {
            Skip();

            var start = scanner.Current;

            var name = TryIdentifier();
            if (name != null)
            {
                if (SkipMatch('.'))
                {
                    var name2 = Identifier();

                    name = new Ast.QualifiedIdentifier(Span(name.Span.Start), name, Enumerable.Repeat(name2, 1).ToArray());
                }

                var arguments = TryTypeArgs();

                var capability = TryCapability();

                if (capability == null)
                {
                    capability = TryHashCapability();
                }

                var nominalKind = Ast.NominalKind.Plain;
                Skip();
                if (Check('^'))
                {
                    Eat();
                    nominalKind = Ast.NominalKind.Ephemeral;
                }
                else if (Check('!'))
                {
                    Eat();
                    nominalKind = Ast.NominalKind.Aliased;
                }

                return new Ast.NominalType(Span(start), name, arguments, capability, nominalKind);
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

                var @return = TryColonType(); 

                SkipMatch('}');

                var cap = TryCapability();
                if (cap == null)
                {
                    cap = TryHashCapability();
                }

                return new Ast.LambdaType(
                    Span(start),
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
                do
                {
                    parameters.Add(TypeParameter());
                }
                while (SkipMatch(','));

                Match(']');
            }

            return parameters;
        }

        private Ast.TypeParameter TypeParameter()
        {
            Skip();

            var start = scanner.Current;

            var identifier = Identifier();

            var constraint = TryColonType();

            Ast.Type? @default = null;

            if (SkipMatch('='))
            {
                @default = Type();
            }

            return new Ast.TypeParameter(Span(start), identifier, constraint, @default);
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

        private Ast.Char Char()
        {
            var start = scanner.Current;

            Match('\'');
            if (Check('\\'))
            {
                Eat();
                switch (At())
                {
                    case 'n':
                    case 'r':
                        Eat();
                        break;
                    default:
                        throw NoParse("illegal character");
                }
            }
            else
            {
                Eat();
            }
            Match('\'');

            return new Ast.Char(Span(start));
        }

        private Ast.String String()
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
                    Eat();
                    continue;
                }
                else
                {
                    break;
                }
            }
            Match("\"");

            return new Ast.String(Span(start));
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
                    Eat();
                    continue;
                }
                if (scanner.Check("\"\"\""))
                {
                    break;
                }
                Eat();
            }
            Match("\"\"\"");

            return new Ast.DocString(Span(start));
        }

        private Ast.Identifier Identifier() => TryIdentifier() ?? throw new NotImplementedException("identifier expected");

        private Ast.Identifier? TryIdentifier()
        {
            if (scanner.IsLetter_())
            {
                var start = scanner.Current;
                do
                {
                    Eat();
                }
                while (scanner.IsLetterOrDigit_());

                while (Check('\''))
                {
                    Eat();
                }

                var span = Span(start);

                if (!Keywords.IsKeyword(span))
                {
                    return new Ast.Identifier(Span(start));
                }

                scanner.Current = start;
            }

            return null;
        }

        private NotYetException NotYet(string message)
        {
            return new NotYetException(message);
        }

        private NoParseException NoParse(string message)
        {
            return new NoParseException(message);
        }
    }
}
