using System;
using System.Collections.Generic;
using System.Linq;

using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.ParseTree;

namespace Joke.Front.Pony.Parsing
{
    partial class PonyParser
    {
        private PtExpression Infix(NL nl = NL.Both) => TryInfix(nl) ?? throw NoParse("infix");
        private PtExpression? TryInfix(NL nl = NL.Both)
        {
            var term = TryTerm(nl);

            if (term == null)
            {
                return null;
            }

            var parts = Collect(TryInfixPart);

            if (parts.Count >= 1)
            {
                var operators = parts.Select(p => p.Kind).Distinct().ToList();
                if (operators.Count >= 2)
                {
                    var part = parts.Where(p => p.Kind != operators[0]).First();
                    var token = Tokens[part.Span.Start];
                    AddError(token, "binary operators have no precedence, use ( ) to group binary expressions");
                }
                var @operator = operators[0];
                switch (@operator)
                {
                    case PtBinaryKind.As:
                        if (parts.Count > 1)
                        {
                            throw NoParse("binary operator <as> doesn't associate");
                        }
                        return new PtAs(Mark(term), term, ((Parts.AsPart)parts[0]).Type);
                    case PtBinaryKind.Is:
                    case PtBinaryKind.Isnt:
                        if (parts.Count > 1)
                        {
                            throw NoParse("binary operator <is>/<isnt> doesn't associate");
                        }
                        return new PtBinary(Mark(term), @operator, term, ((Parts.IsPart)parts[0]).Expression);
                    default:
                        var operands = new List<PtExpression> { term };
                        operands.AddRange(parts.Cast<Parts.BinaryPart>().Select(b => b.Right));
                        return new PtBinary(Mark(term), @operator, operands);
                }
            }

            return term;
        }

        private Parts.InfixPart? TryInfixPart()
        {
            return TokenKind switch
            {
                TK.And => BinaryPart(PtBinaryKind.And),
                TK.Or => BinaryPart(PtBinaryKind.Or),
                TK.Xor => BinaryPart(PtBinaryKind.Xor),
                TK.Plus => BinaryPart(PtBinaryKind.Plus),
                TK.PlusTilde => BinaryPart(PtBinaryKind.PlusUnsafe),
                TK.Minus => BinaryPart(PtBinaryKind.Minus),
                TK.MinusTilde => BinaryPart(PtBinaryKind.MinusUnsafe),
                TK.Multiply => BinaryPart(PtBinaryKind.Multiply),
                TK.MultiplyTilde => BinaryPart(PtBinaryKind.MultiplyUnsafe),
                TK.Divide => BinaryPart(PtBinaryKind.Divide),
                TK.DivideTilde => BinaryPart(PtBinaryKind.DivideUnsafe),
                TK.Rem => BinaryPart(PtBinaryKind.Rem),
                TK.RemTilde => BinaryPart(PtBinaryKind.RemUnsafe),
                TK.Mod => BinaryPart(PtBinaryKind.Mod),
                TK.ModTilde => BinaryPart(PtBinaryKind.ModUnsafe),
                TK.LShift => BinaryPart(PtBinaryKind.LShift),
                TK.LShiftTilde => BinaryPart(PtBinaryKind.LShiftUnsafe),
                TK.RShift => BinaryPart(PtBinaryKind.RShift),
                TK.RShiftTilde => BinaryPart(PtBinaryKind.RShiftUnsafe),
                TK.Eq => BinaryPart(PtBinaryKind.Eq),
                TK.EqTilde => BinaryPart(PtBinaryKind.EqUnsafe),
                TK.Ne => BinaryPart(PtBinaryKind.Ne),
                TK.NeTilde => BinaryPart(PtBinaryKind.NeUnsafe),
                TK.Lt => BinaryPart(PtBinaryKind.Lt),
                TK.LtTilde => BinaryPart(PtBinaryKind.LtUnsafe),
                TK.Le => BinaryPart(PtBinaryKind.Le),
                TK.LeTilde => BinaryPart(PtBinaryKind.LeUnsafe),
                TK.Gt => BinaryPart(PtBinaryKind.Gt),
                TK.GtTilde => BinaryPart(PtBinaryKind.GtUnsafe),
                TK.Ge => BinaryPart(PtBinaryKind.Ge),
                TK.GeTilde => BinaryPart(PtBinaryKind.GeUnsafe),
                TK.Is => IsPart(PtBinaryKind.Is),
                TK.Isnt => IsPart(PtBinaryKind.Isnt),
                TK.As => AsPart(),
                _ => null,
            };
        }

