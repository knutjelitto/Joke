using Joke.Front.Pony.Lex;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Front.Pony
{
    partial class PonyParser
    {
        private enum NL
        {
            Both,
            Next,
            Case
        }

        private Tree.Expression Infix(NL nl = NL.Both) => TryInfix(nl) ?? throw NoParse("infix");

        private Tree.Expression? TryInfix(NL nl = NL.Both)
        {
            Begin();

            var term = TryTerm(nl);

            if (term == null)
            {
                Discard();
                return null;
            }

            var parts = new List<Tree.InfixPart>();

            var done = false;
            while (!done && next < limit)
            {
                switch (TokenKind)
                {
                    case TK.And:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.And));
                        break;
                    case TK.Or:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Or));
                        break;
                    case TK.Xor:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Xor));
                        break;

                    case TK.Plus:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Plus));
                        break;
                    case TK.Minus:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Minus));
                        break;
                    case TK.Multiply:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Multiply));
                        break;
                    case TK.Divide:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Divide));
                        break;
                    case TK.Rem:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Rem));
                        break;
                    case TK.Mod:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Mod));
                        break;

                    case TK.PlusTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.PlusUnsafe));
                        break;
                    case TK.MinusTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.MinusUnsafe));
                        break;
                    case TK.MultiplyTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.MultiplyUnsafe));
                        break;
                    case TK.DivideTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.DivideUnsafe));
                        break;
                    case TK.RemTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.RemUnsafe));
                        break;
                    case TK.ModTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.ModUnsafe));
                        break;

                    case TK.LShift:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.LShift));
                        break;
                    case TK.RShift:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.RShift));
                        break;

                    case TK.LShiftTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.LShiftUnsafe));
                        break;
                    case TK.RShiftTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.RShiftUnsafe));
                        break;

                    case TK.Eq:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Eq));
                        break;
                    case TK.Ne:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Ne));
                        break;
                    case TK.Lt:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Lt));
                        break;
                    case TK.Le:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Le));
                        break;
                    case TK.Gt:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Gt));
                        break;
                    case TK.Ge:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.Ge));
                        break;

                    case TK.EqTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.EqUnsafe));
                        break;
                    case TK.NeTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.NeUnsafe));
                        break;
                    case TK.LtTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.LtUnsafe));
                        break;
                    case TK.LeTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.LeUnsafe));
                        break;
                    case TK.GtTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.GtUnsafe));
                        break;
                    case TK.GeTilde:
                        parts.Add(BinaryPart(Tree.BinaryOpKind.GeUnsafe));
                        break;

                    case TK.Is:
                        parts.Add(IsPart(false));
                        break;
                    case TK.Isnt:
                        parts.Add(IsPart(true));
                        break;
                    case TK.As:
                        parts.Add(AsPart());
                        break;

                    default:
                        done = true;
                        break;
                }
            }

            if (parts.Count > 0)
            {
                return new Tree.Infix(End(), term, parts);
            }

            Discard();
            return term;
        }

        private Tree.BinaryOpPart BinaryPart(Tree.BinaryOpKind kind)
        {
            Begin(TokenKind);
            var partial = TryPartial();
            var term = Term(NL.Both);

            return new Tree.BinaryOpPart(End(), kind, partial, term);
        }

        private Tree.IsPart IsPart(bool isnt)
        {
            Begin(TK.Is, TK.Isnt);
            var term = Term();
            return new Tree.IsPart(End(), isnt, term);
        }

        private Tree.AsPart AsPart()
        {
            Begin(TK.As);
            var type = Type();
            return new Tree.AsPart(End(), type);
        }

        private Tree.Expression Term(NL nl = NL.Both) => TryTerm(nl) ?? throw NoParse("term");
        private Tree.Expression? TryTerm(NL nl = NL.Both)
        {
            if (More())
            {
                switch (TokenKind)
                {
                    case TK.If:
                        return Iff();
                    case TK.Ifdef:
                        return Iffdef();
                    case TK.Iftype:
                        return Ifftype();
                    case TK.Match:
                        return DoMatch();
                    case TK.While:
                        return While();
                    case TK.Repeat:
                        return Repeat();
                    case TK.For:
                        return For();
                    case TK.With:
                        return With();
                    case TK.Try:
                        return Try();
                    case TK.Recover:
                        return Recover();
                    case TK.Consume:
                        return Consume();
                    case TK.Constant:
                        throw NotYet("term -- constant");
                    default:
                        return TryPattern(nl);
                }
            }

            return null;
        }

        private Tree.With With()
        {
            Debug.Assert(Iss(TK.With));

            Begin(TK.With);
            var annotations = TryAnnotations();
            var elements = WithElements();
            Match(TK.Do);
            var body = RawSeq();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Tree.With(End(), annotations, elements, body, elsePart);
        }

        private Tree.WithElements WithElements()
        {
            Begin();
            var elements = new List<Tree.WithElement>();
            do
            {
                elements.Add(WithElement());
            }
            while (MayMatch(TK.Comma));

            return new Tree.WithElements(End(), elements);
        }

        private Tree.WithElement WithElement()
        {
            Begin();

            var names = Ids();
            Match(TK.Assign);
            var initializer = RawSeq();

            return new Tree.WithElement(End(), names, initializer);
        }

        private Tree.For For()
        {
            Begin(TK.For);
            var annotations = TryAnnotations();
            var ids = Ids();
            Match(TK.In);
            var iterator = RawSeq();
            Match(TK.Do);
            var body = RawSeq();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Tree.For(End(), annotations, ids, iterator, body, elsePart);
        }

        private Tree.Ids Ids()
        {
            if (Iss(TK.Identifier))
            {
                Begin();
                var name = Identifier();
                return new Tree.IdsSingle(End(), name);
            }
            else if (Iss(TK.LParen, TK.LParenNew))
            {
                return IdsMulti();
            }

            throw NoParse("ids");
        }

        private Tree.IdsMulti IdsMulti()
        {
            Begin(TK.LParen, TK.LParenNew);

            var idss = new List<Tree.Ids>();

            do
            {
                var ids = Ids();
                idss.Add(ids);
            }
            while (MayMatch(TK.Comma));
            Match(TK.RParen);

            return new Tree.IdsMulti(End(), idss);
        }

        private Tree.Repeat Repeat()
        {
            Begin(TK.Repeat);
            var annotations = TryAnnotations();
            var body = RawSeq();
            Match(TK.Until);
            var condition = RawSeq();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Tree.Repeat(End(), annotations, body, condition, elsePart);
        }

        private Tree.While While()
        {
            Begin(TK.While);
            var annotations = TryAnnotations();
            var condition = RawSeq();
            Match(TK.Do);
            var body = RawSeq();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Tree.While(End(), annotations, condition, body, elsePart);
        }

        private Tree.Match DoMatch()
        {
            Begin(TK.Match);
            var annotations = TryAnnotations();
            var expr = RawSeq();
            var cases = Cases();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Tree.Match(End(), annotations, expr, cases, elsePart);
        }

        private Tree.Cases Cases()
        {
            Begin();

            var cases = new List<Tree.Case>();
            Tree.Case? @case = null;
            while ((@case = TryCase()) != null)
            {
                cases.Add(@case);
            }

            return new Tree.Cases(End(), cases);
        }

        private Tree.Case? TryCase()
        {
            if (MayBegin(TK.Pipe))
            {
                var annotations = TryAnnotations();
                var pattern = TryPattern(NL.Case);
                Tree.Expression? guard = null;
                if (Iss(TK.If))
                {
                    Match(TK.If);
                    guard = RawSeq();
                }
                var body = TryBody();

                return new Tree.Case(End(), annotations, pattern, guard, body);
            }

            return null;
        }

        private Tree.Consume Consume()
        {
            Begin(TK.Consume);
            var cap = TryCap(false);
            var term = Term();
            return new Tree.Consume(End(), cap, term);
        }

        private Tree.Recover Recover()
        {
            Begin(TK.Recover);
            var annotations = TryAnnotations();
            var cap = TryCap(false);
            var body = RawSeq();
            Match(TK.End);

            return new Tree.Recover(End(), annotations, cap, body);
        }

        public Tree.Expression Try()
        {
            Begin(TK.Try);
            var annotations = TryAnnotations();
            var body = RawSeq();
            var elsePart = TryElseClause();
            var thenPart = TryThenClause();
            Match(TK.End);

            return new Tree.Try(End(), annotations, body, elsePart, thenPart);
        }

        private Tree.Expression Iff()
        {
            Begin(TK.If);
            var annotations = TryAnnotations();
            var condition = RawSeq();
            var thenPart = ThenClause();
            var elsePart = TryElseIf() ?? TryElseClause();
            Match(TK.End);

            return new Tree.Iff(End(), Tree.IffKind.Iff, annotations, condition, thenPart, elsePart);
        }

        private Tree.Expression? TryElseIf()
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = RawSeq();
                var thenPart = ThenClause();
                var elsePart = TryElseIfdef() ?? TryElseClause();

                return new Tree.Iff(End(), Tree.IffKind.ElseIff, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Tree.Expression Iffdef()
        {
            Begin(TK.Ifdef);
            var annotations = TryAnnotations();
            var condition = Infix();
            var thenPart = ThenClause();
            var elsePart = TryElseIfdef() ?? TryElseClause();
            Match(TK.End);

            return new Tree.Iff(End(), Tree.IffKind.IffDef, annotations, condition, thenPart, elsePart);
        }

        private Tree.Expression? TryElseIfdef()
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = Infix();
                var thenPart = ThenClause();
                var elsePart = TryElseIfdef() ?? TryElseClause();

                return new Tree.Iff(End(), Tree.IffKind.ElseIffDef, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Tree.Expression Ifftype()
        {
            Begin(TK.Iftype);
            var annotations = TryAnnotations();
            var condition = SubType();
            var thenPart = ThenClause();
            var elsePart = TryElseIftype() ?? TryElseClause();
            Match(TK.End);

            return new Tree.Iff(End(), Tree.IffKind.IffType, annotations, condition, thenPart, elsePart);
        }

        private Tree.Expression? TryElseIftype()
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = SubType();
                var thenPart = ThenClause();
                var elsePart = TryElseIftype() ?? TryElseClause();

                return new Tree.Iff(End(), Tree.IffKind.ElseIffType, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Tree.Expression SubType()
        {
            Begin();
            var sub = Type();
            Match(TK.Subtype);
            var super = Type();

            return new Tree.SubType(End(), sub, super);
        }


        private Tree.Expression ThenClause() => TryThenClause() ?? throw NoParse("then");
        private Tree.Expression? TryThenClause()
        {
            if (MayBegin(TK.Then))
            {
                var annotations = TryAnnotations();
                var body = RawSeq();

                return new Tree.Then(End(), annotations, body);
            }

            return null;
        }

        private Tree.Else? TryElseClause()
        {
            if (MayBegin(TK.Else))
            {
                var annotations = TryAnnotations();
                var elsePart = RawSeq();

                return new Tree.Else(End(), annotations, elsePart);
            }

            return null;
        }

        private Tree.Expression RawSeq(NL nl = NL.Both) => TryRawSeq(nl) ?? throw NoParse("raw-seq");
        private Tree.Expression? TryRawSeq(NL nl = NL.Both)
        {
            var expression = TryExprSeq(nl);
            if (expression == null)
            {
                expression = TryJump();
            }

            return expression;
        }

        private Tree.Expression? TryExprSeq(NL nl = NL.Both)
        {
            Begin();

            var assignment = TryAssignment(nl);

            if (assignment != null)
            {
                var next = TrySemiExpr() ?? TryNoSemi();

                if (next != null)
                {
                    return new Tree.Sequence(End(), assignment, next);
                }
            }

            Discard();

            return assignment;
        }

        private Tree.Expression? TryJump()
        {
            if (More())
            {
                var kind = Tree.JumpKind.Missing;

                switch (TokenKind)
                {
                    case TK.Return:
                        kind = Tree.JumpKind.Return;
                        break;
                    case TK.Break:
                        kind = Tree.JumpKind.Break;
                        break;
                    case TK.Continue:
                        kind = Tree.JumpKind.Continue;
                        break;
                    case TK.Error:
                        kind = Tree.JumpKind.Error;
                        break;
                    case TK.CompileIntrinsic:
                        kind = Tree.JumpKind.CompileIntrinsic;
                        break;
                    case TK.CompileError:
                        kind = Tree.JumpKind.CompileError;
                        break;
                }

                if (kind != Tree.JumpKind.Missing)
                {
                    Begin(TokenKind);
                    var value = TryRawSeq();

                    return new Tree.Jump(End(), kind, value);
                }
            }

            return null;
        }

        private Tree.Expression? TryNoSemi()
        {
            return TryRawSeq(NL.Next);
        }

        private Tree.Expression? TrySemiExpr()
        {
            if (MayBegin(TK.Semi))
            {
                var expression = RawSeq();
                return new Tree.SemiExpression(End(), expression);
            }

            return null;
        }

        private Tree.Expression Assignment(NL nl = NL.Both) => TryAssignment(nl) ?? throw NoParse("assignment");
        private Tree.Expression? TryAssignment(NL nl = NL.Both)
        {
            Begin();

            var infix = TryInfix(nl);

            if (infix == null)
            {
                Discard();
                return null;
            }

            if (MayMatch(TK.Assign))
            {
                var right = Assignment(NL.Both);
                return new Tree.Assignment(End(), infix, right);
            }

            Discard();
            return infix;
        }

        private Tree.Expression? TryPattern(NL nl = NL.Both)
        {
            var expression = TryLocal();

            if (expression == null)
            {
                expression = TryParamPattern(nl);
            }

            return expression;
        }

        private Tree.Expression? TryLocal()
        {
            if (More())
            {
                var kind = Tree.LocalKind.Missing;

                switch (TokenKind)
                {
                    case TK.Var:
                        kind = Tree.LocalKind.Var;
                        break;
                    case TK.Let:
                        kind = Tree.LocalKind.Let;
                        break;
                    case TK.Embed:
                        kind = Tree.LocalKind.Embed;
                        break;
                }

                if (kind != Tree.LocalKind.Missing)
                {
                    Begin(TokenKind);
                    var name = Identifier();
                    var type = TryColonType();

                    return new Tree.Local(End(), kind, name, type);
                }
            }

            return null;
        }

        private Tree.Expression ParamPattern(NL nl = NL.Both) => TryParamPattern(nl) ?? throw NoParse("param-pattern");
        private Tree.Expression? TryParamPattern(NL nl = NL.Both)
        {
            var expression = TryPrefix(nl);

            if (expression == null)
            {
                expression = TryPostfix(nl);
            }

            return expression;
        }

        private Tree.Expression? TryPrefix(NL nl = NL.Both)
        {
            if (More())
            {
                switch (TokenKind)
                {
                    case TK.Addressof:
                        return Unary(Tree.UnaryOpKind.Addressof, nl);
                    case TK.DigestOf:
                        return Unary(Tree.UnaryOpKind.Digestof, nl);
                    case TK.Not:
                        return Unary(Tree.UnaryOpKind.Not, nl);
                    case TK.Minus when nl != NL.Next:
                    case TK.MinusNew:
                        return Unary(Tree.UnaryOpKind.Minus, nl);
                    case TK.MinusTilde when nl != NL.Next:
                    case TK.MinusTildeNew:
                        return Unary(Tree.UnaryOpKind.MinusUnsafe, nl);
                }
            }

            return null;
        }

        private Tree.Expression Unary(Tree.UnaryOpKind kind, NL nl)
        {
            Begin(TK.Addressof, TK.DigestOf, TK.Not, TK.Minus, TK.MinusNew, TK.MinusTilde, TK.MinusTildeNew);
            var expression = ParamPattern(nl != NL.Case ? NL.Both : nl);
            return new Tree.UnaryOp(End(), kind, expression);
        }

        private Tree.Expression? TryPostfix(NL nl = NL.Both)
        {
            Begin();

            var atom = TryAtom(nl);

            if (atom == null)
            {
                Discard();
                return null;
            }

            var parts = new List<Tree.PostfixPart>();

            var done = false;
            while (!done && More())
            {
                switch (TokenKind)
                {
                    case TK.Dot:
                        parts.Add(Dot());
                        break;
                    case TK.Tilde:
                        parts.Add(Tilde());
                        break;
                    case TK.Chain:
                        parts.Add(Chain());
                        break;
                    case TK.LSquare:
                        parts.Add(Qualify());
                        break;
                    case TK.LParen:
                        parts.Add(Call());
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            if (parts.Count > 0)
            {
                return new Tree.Postfix(End(), atom, parts);
            }

            Discard();
            return atom;
        }

        private Tree.Dot Dot()
        {
            Begin(TK.Dot);
            var member = Identifier();
            return new Tree.Dot(End(), member);
        }

        private Tree.Tilde Tilde()
        {
            Begin(TK.Tilde);
            var method = Identifier();
            return new Tree.Tilde(End(), method);
        }

        private Tree.Chain Chain()
        {
            Begin(TK.Chain);
            var method = Identifier();
            return new Tree.Chain(End(), method);
        }

        private Tree.Qualify Qualify()
        {
            Begin();
            var arguments = TypeArguments();
            return new Tree.Qualify(End(), arguments);
        }

        private Tree.Call Call()
        {
            Begin(TK.LParen);
            var arguments = Arguments();
            Match(TK.RParen);
            var partial = TryPartial(); ;

            return new Tree.Call(End(), arguments, partial);
        }

        private Tree.Partial? TryPartial()
        {
            if (MayBegin(TK.Question))
            {
                return new Tree.Partial(End());
            }

            return null;
        }

        private Tree.Arguments Arguments()
        {
            Begin();

            var arguments = new List<Tree.Argument>();

            while (Issnt(TK.Where, TK.RParen))
            {
                arguments.Add(Positional());
                if (Iss(TK.Comma))
                {
                    Match(TK.Comma);
                    continue;
                }
                break;
            }

            if (Iss(TK.Where))
            {
                Match(TK.Where);
                do
                {
                    arguments.Add(Named());
                }
                while (MayMatch(TK.Comma));
            }

            return new Tree.Arguments(End(), arguments);
        }

        private Tree.PositionalArgument Positional()
        {
            Begin();

            var value = RawSeq();
            return new Tree.PositionalArgument(End(), value);
        }

        private Tree.NamedArgument Named()
        {
            Begin();

            var name = Identifier();
            Match(TK.Assign);
            var value = RawSeq();
            return new Tree.NamedArgument(End(), name, value);
        }

        private Tree.Ref Ref()
        {
            Begin();

            var name = Identifier();
            return new Tree.Ref(End(), name);
        }

        private Tree.ThisLiteral ThisLiteral()
        {
            Begin(TK.This);
            return new Tree.ThisLiteral(End());
        }

        private Tree.Expression? TryAtom(NL nl = NL.Both)
        {
            if (More())
            {
                switch (TokenKind)
                {
                    case TK.Identifier:
                        return Ref();
                    case TK.This:
                        return ThisLiteral();
                    case TK.String:
                    case TK.DocString:
                        return String();
                    case TK.Char:
                        return Char();
                    case TK.Int:
                        return Int();
                    case TK.Float:
                        return Float();
                    case TK.True:
                        return Bool(true);
                    case TK.False:
                        return Bool(false);
                    case TK.LParen when nl != NL.Next:
                    case TK.LParenNew:
                        return GroupedExpression();
                    case TK.LSquare when nl != NL.Next:
                    case TK.LSquareNew:
                        return Array();
                    case TK.Object:
                        return Object();
                    case TK.LBrace:
                        return Lambda(false);
                    case TK.AtLBrace:
                        return Lambda(true);
                    case TK.At:
                        return FfiCall();
                    case TK.Location:
                        return Location();
                    case TK.If when nl != NL.Case:
                        return Iff();
                    case TK.While:
                        return While();
                    case TK.For:
                        return For();
                    default:
                        break;
                }
            }

            return null;
        }

        private Tree.Location Location()
        {
            Begin(TK.Location);
            return new Tree.Location(End());
        }

        private Tree.Expression? TryLiteral()
        {
            if (More())
            {
                switch (TokenKind)
                {
                    case TK.String:
                    case TK.DocString:
                        return String();
                    case TK.Char:
                        return Char();
                    case TK.Int:
                        return Int();
                    case TK.Float:
                        return Float();
                    case TK.True:
                        return Bool(true);
                    case TK.False:
                        return Bool(false);
                }
            }

            return null;
        }

        private Tree.Object Object()
        {
            Begin();
            Match(TK.Object);

            var annotations = TryAnnotations();
            var cap = TryCap(false);
            var provides = TryProvides();
            var members = Members();
            Match(TK.End);
            return new Tree.Object(End(), annotations, cap, provides, members);
        }

        private Tree.Lambda Lambda(bool bare)
        {
            Begin(TK.LBrace, TK.AtLBrace);
            var annotations = TryAnnotations();
            var recCap = TryCap(false);
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaParameters();
            var captures = TryLambdaCaptures();
            var returnType = TryColonType();
            var partial = TryPartial();
            var body = TryBody();
            Match(TK.RBrace);
            var refCap = TryCap(false);

            return new Tree.Lambda(End(), bare, annotations, recCap, name, typeParameters, parameters, captures, returnType, partial, body, refCap);
        }

        private Tree.LambdaParameters LambdaParameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = new List<Tree.LambdaParameter>();
            if (Issnt(TK.RParen))
            {
                do
                {
                    parameters.Add(LambdaParameter());
                }
                while (MayMatch(TK.Comma));
            }
            Match(TK.RParen);

            return new Tree.LambdaParameters(End(), parameters);
        }

        private Tree.LambdaParameter LambdaParameter()
        {
            Begin();

            var name = Identifier();
            var type = TryColonType();
            var value = TryDefaultInfixArg();
            return new Tree.LambdaParameter(End(), name, type, value);
        }

        private Tree.LambdaCaptures? TryLambdaCaptures()
        {
            if (MayBegin(TK.LParen, TK.LParenNew))
            {
                var captures = new List<Tree.LambdaCapture>();

                captures.Add(LambdaCapture());
                while (MayMatch(TK.Comma))
                {
                    captures.Add(LambdaCapture());
                }
                Match(TK.RParen);
                return new Tree.LambdaCaptures(End(), captures);
            }

            return null;
        }

        private Tree.LambdaCapture LambdaCapture()
        {
            Ensure();

            switch (TokenKind)
            {
                case TK.Identifier:
                    Begin();
                    var name = Identifier();
                    var type = TryColonType();
                    var value = TryDefaultInfixArg();
                    return new Tree.LambdaCaptureName(End(), name, type, value);
                case TK.This:
                    Begin();
                    var thisLiteral = ThisLiteral();
                    return new Tree.LambdaCaptureThis(End(), thisLiteral);
            }

            throw NoParse("lambda-capture");
        }


        private Tree.FfiCall FfiCall()
        {
            Begin(TK.At);
            var name = FfiName();
            var returnType = TryTypeArguments();
            var arguments = Arguments();
            var partial = TryPartial();

            return new Tree.FfiCall(End(), name, returnType, arguments, partial);
        }

        private Tree.FfiName FfiName()
        {
            Begin();

            var name = (Tree.Expression?)TryString() ?? Identifier();

            return new Tree.FfiName(End(), name);
        }

        private Tree.GroupedExpression GroupedExpression()
        {
            Begin(TK.LParen, TK.LParenNew);
            var expressions = new List<Tree.Expression>();
            if (Issnt(TK.RParen))
            {
                expressions.Add(RawSeq());

                while (MayMatch(TK.Comma))
                {
                    expressions.Add(RawSeq());
                }
            }
            Match(TK.RParen);

            return new Tree.GroupedExpression(End(), expressions);
        }

        private Tree.Array Array()
        {
            Debug.Assert(Iss(TK.LSquare, TK.LSquareNew));

            Begin(TK.LSquare, TK.LSquareNew);
            var type = TryArrayType();
            var elements = TryRawSeq();
            Match(TK.RSquare);

            return new Tree.Array(End(), type, elements);
        }

        private Tree.Type? TryArrayType()
        {
            if (MayBegin(TK.As))
            {
                var type = Type();
                Match(TK.Colon);

                return new Tree.ArrayType(End(), type);
            }

            return null;
        }

        public Tree.Identifier Identifier()
        {
            Begin(TK.Identifier);
            return new Tree.Identifier(End());
        }

        public Tree.Identifier? TryIdentifier()
        {
            if (Iss(TK.Identifier))
            {
                return Identifier();
            }

            return null;
        }

        private Tree.Char Char()
        {
            Begin(TK.Char);
            return new Tree.Char(End());
        }

        private Tree.String String()
        {
            Begin(TK.String, TK.DocString);
            return new Tree.String(End());
        }

        private Tree.String? TryString()
        {
            if (Iss(TK.String, TK.DocString))
            {
                return String();
            }

            return null;
        }

        private Tree.Int Int()
        {
            Begin(TK.Int);
            return new Tree.Int(End());
        }

        private Tree.Float Float()
        {
            Begin(TK.Float);
            return new Tree.Float(End());
        }

        private Tree.Bool Bool(bool value)
        {
            Begin(TK.False, TK.True);
            return new Tree.Bool(End(), value);
        }
    }
}
