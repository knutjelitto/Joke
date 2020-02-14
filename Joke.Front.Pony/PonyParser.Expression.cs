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
            var partial = TryPartial();
            var term = TryTerm(NL.Both) ?? throw NoParse("term");

            return new Tree.BinaryOpPart(End(), kind, partial, term);
        }

        private Tree.IsPart IsPart(bool isnt)
        {
            Debug.Assert(Iss(TK.Is, TK.Isnt));

            Begin(); Match();

            var term = TryTerm() ?? throw NoParse("term");

            return new Tree.IsPart(End(), isnt, term);
        }

        private Tree.AsPart AsPart()
        {
            throw NotYet("as");
        }

        private Tree.Expression? TryTerm(NL nl = NL.Both)
        {
            if (More())
            {
                switch (Kind)
                {
                    case TK.If:
                        return Iff();
                    case TK.Ifdef:
                        return Iffdef();
                    case TK.Iftype:
                        throw NotYet("term -- iftype");
                    case TK.Match:
                        throw NotYet("term -- match");
                    case TK.While:
                        throw NotYet("term -- while");
                    case TK.Repeat:
                        throw NotYet("term -- repeat");
                    case TK.For:
                        throw NotYet("term -- for");
                    case TK.With:
                        throw NotYet("term -- with");
                    case TK.Try:
                        throw NotYet("term -- try");
                    case TK.Recover:
                        throw NotYet("term -- recover");
                    case TK.Consume:
                        throw NotYet("term -- consume");
                    case TK.Constant:
                        throw NotYet("term -- constant");
                    default:
                        return TryPattern(nl);
                }
            }

            return null;
        }

        private Tree.Expression Iff()
        {
            Debug.Assert(Iss(TK.If));

            Begin(); Match();

            var annotations = Annotations();
            var condition = TryRawSeq() ?? throw NoParse("if -- raw-seq-condition");
            var thenPart = ThenClause();
            var elsePart = TryElseIf() ?? TryElseClause();
            Match("end", TK.End);

            return new Tree.Iff(End(), Tree.IffKind.Iff, annotations, condition, thenPart, elsePart);
        }

        private Tree.Expression? TryElseIf()
        {
            if (Iss(TK.Elseif))
            {
                Begin(); Match();

                var annotations = Annotations();
                var condition = TryRawSeq() ?? throw NoParse("elseif -- raw-seq-condition");
                var thenPart = ThenClause();
                var elsePart = TryElseIfdef() ?? TryElseClause();

                return new Tree.Iff(End(), Tree.IffKind.ElseIff, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Tree.Expression Iffdef()
        {
            Debug.Assert(Iss(TK.Ifdef));

            Begin(); Match();

            var annotations = Annotations();
            var condition = TryInfix() ?? throw NoParse("ifdef -- infix-condition");
            var thenPart = ThenClause();
            var elsePart = TryElseIfdef() ?? TryElseClause();
            Match("end", TK.End);

            return new Tree.Iff(End(), Tree.IffKind.IffDef, annotations, condition, thenPart, elsePart);
        }

        private Tree.Expression? TryElseIfdef()
        {
            if (Iss(TK.Elseif))
            {
                Begin(); Match();

                var annotations = Annotations();
                var condition = TryInfix() ?? throw NoParse("elseif - infix-condition");
                var thenPart = ThenClause();
                var elsePart = TryElseIfdef() ?? TryElseClause();

                return new Tree.Iff(End(), Tree.IffKind.ElseIffDef, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Tree.Expression ThenClause()
        {
            Begin(); Match("then", TK.Then);

            var annotations = Annotations();
            var body = TryRawSeq() ?? throw NoParse("then -- body");

            return new Tree.Then(End(), annotations, body);
        }

        private Tree.Expression? TryElseClause()
        {
            if (Iss(TK.Else))
            {
                Begin();
                Match();

                var annotations = Annotations();
                var elsePart = TryRawSeq() ?? throw NoParse("else -- raw-seq");

                return new Tree.Else(End(), annotations, elsePart);
            }

            return null;
        }

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

                switch(Kind)
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
                    Begin(); Match();

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
            if (More() && Iss(TK.Semi))
            {
                Begin();

                Match();
                var expression = TryRawSeq() ?? throw NoParse("semi-expr - raw-seq");

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
                    Begin(); Match();

                    var name = Identifier();
                    var type = TryColonType();

                    return new Tree.Local(End(), kind, name, type);
                }
            }

            return null;
        }

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

                var expression = TryParamPattern(nl != NL.Case ? NL.Both : nl) ?? throw NoParse("unary operand missing");

                return new Tree.UnaryOp(End(), kind, expression);
            }

            return null;
        }

        private Tree.Expression? TryPostfix(NL nl = NL.Both)
        {
            var atom = TryAtom(nl);

            if (atom != null)
            {
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
                            atom = Chain(atom);
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
            }

            return atom;
        }

        private Tree.Expression Dot(Tree.Expression atom)
        {
            Debug.Assert(Iss(TK.Dot));

            Begin(); Match();

            var member = Identifier();

            return new Tree.Dot(End(), atom, member);
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
            Debug.Assert(Iss(TK.LSquare, TK.LSquareNew));

            Begin();

            var arguments = TryTypeArguments() ?? throw NoParse("INTERNAL:Qualify");

            return new Tree.Qualify(End(), atom, arguments);
        }

        private Tree.Expression Call(Tree.Expression atom)
        {
            Begin();

            Match("'('", TK.LParen);
            var arguments = Arguments();
            Match("')'", TK.RParen);
            var partial = TryPartial(); ;

            return new Tree.Call(End(), atom, arguments, partial);
        }

        private Tree.Partial? TryPartial()
        {
            if (More() && Iss(TK.Question))
            {
                Begin();
                Match();
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

            var value = TryRawSeq() ?? throw NoParse("positional -- raw-seq");

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

        private Tree.Expression? TryAtom(NL nl = NL.Both)
        {
            if (More())
            {
                switch (Kind)
                {
                    case TK.Identifier:
                        return Ref();
                    case TK.This:
                        return ThisLiteral();
                    case TK.String:
                    case TK.DocString:
                        return String();
                    case TK.Int:
                        return Int();
                    case TK.Float:
                        return Float();
                    case TK.True:
                    case TK.False:
                        return Bool();
                    case TK.LParen when nl != NL.Next:
                    case TK.LParenNew:
                        return GroupedExpression();
                    case TK.LSquare when nl != NL.Next:
                    case TK.LSquareNew:
                        return Array();
                    case TK.Object:
                        throw NotYet("atom -- object");
                    case TK.LBrace:
                        throw NotYet("atom -- lambda");
                    case TK.AtLBrace:
                        throw NotYet("atom -- barelambda");
                    case TK.At:
                        return FfiCall();
                    case TK.Location:
                        throw NotYet("atom -- __loc");
                    case TK.If when nl != NL.Case:
                        return Iff();
                    case TK.While:
                        throw NotYet("atom -- while");
                    case TK.For:
                        throw NotYet("atom -- for");
                    default:
                        break;
                }
            }

            return null;
        }

        private Tree.Expression? TryLiteral()
        {
            if (More())
            {
                switch (Kind)
                {
                    case TK.String:
                    case TK.DocString:
                        return String();
                    case TK.Int:
                        return Int();
                    case TK.Float:
                        return Float();
                    case TK.True:
                    case TK.False:
                        return Bool();
                }
            }

            return null;
        }

        private Tree.FfiCall FfiCall()
        {
            Debug.Assert(Iss(TK.At));

            Begin(); Match();

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
            Debug.Assert(Iss(TK.LParen, TK.LParenNew));

            Begin(); Match();

            var expressions = new List<Tree.Expression>();

            if (Issnt(TK.RParen))
            {
                var expression = TryRawSeq() ?? throw NoParse("tuple -- raw-seq");
                expressions.Add(expression);

                while (Iss(TK.Comma))
                {
                    Match();
                    expression = TryRawSeq() ?? throw NoParse("tuple -- raw-seq");
                    expressions.Add(expression);
                }
            }
            Match("')'", TK.RParen);

            return new Tree.GroupedExpression(End(), expressions);
        }

        private Tree.Array Array()
        {
            Debug.Assert(Iss(TK.LSquare, TK.LSquareNew));

            Begin(); Match();
            var type = TryArrayType();
            var elements = TryRawSeq();
            Match("']'", TK.RSquare);

            return new Tree.Array(End(), type, elements);
        }

        private Tree.Type? TryArrayType()
        {
            if (Iss(TK.As))
            {
                Begin(); Match();
                var type = Type();
                Match("':'", TK.Colon);

                return new Tree.ArrayType(End(), type);
            }

            return null;
        }

        private Tree.Identifier Identifier() => TryIdentifier() ?? throw NoParse("identifier");

        public Tree.Identifier? TryIdentifier()
        {
            if (Iss(TK.Identifier))
            {
                Begin(); Match();

                return new Tree.Identifier(End());
            }

            return null;
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

        private Tree.Int Int()
        {
            Begin();

            Match("integer", TK.Int);

            return new Tree.Int(End());
        }

        private Tree.Float Float()
        {
            Begin();

            Match("float", TK.Float);

            return new Tree.Float(End());
        }

        private Tree.Bool Bool()
        {
            Begin();

            Match("boolean", TK.True, TK.False);

            return new Tree.Bool(End());
        }
    }
}