        private Parts.BinaryPart BinaryPart(PtBinaryKind kind)
        {
            var token = Token;

            Begin(TokenKind);
            var partial = MayPartial();
            var term = Term(NL.Both);

            if (partial)
            {
                switch (kind)
                {
                    case PtBinaryKind.Plus:
                        kind = PtBinaryKind.PlusPartial;
                        break;
                    case PtBinaryKind.Minus:
                        kind = PtBinaryKind.MinusPartial;
                        break;
                    case PtBinaryKind.Multiply:
                        kind = PtBinaryKind.MultiplyPartial;
                        break;
                    case PtBinaryKind.Divide:
                        kind = PtBinaryKind.DividePartial;
                        break;
                    case PtBinaryKind.Mod:
                        kind = PtBinaryKind.ModPartial;
                        break;
                    case PtBinaryKind.Rem:
                        kind = PtBinaryKind.RemPartial;
                        break;
                    default:
                        AddError(token, $"{Keywords.String(token.Kind)} can't be made partial");
                        break;
                }
            }

            return new Parts.BinaryPart(End(), kind, term);
        }

        private Parts.IsPart IsPart(PtBinaryKind kind)
        {
            Begin(TK.Is, TK.Isnt);
            var term = Term();
            return new Parts.IsPart(End(), kind, term);
        }

        private Parts.AsPart AsPart()
        {
            Begin(TK.As);
            var type = Type();
            return new Parts.AsPart(End(), type);
        }

        private PtExpression Term(NL nl = NL.Both) => TryTerm(nl) ?? throw NoParse("term");
        private PtExpression? TryTerm(NL nl = NL.Both)
        {
            return TokenKind switch
            {
                TK.If => Iff(),
                TK.Ifdef => Iffdef(),
                TK.Iftype => Ifftype(),
                TK.Match => DoMatch(),
                TK.While => While(),
                TK.Repeat => Repeat(),
                TK.For => For(),
                TK.With => With(),
                TK.Try => Try(),
                TK.Recover => Recover(),
                TK.Consume => Consume(),
                TK.Constant => throw NotYet("term -- constant"),
                _ => TryPattern(nl),
            };
        }

        private PtWith With()
        {
            Begin(TK.With);
            var annotations = TryAnnotations();
            var elements = WithElements();
            Match(TK.Do);
            var body = RawSequence();
            var elsePart = TryElse();

            return new PtWith(End(TK.End), annotations, elements, body, elsePart);
        }

        private PtWithElements WithElements()
        {
            Begin();
            var elements = List(WithElement);
            return new PtWithElements(End(), elements);
        }

        private PtWithElement WithElement()
        {
            Begin();
            var names = Ids();
            Match(TK.Assign);
            var initializer = RawSequence();
            return new PtWithElement(End(), names, initializer);
        }

        private PtFor For()
        {
            Begin(TK.For);
            var annotations = TryAnnotations();
            var ids = Ids();
            Match(TK.In);
            var iterator = RawSequence();
            Match(TK.Do);
            var body = RawSequence();
            var elsePart = TryElse();

            return new PtFor(End(TK.End), annotations, ids, iterator, body, elsePart);
        }

