using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
                switch (Kind)
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
            Begin();

            Match();
            var partial = MayMatch(TK.Question);
            var term = TryTerm(NL.Both) ?? throw NoParse("term");

            return new Tree.BinaryOpPart(End(), kind, partial, term);
        }

        private Tree.IsPart IsPart(bool isnt)
        {
            throw NotYet("is/isnt");
        }

        private Tree.AsPart AsPart()
        {
            throw NotYet("as");
        }

        private Tree.Expression? TryTerm(NL nl)
        {
            switch (Kind)
            {
                case TK.If:
                    throw NotYet("term");
                case TK.Ifdef:
                    return Ifdef();
                case TK.Iftype:
                    throw NotYet("term");
                case TK.Match:
                    throw NotYet("term");
                case TK.While:
                    throw NotYet("term");
                case TK.Repeat:
                    throw NotYet("term");
                case TK.For:
                    throw NotYet("term");
                case TK.With:
                    throw NotYet("term");
                case TK.Try:
                    throw NotYet("term");
                case TK.Recover:
                    throw NotYet("term");
                case TK.Consume:
                    throw NotYet("term");
                case TK.Identifier:
                case TK.String:
                case TK.Int:
                case TK.Float:
                case TK.True:
                case TK.False:
                    return Pattern(nl);
                case TK.Constant:
                    throw NotYet("term");
            }

            return null;
        }

        private Tree.Expression Ifdef()
        {
            Debug.Assert(Iss(TK.Ifdef));

            Begin();

            Match();

            var annotations = Annotations();
            var condition = TryInfix() ?? throw NoParse("ifdef - condition");
            Match("then", TK.Then);
            var thenPart = RawSeq();
            var elsePart = TryElseIfdef() ?? TryElseClause();
            Match("end", TK.End);

            throw NotYet("ifdef");
        }

        private Tree.Expression? TryElseIfdef()
        {
            if (Iss(TK.Elseif))
            {
                Begin();

                Match();

                var annotations = Annotations();
                var condition = TryInfix() ?? throw NoParse("elseif - condition");
                Match("then", TK.Then);
                var thenPart = RawSeq();
                var elsePart = TryElseIfdef() ?? TryElseClause();

                throw NotYet("ifdef");
            }

            return null;
        }

        private Tree.Expression? TryElseClause()
        {
            throw NotYet("else");
        }

        private Tree.String String()
        {
            Begin();

            Match("string", TK.String, TK.DocString);

            return new Tree.String(End());
        }

        private Tree.String? TryString()
        {
            if (Iss(TK.String, TK.DocString))
            {
                Begin();

                Match();
                return new Tree.String(End());
            }

            return null;
        }

        private Tree.Expression RawSeq(NL nl = NL.Both)
        {   
            var expression = TryExprSeq(nl);
            if (expression == null)
            {
                expression = Jump();
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

        private Tree.Expression Jump()
        {
            throw NotYet("jump");
        }

        private Tree.Expression? TryNoSemi()
        {
            return RawSeq(NL.Next);
        }

        private Tree.Expression? TrySemiExpr()
        {
            if (More() && Iss(TK.Semi))
            {
                Begin();
                Match();
                var expression = RawSeq();

                return new Tree.SemiExpression(End(), expression);
            }

            return null;
        }

        private Tree.Expression? TryAssignment(NL nl = NL.Both)
        {
            var infix = TryInfix(nl);

            if (infix != null && Iss(TK.Assign))
            {
                Begin();

                Match();
                var right = TryAssignment(NL.Both) ?? throw NoParse("assignment - assignment");

                return new Tree.Assignment(End(), infix, right);
            }

            return infix;
        }

        private Tree.Expression Pattern(NL nl = NL.Both)
        {
            var expression = TryLocal();

            if (expression == null)
            {
                expression = ParamPattern(nl);
            }

            return expression;
        }

        private Tree.Expression? TryLocal()
        {
            Ensure();

            var kind = Tree.LocalKind.Missing;

            switch (Kind)
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
                Begin();

                var name = Identifier();
                var type = TryColonType();

                return new Tree.Local(End(), kind, name, type);
            }

            return null;
        }

        private Tree.Expression ParamPattern(NL nl = NL.Both)
        {
            var expression = TryPrefix(nl);

            if (expression == null)
            {
                expression = Postfix(nl);
            }

            return expression;
        }

        private Tree.Expression? TryPrefix(NL nl = NL.Both)
        {
            if (next == limit)
            {
                return null;
            }

            var kind = Tree.UnaryOpKind.Missing;

            switch (Kind)
            {
                case TK.Addressof:
                    kind = Tree.UnaryOpKind.Addressof;
                    break;
                case TK.DigestOf:
                    kind = Tree.UnaryOpKind.Digestof;
                    break;
                case TK.Not:
                    kind = Tree.UnaryOpKind.Not;
                    break;
                case TK.Minus when nl != NL.Next:
                case TK.MinusNew:
                    kind = Tree.UnaryOpKind.Minus;
                    break;
                case TK.MinusTilde when nl != NL.Next:
                case TK.MinusTildeNew:
                    kind = Tree.UnaryOpKind.MinusUnsafe;
                    break;
            }

            if (kind != Tree.UnaryOpKind.Missing)
            {
                Begin();

                Match();

                var expression = ParamPattern(nl != NL.Case ? NL.Both : nl);

                return new Tree.UnaryOp(End(), kind, expression);
            }

            return null;
        }

        private Tree.Expression Postfix(NL nl = NL.Both)
        {
            var atom = Atom(nl);
            
            var done = false;
            while (!done && next < limit)
            {
                switch (Kind)
                {
                    case TK.Dot:
                        atom = Dot(atom);
                        break;
                    case TK.Tilde:
                        atom = Tilde(atom);
                        break;
                    case TK.Chain:
                        atom = Tilde(atom);
                        break;
                    case TK.LSquare:
                        atom = Qualify(atom);
                        break;
                    case TK.LParen:
                        atom = Call(atom);
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return atom;
        }

        private Tree.Expression Dot(Tree.Expression atom)
        {
            throw NotYet("dot");
        }

        private Tree.Expression Tilde(Tree.Expression atom)
        {
            throw NotYet("tilde");
        }

        private Tree.Expression Chain(Tree.Expression atom)
        {
            throw NotYet("chain");
        }

        private Tree.Expression Qualify(Tree.Expression atom)
        {
            throw NotYet("qualify");
        }

        private Tree.Expression Call(Tree.Expression atom)
        {
            Begin();

            Match("'('", TK.LParen);
            var arguments = Arguments();
            Match("')'", TK.RParen);

            return new Tree.Call(End(), arguments);
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
                    Match();
                    continue;
                }
                break;
            }

            if (Iss(TK.Where))
            {
                do
                {
                    Match();
                    arguments.Add(Named());
                }
                while (Iss(TK.Comma));
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
            throw NotYet("named-argument");
        }

        private Tree.Ref Ref()
        {
            Begin();

            var name = Identifier();

            return new Tree.Ref(End(), name);
        }

        private Tree.ThisLiteral ThisLiteral()
        {
            Debug.Assert(Iss(TK.This));

            Begin();

            Match();

            return new Tree.ThisLiteral(End());
        }

        private Tree.Expression Atom(NL nl = NL.Both)
        {
            Ensure();

            switch (Kind)
            {
                case TK.Identifier:
                    return Ref();
                case TK.This:
                    return ThisLiteral();
                case TK.String:
                    return String();
                default:
                    break;
            }

            throw NotYet("atom");
        }
    }
}
