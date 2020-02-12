﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Joke.Front.Pony
{
    public class PonyParser2 : PonyParserBase
    {
        public PonyParser2(PonyScanner scanner)
            : base(scanner)
        {
        }

        public Ast.Unit Parse()
        {
            return Unit();
        }

        private enum IN
        {
            Both,
            Next,
            Case
        }

        private Ast.Unit Unit()
        {
            var items = new List<Ast.Item>();
            Ast.DocString? doc = null;

            while (More())
            {
                var (_, prefix) = KeywordPrefix();

                switch (prefix)
                {
                    case "class":
                        items.Add(Class(Ast.ClassKind.Class));
                        break;
                    case "struct":
                        items.Add(Class(Ast.ClassKind.Struct));
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
                    case "trait":
                        items.Add(Class(Ast.ClassKind.Trait));
                        break;
                    case "type":
                        items.Add(Class(Ast.ClassKind.Type));
                        break;
                    case "use":
                        items.Add(Use());
                        break;
                    default:
                        doc = TryDocString();
                        break;
                }
            }

            return new Ast.Unit(Span(0), doc, items);
        }

        private Ast.Use Use()
        {
            Skip();

            var start = GetStart();

            var name = TryIdentifier();

            if (name != null)
            {
                SkipMatch('=');
            }

            if (TrySkipMatch('@'))
            {
                Ast.ExternIdentifier ffiName;
                if (Check('"'))
                {
                    ffiName = new Ast.ExternIdentifierString(String().Span);
                }
                else
                {
                    ffiName = new Ast.ExternIdentifierPlain(Identifier().Span);
                }
                var typeArgs = TypeArgs();
                var parameters = Parameters();
                var partial = SkipIff('?');
                var condition = TryCondition();

                return new Ast.UseFfi(Span(start), name, ffiName, typeArgs, parameters, partial, condition);
            }
            else if (Check('"'))
            {
                var url = String();
                var condition = TryCondition();

                return new Ast.UseUrl(Span(start), name, url, condition);
            }

            throw NotYet("use");
        }

        private Ast.Expression? TryCondition()
        {
            Skip();
            if (TryMatchKeyword("if"))
            {
                return Infix();
            }

            return null;
        }

        private Ast.Class Class(Ast.ClassKind kind)
        {
            Skip();

            var start = GetStart();

            var cap = TryCapability();
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var provides = TryProvides();
            var doc = TryDocString();
            var members = ClassMembers();

            return new Ast.Class(Span(start), kind, cap, name, typeParameters, provides, doc, members);
        }

        private IReadOnlyList<Ast.Member> ClassMembers()
        {
            var done = false;

            var members = new List<Ast.Member>();

            while (!done)
            {
                var (start, prefix) = KeywordPrefix();

                switch (prefix)
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
                        members.Add(MethodMember(start, Ast.MemberKind.New));
                        break;
                    case "fun":
                        members.Add(MethodMember(start, Ast.MemberKind.Fun));
                        break;
                    case "be":
                        members.Add(MethodMember(start, Ast.MemberKind.Be));
                        break;
                    default:
                        SetStart(start);
                        done = true;
                        break;
                }
            }

            return members;
        }

        private Ast.Member MethodMember(int start, Ast.MemberKind kind)
        {
            var receiverCap = TryCapability();
            var bare = false;
            if (receiverCap == null)
            {
                bare = TrySkipMatch('@');
            }
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = Parameters();
            var returnType = TryColonType();
            var partial = TrySkipMatch('?');
            var docs = TryDocString();
            var body = TryBody();

            return new Ast.MethodMember(Span(start), kind, receiverCap, bare, name, typeParameters, parameters, returnType, partial, docs, body);
        }

        private Ast.Expression? TryBody()
        {
            if (TrySkipMatch("=>"))
            {
                return RawSeq();

            }

            return null;
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

            SetStart(start);

            return null;
        }

        private Ast.Expression RawSeq() => TryRawSeq() ?? throw NoParse("raw-sequence");

        private Ast.Expression? TryRawSeq()
        {
            return TryExprSeq() ?? TryJump();
        }

        private Ast.Expression ExprSeq(IN next = IN.Both) => TryExprSeq(next) ?? throw NoParse("exprssion-sequence");

        private Ast.Expression? TryExprSeq(IN next = IN.Both)
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

        private Ast.Expression Assignment(IN next = IN.Both) => TryAssignment(next) ?? throw NoParse("assignment");

        private Ast.Expression? TryAssignment(IN next = IN.Both)
        {
            var lhs = TryInfix(next);

            if (lhs == null)
            {
                return null;
            }

            Skip();
            if (At() == '=' && At(1) != '>')
            {
                Eat(1);

                var rhs = Assignment();

                return new Ast.Assignment(Span(lhs.Span.Start), lhs, rhs);
            }

            return lhs;
        }

        private Ast.Type? TryProvides()
        {
            if (TryMatchKeyword("is"))
            {
                return Type();
            }

            return null;
        }

        private Ast.Expression Infix(IN next = IN.Both) => TryInfix(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryInfix(IN next = IN.Both)
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
                    if (TryMatchKeyword("as"))
                    {
                        var type = Type();

                        term = new Ast.As(Span(term.Span.Start), term, type);
                    }
                    else if (TryMatchKeyword("is"))
                    {
                        var term2 = Term();

                        term = new Ast.Is(Span(term.Span.Start), false, term, term2);
                    }
                    else if (TryMatchKeyword("isnt"))
                    {
                        var term2 = Term();

                        term = new Ast.Is(Span(term.Span.Start), true, term, term2);
                    }
                    else
                    {
                        done = true;
                    }
                }
            }

            return term;
        }

        private Ast.Expression Term(IN next = IN.Both) => TryTerm(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryTerm(IN next = IN.Both)
        {
            var (start, prefix) = KeywordPrefix();

            switch (prefix)
            {
                case "if":
                    return IfCond(start);
                case "ifdef":
                    return IfDef(start);
                case "iftype":
                    return IfType(start);
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
                case "with":
                    return With(start);
            }

            SetStart(start);

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
            if (TryMatchKeyword("then"))
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
            if (TryMatchKeyword("else"))
            {
                return RawSeq();
            }

            return null;
        }

        private Ast.Expression RepeatLoop(int start)
        {
            var body = Seq();
            MatchKeyword("until");
            var condition = RawSeq();
            var elseBody = TryElse();
            MatchKeyword("end");

            return new Ast.RepeatLoop(Span(start), body, condition, elseBody);
        }

        private Ast.Expression ForLoop(int start)
        {
            var ids = IdSeq();
            MatchKeyword("in");
            var iterator = RawSeq();
            MatchKeyword("do");
            var body = RawSeq();
            var elseBody = TryElse();
            MatchKeyword("end");

            return new Ast.ForLoop(Span(start), ids, iterator, body, elseBody);
        }

        private Ast.Expression WhileLoop(int start)
        {
            var condition = RawSeq();
            MatchKeyword("do");
            var body = Seq();
            var elseBody = TryElse();
            MatchKeyword("end");

            return new Ast.While(Span(start), condition, body, elseBody);
        }

        private Ast.IdSeq IdSeq()
        {
            Skip();
            if (Check('('))
            {
                return IdSeqMulti();
            }

            var name = Identifier();

            return new Ast.IdSeqSingle(Span(name.Span.Start), name);
        }

        private Ast.IdSeq IdSeqMulti()
        {
            Debug.Assert(!scanner.CanSkip());

            var start = GetStart();

            Match('(');
            var names = new List<Ast.IdSeq>();
            do
            {
                names.Add(IdSeq());
            }
            while (TrySkipMatch(','));
            Match(')');

            return new Ast.IdSeqMulti(Span(start), names);
        }

        private Ast.WithElement WithElement()
        {
            var names = IdSeq();
            Skip();
            Match('=');
            var initializer = RawSeq();

            return new Ast.WithElement(Span(names.Span.Start), names, initializer);
        }


        private IReadOnlyList<Ast.WithElement> WithElements()
        {
            var elements = new List<Ast.WithElement>();

            do
            {
                var element = WithElement();

                elements.Add(element);
            }
            while (TrySkipMatch(','));

            return elements;
        }

        private Ast.With With(int start)
        {
            var elements = WithElements();
            Skip();
            Match("do");
            var body = RawSeq();
            var elseBody = TryElse();
            Skip();
            Match("end");

            return new Ast.With(Span(start), elements, body, elseBody);
        }

        private Ast.Match MatchExpression(int start)
        {
            var toMatch = RawSeq();
            var cases = Cases();
            var elseBody = TryElse();
            MatchKeyword("end");

            return new Ast.Match(Span(start), toMatch, cases, elseBody);
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
            var start = GetStart();

            if (TryMatchPunctuation('|'))
            {
                Eat(1);

                var pattern = TryPattern(IN.Case);
                Ast.Expression? guard = null;
                Ast.Expression? body = null;
                if (TrySkipMatch("if"))
                {
                    guard = RawSeq();
                }
                if (TrySkipMatch("=>"))
                {
                    body = RawSeq();
                }

                return new Ast.Case(Span(start), pattern, guard, body);
            }

            return null; ;
        }

        private Ast.Expression ParamPattern(IN next = IN.Both) => TryParamPattern(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryParamPattern(IN next = IN.Both)
        {
            return TryPrefix() ?? TryPostfix(next);
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
                    Eat(1);
                    if (Check('~'))
                    {
                        Eat(1);
                        return PrefixOp(start, Ast.PrefixOp.NegUnsafe);
                    }
                    return PrefixOp(start, Ast.PrefixOp.Neg);
            }

            SetStart(start);

            return null;
        }

        private Ast.Expression Postfix(IN next = IN.Both) => TryPostfix(next) ?? throw new NotImplementedException();

        private Ast.Expression? TryPostfix(IN next = IN.Both)
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
                        expression = Tilde(expression);
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

            var start = GetStart();
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
                    if (TryMatchKeyword("and"))
                    {
                        op = Ast.InfixOp.And;
                    }
                    if (TryMatchKeyword("or"))
                    {
                        op = Ast.InfixOp.Or;
                    }
                    if (TryMatchKeyword("xor"))
                    {
                        op = Ast.InfixOp.Xor;
                    }
                    break;

            }

            if (op != Ast.InfixOp.NONE)
            {
                var @unsafe = TrySkipMatch('?');

                var rhs = Term();

                return new Ast.Infix(Span(start), op, @unsafe, lhs, rhs);
            }

            return null;

        }

        private Ast.Expression ConstExpr()
        {
            var start = GetStart();

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

            SetStart(start);

            return null;
        }

        private Ast.Expression Pattern(IN next = IN.Both) => TryPattern(next) ?? throw new NotImplementedException();


        private Ast.Expression? TryPattern(IN next = IN.Both)
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
            var start = GetStart();

            Match('.');

            var memberName = Identifier();

            return new Ast.Dot(Span(start), postfixed, memberName);
        }

        private Ast.Expression Chain(Ast.Expression postfixed)
        {
            var start = GetStart();

            Match(".>");

            var memberName = Identifier();

            return new Ast.Chain(Span(start), postfixed, memberName);
        }

        private Ast.Expression Tilde(Ast.Expression expression)
        {
            var start = GetStart();

            Match('~');

            var name = Identifier();

            return new Ast.Tilde(Span(start), expression, name);
        }

        private Ast.Expression Call(Ast.Expression atom)
        {
            Match('(');
            var positional = Positional();
            var named = Named();
            Match(')');

            var partial = TrySkipMatch('?');

            return new Ast.Call(Span(atom.Span.Start), atom, positional, named, partial);
        }

        private IReadOnlyList<Ast.Argument> Positional()
        {
            var arguments = new List<Ast.Argument>();

            while (!IsKeyword("where"))
            {
                if (IsPunctuation(')'))
                {
                    break;
                }

                var expr = RawSeq();

                arguments.Add(new Ast.Argument(expr.Span, null, expr));

                if (!TrySkipMatch(','))
                {
                    break;
                }
            }

            return arguments;
        }

        private IReadOnlyList<Ast.Argument> Named()
        {
            var arguments = new List<Ast.Argument>();

            if (TryMatchKeyword("where"))
            {
                arguments.Add(NamedArg());

                while (TrySkipMatch(','))
                {
                    arguments.Add(NamedArg());
                }
            }

            return arguments;
        }

        private Ast.Argument NamedArg()
        {
            var name = Identifier();
            SkipMatch('=');
            var value = RawSeq();

            return new Ast.Argument(Span(name.Span.Start), name, value);
        }

        private Ast.TypeArguments Qualify()
        {
            return TypeArgs();
        }

        private Ast.Expression Atom(IN next = IN.Both) => TryAtom(next) ?? throw NoParse("Atom");

        private Ast.Expression? TryAtom(IN next = IN.Both)
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
                            return Lambda();
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
                    return Object(start);
                case "if" when next != IN.Case:
                    return IfCond(start);
                case "while":
                    return WhileLoop(start);
                case "for":
                    return ForLoop(start);
            }

            SetStart(start);

            return TryRef();
        }

        private Ast.Expression Number()
        {
            if (IsDigit())
            {
                var start = GetStart();

                if (Check("0x"))
                {
                    Eat(2);

                    MatchHexDigit();

                    while (IsHexDigit())
                    {
                        Eat(1);
                    }

                    return new Ast.Integer(Span(start));
                }

                do
                {
                    Eat(1);
                }
                while (IsDigit() || At() == '_');

                if (At() == '.')
                {
                    do
                    {
                        Eat(1);
                    }
                    while (IsDigit());

                    if (At() == 'e' || At() == 'E')
                    {
                        Eat(1);

                        if (At() == '-')
                        {
                            Eat(1);
                        }

                        MatchDigit();
                        while (IsDigit())
                        {
                            Eat(1);
                        }
                    }
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

            var start = GetStart();

            Match('@');

            Ast.ExternIdentifier name;

            if (At() == '"')
            {
                name = new Ast.ExternIdentifierString(String().Span);
            }
            else
            {
                name = new Ast.ExternIdentifierPlain(Identifier().Span);
            }

            var returnType = TryTypeArgs();

            Skip();
            Match('(');
            var positional = Positional();
            var named = Named();
            Match(')');

            var partial = TrySkipMatch('?');

            return new Ast.FfiCall(Span(start), name, positional, named, partial);
        }

        private Ast.TypeArguments TypeArgs()
        {
            return TryTypeArgs() ?? throw NoParse("TypeArgs");
        }

        private Ast.TypeArguments? TryTypeArgs()
        {
            Skip();

            var start = GetStart();

            if (Check('['))
            {
                Eat(1);

                var args = new List<Ast.TypeArgument>();

                do
                {
                    args.Add(TypeArg());
                }
                while (TrySkipMatch(','));

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

        private Ast.Expression Lambda()
        {
            Debug.Assert(!scanner.CanSkip());

            var start = GetStart();

            var bare = Iff(At() == '@', Eat);

            MatchPunctuation('{');

            var receiverCap = TryCapability();
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaParameters();
            var captures = TryLambdaCaptures();
            var @return = TryColonType();
            var partial = SkipIff('?');
            Skip();
            Match("=>");
            var body = RawSeq();
            MatchPunctuation('}');
            var referenceCap = TryCapability();

            return new Ast.Lambda(Span(start), bare, receiverCap, name, typeParameters, parameters, captures, @return, partial, body, referenceCap);
        }

        private IReadOnlyList<Ast.LambdaParameter> LambdaParameters()
        {
            TrySkipMatch('(');

            var parameters = new List<Ast.LambdaParameter>();

            if (!TrySkipMatch(')'))
            {
                do
                {
                    parameters.Add(LambdaParameter());
                }
                while (TrySkipMatch(','));

                Match(')');
            }

            return parameters;
        }

        private Ast.LambdaParameter LambdaParameter()
        {
            var name = Identifier();
            var type = TryColonType();
            var value = TryDefaultValue();

            return new Ast.LambdaParameter(Span(name.Span.Start), name, type, value);
        }

        private IReadOnlyList<Ast.LambdaCapture> TryLambdaCaptures()
        {
            if (TrySkipMatch('('))
            {
                var captures = new List<Ast.LambdaCapture>();

                if (!TrySkipMatch(')'))
                {
                    do
                    {
                        captures.Add(LambdaCapture());
                    }
                    while (TrySkipMatch(','));
                }

                Match(')');

                return captures;
            }

            return new List<Ast.LambdaCapture>();
        }


        private Ast.LambdaCapture LambdaCapture()
        {
            Skip();

            var start = GetStart();

            if (TryMatchKeyword("this"))
            {
                return new Ast.LambdaCaptureThis(Span(start));
            }

            var name = Identifier();
            var type = TryColonType();
            var value = TryDefaultValue();

            return new Ast.LambdaCaptureName(Span(start), name, type, value);
        }

        private Ast.Type ColonType() => TryColonType() ?? throw NoParse(": type");

        private Ast.Type? TryColonType()
        {
            if (TrySkipMatch(':'))
            {
                return Type();
            }

            return null;
        }

        private Ast.Expression Array(IN next = IN.Both) => TryArray(next) ?? throw NoParse("array");

        private Ast.Expression? TryArray(IN next = IN.Both)
        {
            if (Check('['))
            {
                var start = GetStart();

                Eat(1);

                var type = TryArrayType();

                var elements = TryRawSeq();

                Skip();
                Match(']');

                return new Ast.Array(Span(start), type, elements);
            }

            return null;
        }

        private Ast.Type? TryArrayType()
        {
            if (TryMatchKeyword("as"))
            {
                var type = Type();

                Skip();
                Match(':');

                return type;
            }

            return null;
        }

        private Ast.Expression GroupedExpr(IN next = IN.Both) => TryGroupedExpr(next) ?? throw NoParse("GroupedExpr");

        private Ast.Expression? TryGroupedExpr(IN next = IN.Both)
        {
            var nl = Skip();

            var start = GetStart();

            if (Check('(') && (next == IN.Both || !nl))
            {
                Eat(1);

                var values = new List<Ast.Expression>();

                values.Add(RawSeq());

                while (TrySkipMatch(','))
                {
                    values.Add(RawSeq());
                }

                Match(')');

                return new Ast.Tuple(Span(start), values);
            }

            return null;
        }

        private Ast.Expression Object(int start)
        {
            var cap = TryCapability();
            var provides = TryProvides();
            var members = ClassMembers();
            MatchKeyword("end");

            return new Ast.Object(Span(start), cap, provides, members);
        }

        private Ast.Expression LocalOp(int start, Ast.MemberKind kind)
        {
            var name = Identifier();
            var type = TryColonType();

            return new Ast.Local(Span(start), kind, name, type);
        }

        private Ast.Expression IfCond(int start)
        {
            return Iff(start, RawSeq);
        }


        private Ast.Expression IfDef(int start)
        {
            return Iff(start, () => Infix());
        }

        private Ast.Expression IfType(int start)
        {
            return Iff(start, Subtype);
        }

        private Ast.Expression Subtype()
        {
            Skip();
            var start = GetStart();
            var sub = Type();
            Skip();
            Match("<:");
            var type = Type();

            return new Ast.SubType(Span(start), sub, type);
        }

        private void SkipAttributes()
        {
            Skip();
            if (At() == '\\')
            {
                Eat();
                do
                {
                    Identifier();
                    Skip();
                }
                while (At() != '\\');
                Eat();
            }
        }

        private Ast.Expression Iff(int start, Func<Ast.Expression> parseCondition)
        {
            SkipAttributes();
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
                        SkipAttributes();
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

        private Ast.Parameters Parameters()
        {
            Skip();

            var start = GetStart();

            Match('(');
            var parameters = new List<Ast.Parameter>();
            var ellipsis = false;
            Skip();
            if (At() != ')')
            {
                do
                {
                    Skip();
                    if (Check("..."))
                    {
                        Eat(3);
                        Skip();
                        ellipsis = true;
                        break;
                    }
                    parameters.Add(Parameter());
                }
                while (TrySkipMatch(','));
            }
            Match(')');

            return new Ast.Parameters(Span(start), parameters, ellipsis);
        }

        private Ast.Parameter Parameter()
        {
            var name = Identifier();
            var type = ColonType(); ;
            var defaultArg = TryDefaultValue();

            return new Ast.Parameter(Span(name.Span.Start), name, type, defaultArg);
        }

        private Ast.Expression DefaultValue => TryDefaultValue() ?? throw NoParse("default value");

        private Ast.Expression? TryDefaultValue()
        {
            if (TrySkipMatch('='))
            {
                return Infix();
            }

            return null;
        }

        private Ast.Member FieldMember(int start, Ast.MemberKind kind)
        {
            var id = Identifier();
            SkipMatch(':');
            var type = Type();
            var value = TryDefaultValue();
            var doc = TryDocString();

            return new Ast.FieldMember(Span(start), kind, id, type, value, doc);
        }

        private Ast.Type Type()
        {
            var atom = AtomType();

            Skip();
            if (At() == '-' && At(1) == '>')
            {
                var start = GetStart();

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

            var start = GetStart();

            if (TryMatchKeyword("this"))
            {
                return new Ast.ThisType(Span(start));
            }
            
            return null;
        }

        private (int, string) KeywordPrefix(bool withHash = false)
        {
            Skip();
            var start = GetStart();
            if (withHash && Check('#'))
            {
                Eat(1);
            }
            while (scanner.IsLetter_())
            {
                Eat(1);
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

            SetStart(start);

            return null;
        }

        private Ast.Capability? TryHashCapability()
        {
            Debug.Assert(!scanner.CanSkip());

            var start = GetStart();
            if (TryMatch('#'))
            {
                (var _, var prefix) = KeywordPrefix(true);

                switch (prefix)
                {
                    case "any": return new Ast.Capability(Span(start), Ast.RefCapability.HashAny);
                    case "read": return new Ast.Capability(Span(start), Ast.RefCapability.HashRead);
                    case "send": return new Ast.Capability(Span(start), Ast.RefCapability.HashSend);
                    case "share": return new Ast.Capability(Span(start), Ast.RefCapability.HashShare);
                    case "alias": return new Ast.Capability(Span(start), Ast.RefCapability.HashAlias);
                }

                SetStart(start);
            }

            return null;
        }

        private Ast.Type? TryGroupedType()
        {
            Skip();

            if (Check('('))
            {
                var start = GetStart();

                Eat(1);

                var types = new List<Ast.Type>() { InfixType() };

                while (TrySkipMatch(','))
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
            var start = GetStart();

            var intersections = new List<Ast.Type>() { IntersectionType() };

            while (TrySkipMatch('|'))
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
            var start = GetStart();

            var primaries = new List<Ast.Type>() { Type() };

            while (TrySkipMatch('&'))
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

            var start = GetStart();

            var name = TryIdentifier();
            if (name != null)
            {
                if (TrySkipMatch('.'))
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
                    Eat(1);
                    nominalKind = Ast.NominalKind.Ephemeral;
                }
                else if (Check('!'))
                {
                    Eat(1);
                    nominalKind = Ast.NominalKind.Aliased;
                }

                return new Ast.NominalType(Span(start), name, arguments, capability, nominalKind);
            }

            return null;
        }

        private Ast.Type? TryLambdaType()
        {
            Debug.Assert(!scanner.CanSkip());

            var start = GetStart();
            var bare = At() == '@';

            if (TryMatch('{') || TryMatch("@{"))
            {
                var capability = TryCapability();
                var identifier = TryIdentifier();
                var typeParameters = TryTypeParameters();


                SkipMatchPunctuation('(');

                IReadOnlyList<Ast.Type>? argumentTypes = System.Array.Empty<Ast.Type>();

                if (!TrySkipMatch(')'))
                {
                    argumentTypes = TypeList();
                    Skip();
                    Match(')');
                }

                var @return = TryColonType();
                var partial = TrySkipMatch('?');
                SkipMatchPunctuation('}');

                var cap = TryCapability();
                if (cap == null)
                {
                    cap = TryHashCapability();
                }

                return new Ast.LambdaType(
                    Span(start),
                    bare,
                    capability,
                    identifier,
                    typeParameters,
                    argumentTypes,
                    @return,
                    partial,
                    cap);
            }

            return null;
        }

        private Ast.TypeParameters TryTypeParameters()
        {
            var parameters = new List<Ast.TypeParameter>();

            Skip();

            var start = GetStart();

            if (TryMatch('['))
            {
                do
                {
                    parameters.Add(TypeParameter());
                }
                while (TrySkipMatch(','));

                Match(']');
            }

            return new Ast.TypeParameters(Span(start), parameters);
        }

        private Ast.TypeParameter TypeParameter()
        {
            var identifier = Identifier();
            var constraint = TryColonType();
            var defaultType = TryDefaultType();

            return new Ast.TypeParameter(Span(identifier.Span.Start), identifier, constraint, defaultType);
        }

        private Ast.Type? TryDefaultType()
        {
            if (TrySkipMatch('='))
            {
                return Type();
            }

            return null;
        }

        private IReadOnlyList<Ast.Type> TypeList()
        {
            var arguments = new List<Ast.Type>();
            do
            {
                arguments.Add(Type());
            }
            while (TrySkipMatch(','));

            return arguments;
        }

        private void MatchHexN(int n, string inWhat)
        {
            for (var i = 0; i < n; ++i)
            {
                if (IsHexDigit())
                {
                    Eat(1);
                }
                else
                {
                    throw NoParse($"expected {n} hex-digits in {inWhat}");
                }
            }
        }

        private void MatchEscape(string inWhat)
        {
            Eat(1);

            switch (At())
            {
                case '\"':
                case '\'':
                case '\\':
                case 'a':
                case 'b':
                case 'e':
                case 'f':
                case 'n':
                case 'r':
                case 't':
                case 'v':
                    Eat(1);
                    break;
                case 'x':
                    Eat(1);
                    MatchHexN(2, inWhat);
                    break;
                case 'u':
                    Eat(1);
                    MatchHexN(4, inWhat);
                    break;
                case 'U':
                    Eat(1);
                    MatchHexN(6, inWhat);
                    break;
                default:
                    throw NoParse($"illegal escape sequence in {inWhat}");
            }
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
            var start = GetStart();

            Match('\'');
            if (Check('\\'))
            {
                MatchEscape("character literal");
                
            }
            else
            {
                Eat(1);
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

            var start = GetStart();

            Match("\"");
            while (More())
            {
                if (At() == '\"')
                {
                    break;
                }
                if (At() == '\\')
                {
                    MatchEscape("string literal");
                }
                else
                {
                    Eat(1);
                }
            }
            if (!More())
            {
                throw NoParse("unterminated string literal");
            }
            Match("\"");

            return new Ast.String(Span(start));
        }

        private Ast.DocString DocString()
        {
            var start = GetStart();

            Match("\"\"\"");
            var done = false;
            while (!done && More())
            {
                if (scanner.Check("\"\"\""))
                {
                    Eat(3);
                    while (More() && At() == '\"')
                    {
                        Eat(1);
                    }
                    done = true;
                    break;
                }
                Eat(1);
            }
            if (!done)
            {
                throw NoParse("unterminated doc-string literal");
            }

            return new Ast.DocString(Span(start));
        }

        private Ast.Identifier Identifier() => TryIdentifier() ?? throw NoParse("identifier");

        private Ast.Identifier? TryIdentifier()
        {
            Skip();
            if (scanner.IsLetter_())
            {
                var start = GetStart();
                do
                {
                    Eat(1);
                }
                while (scanner.IsLetterOrDigit_());

                while (Check('\''))
                {
                    Eat(1);
                }

                var span = Span(start);

                if (!Keywords.IsKeyword(span))
                {
                    return new Ast.Identifier(Span(start));
                }

                SetStart(start);
            }

            return null;
        }
    }
}