        private PtIds Ids()
        {
            if (Iss(TK.Identifier))
            {
                return IdsSingle();
            }
            
            return IdsMulti();
        }

        private PtIdsSingle IdsSingle()
        {
            Begin();
            var name = Identifier();
            return new PtIdsSingle(End(), name);
        }

        private PtIdsMulti IdsMulti()
        {
            Begin(TK.LParen, TK.LParenNew);
            var idss = List(Ids);

            return new PtIdsMulti(End(TK.RParen), idss);
        }

        private PtRepeat Repeat()
        {
            Begin(TK.Repeat);
            var annotations = TryAnnotations();
            var body = RawSequence();
            Match(TK.Until);
            var condition = RawSequence();
            var elsePart = TryElse();

            return new PtRepeat(End(TK.End), annotations, body, condition, elsePart);
        }

        private PtWhile While()
        {
            Begin(TK.While);
            var annotations = TryAnnotations();
            var condition = RawSequence();
            Match(TK.Do);
            var body = RawSequence();
            var elsePart = TryElse();

            return new PtWhile(End(TK.End), annotations, condition, body, elsePart);
        }

        private PtMatch DoMatch()
        {
            Begin(TK.Match);
            var annotations = TryAnnotations();
            var expr = RawSequence();
            var cases = Cases();
            var elsePart = TryElse();


            return new PtMatch(End(TK.End), annotations, expr, cases, elsePart);
        }

        private PtCases Cases()
        {
            Begin();
            var cases = Collect(TryCase);

            return new PtCases(End(), cases);
        }

        private PtCase? TryCase()
        {
            if (MayBegin(TK.Pipe))
            {
                var annotations = TryAnnotations();
                var pattern = TryPattern(NL.Case);
                var guard = TryCaseGuard();
                var body = TryBody();

                return new PtCase(End(), annotations, pattern, guard, body);
            }

            return null;
        }

        private PtGuard? TryCaseGuard()
        {
            if (MayBegin(TK.If))
            {
                var expression = RawSequence();
                return new PtGuard(End(), expression);
            }

            return null;
        }

        private PtConsume Consume()
        {
            Begin(TK.Consume);
            var cap = TryCap();
            var term = Term();
            return new PtConsume(End(), cap, term);
        }

        private PtRecover Recover()
        {
            Begin(TK.Recover);
            var annotations = TryAnnotations();
            var cap = TryCap();
            var body = RawSequence();


            return new PtRecover(End(TK.End), annotations, cap, body);
        }

        private PtExpression Try()
        {
            Begin(TK.Try);
            var annotations = TryAnnotations();
            var body = RawSequence();
            var elsePart = TryElse();
            var thenPart = TryThenClause();


            return new PtTry(End(TK.End), annotations, body, elsePart, thenPart);
        }

        private PtExpression Iff(TK iffToken, Func<PtExpression> parseCondition, PtIffKind iff, PtIffKind elseIf)
        {
            Begin(iffToken);
            var annotations = TryAnnotations();
            var condition = parseCondition();
            var thenPart = ThenClause();
            var elsePart = TryElseIf(parseCondition, elseIf) ?? TryElse();


            return new PtIff(End(TK.End), iff, annotations, condition, thenPart, elsePart);
        }

        private PtExpression? TryElseIf(Func<PtExpression> parseCondition, PtIffKind elseIf)
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = parseCondition();
                var thenPart = ThenClause();
                var elsePart = TryElseIf(parseCondition, elseIf) ?? TryElse();

                return new PtIff(End(), elseIf, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private PtExpression Iff()
        {
            return Iff(TK.If, () => RawSequence(), PtIffKind.Iff, PtIffKind.ElseIff);
        }

        private PtExpression Iffdef()
        {
            return Iff(TK.Ifdef, () => Infix(), PtIffKind.IffDef, PtIffKind.ElseIffDef);
        }

        private PtExpression Ifftype()
        {
            return Iff(TK.Iftype, SubType, PtIffKind.IffType, PtIffKind.ElseIffType);
        }

        private PtExpression SubType()
        {
            Begin();
            var sub = Type();
            Match(TK.Subtype);
            var super = Type();

            return new PtSubType(End(), sub, super);
        }


        private PtExpression ThenClause() => TryThenClause() ?? throw NoParse("then");
        private PtExpression? TryThenClause()
        {
            if (MayBegin(TK.Then))
            {
                var annotations = TryAnnotations();
                var body = RawSequence();

                return new PtThen(End(), annotations, body);
            }

            return null;
        }

        private PtElse? TryElse()
        {
            if (MayBegin(TK.Else))
            {
                var annotations = TryAnnotations();
                var elsePart = RawSequence();

                return new PtElse(End(), annotations, elsePart);
            }

            return null;
        }

        private PtExpression RawSequence(NL nl = NL.Both) => TryRawSequence(nl) ?? throw NoParse("raw-sequence");
        private PtExpression? TryRawSequence(NL nl = NL.Both)
        {
            var expression = TryExpressionSequence(nl);
            if (expression == null)
            {
                expression = TryJump();
            }

            return expression;
        }

        private PtExpression? TryExpressionSequence(NL nl = NL.Both)
        {
            var assignment = TryAssignment(nl);

            if (assignment != null)
            {
                var next = TrySemiExpr() ?? TryNoSemi();

                if (next != null)
                {
                    return new PtSequence(Mark(assignment), assignment, next);
                }
            }

            return assignment;
        }

        private PtExpression? TryJump()
        {
            return TokenKind switch
            {
                TK.Return => Jump(PtJumpKind.Return),
                TK.Break => Jump(PtJumpKind.Break),
                TK.Continue => Jump(PtJumpKind.Continue),
                TK.Error => Jump(PtJumpKind.Error),
                TK.CompileIntrinsic => Jump(PtJumpKind.CompileIntrinsic),
                TK.CompileError => Jump(PtJumpKind.CompileError),
                _ => null,
            };
        }

        private PtJump Jump(PtJumpKind kind)
        {
            Begin(TokenKind);
            var value = TryRawSequence();
            return new PtJump(End(), kind, value);
        }

        private PtExpression? TryNoSemi()
        {
            return TryRawSequence(NL.Next);
        }

        private PtExpression? TrySemiExpr()
        {
            if (MayBegin(TK.Semi))
            {
                var expression = RawSequence();
                return new PtSemiExpression(End(), expression);
            }

            return null;
        }

        private PtExpression Assignment(NL nl = NL.Both) => TryAssignment(nl) ?? throw NoParse("assignment");
        private PtExpression? TryAssignment(NL nl = NL.Both)
        {
            var infix = TryInfix(nl);

            if (infix == null)
            {
                return null;
            }

            if (MayMatch(TK.Assign))
            {
                var right = Assignment(NL.Both);
                return new PtAssignment(Mark(infix), infix, right);
            }

            return infix;
        }

        private PtExpression? TryPattern(NL nl = NL.Both)
        {
            return TryLocal() ?? TryParamPattern(nl);
        }

        private PtExpression? TryLocal()
        {
            return TokenKind switch
            {
                TK.Var => Local(PtLocalKind.Var),
                TK.Let => Local(PtLocalKind.Let),
                TK.Embed => Local(PtLocalKind.Embed),
                _ => null,
            };
        }

        private PtLocal Local(PtLocalKind kind)
        {
            Begin(TokenKind);
            var name = Identifier();
            var type = TryColonType();

            return new PtLocal(End(), kind, name, type);
        }

        private PtExpression ParamPattern(NL nl = NL.Both) => TryParamPattern(nl) ?? throw NoParse("param-pattern");
        private PtExpression? TryParamPattern(NL nl = NL.Both)
        {
            var expression = TryPrefix(nl);

            if (expression == null)
            {
                expression = TryPostfix(nl);
            }

            return expression;
        }

        private PtExpression? TryPrefix(NL nl = NL.Both)
        {
            switch (TokenKind)
            {
                case TK.Addressof:
                    return Prefix(PtUnaryKind.Addressof, nl);
                case TK.DigestOf:
                    return Prefix(PtUnaryKind.Digestof, nl);
                case TK.Not:
                    return Prefix(PtUnaryKind.Not, nl);
                case TK.Minus when nl != NL.Next:
                case TK.MinusNew:
                    return Prefix(PtUnaryKind.Minus, nl);
                case TK.MinusTilde when nl != NL.Next:
                case TK.MinusTildeNew:
                    return Prefix(PtUnaryKind.MinusUnsafe, nl);
            }

            return null;
        }

        private PtExpression Prefix(PtUnaryKind kind, NL nl)
        {
            Begin(First.Prefix);
            var expression = ParamPattern(nl != NL.Case ? NL.Both : nl);
            return new PtUnary(End(), kind, expression);
        }

        private PtExpression? TryPostfix(NL nl = NL.Both)
        {
            var atom = TryAtom(nl);

            if (atom == null)
            {
                return null;
            }

            var parts = Collect(TryPostfixPart);
            if (parts.Count > 0)
            {
                return new PtPostfix(Mark(atom), atom, parts);
            }

            return atom;
        }

        private PtPostfixPart? TryPostfixPart()
        {
            return TokenKind switch
            {
                TK.Dot => Dot(),
                TK.Tilde => Tilde(),
                TK.Chain => Chain(),
                TK.LSquare => Qualify(),
                TK.LParen => Call(),
                _ => null,
            };
        }

        private PtDot Dot()
        {
            Begin(TK.Dot);
            var member = Identifier();
            return new PtDot(End(), member);
        }

        private PtTilde Tilde()
        {
            Begin(TK.Tilde);
            var method = Identifier();
            return new PtTilde(End(), method);
        }

        private PtChain Chain()
        {
            Begin(TK.Chain);
            var method = Identifier();
            return new PtChain(End(), method);
        }

        private PtQualify Qualify()
        {
            Begin();
            var arguments = TypeArguments();
            return new PtQualify(End(), arguments);
        }

        private PtCall Call()
        {
            Begin(TK.LParen);
            var arguments = Arguments();
            Match(TK.RParen);
            var partial = MayPartial(); ;

            return new PtCall(End(), arguments, partial);
        }

        private bool MayPartial()
        {
            return MayMatch(TK.Question);
        }

        private PtArguments Arguments()
        {
            Begin();

            var arguments = List<PtArgument>(Positional, TK.Where, TK.RParen);
            if (MayMatch(TK.Where))
            {
                do
                {
                    arguments.Add(Named());
                }
                while (MayMatch(TK.Comma));
            }

            return new PtArguments(End(), arguments);
        }

        private PtPositionalArgument Positional()
        {
            Begin();
            var value = RawSequence();
            return new PtPositionalArgument(End(), value);
        }

        private PtNamedArgument Named()
        {
            Begin();

            var name = Identifier();
            Match(TK.Assign);
            var value = RawSequence();
            return new PtNamedArgument(End(), name, value);
        }

        private PtRef Ref()
        {
            Begin();

            var name = Identifier();
            return new PtRef(End(), name);
        }

        private PtThisLiteral ThisLiteral()
        {
            Begin(TK.This);
            return new PtThisLiteral(End());
        }

        private PtExpression? TryAtom(NL nl = NL.Both)
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

            return null;
        }

        private PtLocation Location()
        {
            Begin(TK.Location);
            return new PtLocation(End());
        }

        private PtObject Object()
        {
            Begin(TK.Object);

            var annotations = TryAnnotations();
            var cap = TryCap();
            var provides = TryProvides();
            var members = Members();

            return new PtObject(End(TK.End), annotations, cap, provides, members);
        }

        private PtLambda Lambda(bool bare)
        {
            Begin(First.Lambda);
            var annotations = TryAnnotations();
            var recCap = TryCap();
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaParameters();
            var captures = TryLambdaCaptures();
            var returnType = TryColonType();
            var partial = MayPartial();
            var body = TryBody();
            Match(TK.RBrace);
            var refCap = TryCap();

            return new PtLambda(End(), bare, annotations, recCap, name, typeParameters, parameters, captures, returnType, partial, body, refCap);
        }

        private PtLambdaParameters LambdaParameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = List(LambdaParameter, TK.RParen);
            return new PtLambdaParameters(End(TK.RParen), parameters);
        }

        private PtLambdaParameter LambdaParameter()
        {
            Begin();
            var name = Identifier();
            var type = TryColonType();
            var value = TryAssignInfix();
            return new PtLambdaParameter(End(), name, type, value);
        }

        private PtLambdaCaptures? TryLambdaCaptures()
        {
            if (MayBegin(TK.LParen, TK.LParenNew))
            {
                var captures = List(LambdaCapture);
                return new PtLambdaCaptures(End(TK.RParen), captures);
            }
            return null;
        }

        private PtLambdaCapture LambdaCapture()
        {
            switch (TokenKind)
            {
                case TK.Identifier:
                    Begin();
                    var name = Identifier();
                    var type = TryColonType();
                    var value = TryAssignInfix();
                    return new PtLambdaCaptureName(End(), name, type, value);
                case TK.This:
                    Begin();
                    var thisLiteral = ThisLiteral();
                    return new PtLambdaCaptureThis(End(), thisLiteral);
            }

            throw NoParse("lambda-capture");
        }


        private PtFfiCall FfiCall()
        {
            Begin(TK.At);
            var name = FfiName();
            var returnType = TryTypeArguments();
            Match(TK.LParen, TK.LParenNew);
            var arguments = Arguments();
            Match(TK.RParen);
            var partial = MayPartial();
            return new PtFfiCall(End(), name, returnType, arguments, partial);
        }

        private PtFfiName FfiName()
        {
            Begin();
            var name = (PtExpression?)TryString() ?? Identifier();
            return new PtFfiName(End(), name);
        }

        private PtGroupedExpression GroupedExpression()
        {
            Begin(TK.LParen, TK.LParenNew);
            var expressions = List(() => RawSequence(), TK.RParen);
            return new PtGroupedExpression(End(TK.RParen), expressions);
        }

        private PtArray Array()
        {
            Begin(TK.LSquare, TK.LSquareNew);
            var type = TryArrayType();
            var elements = TryRawSequence();
            return new PtArray(End(TK.RSquare), type, elements);
        }

        private PtType? TryArrayType()
        {
            if (MayBegin(TK.As))
            {
                var type = Type();
                return new PtArrayType(End(TK.Colon), type);
            }
            return null;
        }

        private PtIdentifier Identifier()
        {
            Begin(TK.Identifier);
            return new PtIdentifier(End());
        }

        private PtIdentifier? TryIdentifier()
        {
            if (Iss(TK.Identifier))
            {
                return Identifier();
            }
            return null;
        }

        private PtChar Char()
        {
            Begin(TK.Char);
            return new PtChar(End());
        }

        private PtString String()
        {
            Begin(TK.String, TK.DocString);
            return new PtString(End());
        }

        private PtString? TryString()
        {
            if (Iss(TK.String, TK.DocString))
            {
                return String();
            }
            return null;
        }

        private PtInt Int()
        {
            Begin(TK.Int);
            return new PtInt(End());
        }

        private PtFloat Float()
        {
            Begin(TK.Float);
            return new PtFloat(End());
        }

        private PtBool Bool(bool value)
        {
            Begin(TK.False, TK.True);
            return new PtBool(End(), value);
        }
    }
}
