using System;
using System.Collections.Generic;
using System.Linq;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing
{
    partial class PonyParser
    {
        private Ast.Expression Infix(NL nl = NL.Both) => TryInfix(nl) ?? throw NoParse("infix");
        private Ast.Expression? TryInfix(NL nl = NL.Both)
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
                    case Ast.BinaryKind.As:
                        if (parts.Count > 1)
                        {
                            throw NoParse("binary operator <as> doesn't associate");
                        }
                        return new Ast.As(Mark(term), term, ((Parts.AsPart)parts[0]).Type);
                    case Ast.BinaryKind.Is:
                    case Ast.BinaryKind.Isnt:
                        if (parts.Count > 1)
                        {
                            throw NoParse("binary operator <is>/<isnt> doesn't associate");
                        }
                        return new Ast.Binary(Mark(term), @operator, term, ((Parts.IsPart)parts[0]).Expression);
                    default:
                        var operands = new List<Ast.Expression> { term };
                        operands.AddRange(parts.Cast<Parts.BinaryPart>().Select(b => b.Right));
                        return new Ast.Binary(Mark(term), @operator, operands);
                }
            }

            return term;
        }

        private Parts.InfixPart? TryInfixPart()
        {
            return TokenKind switch
            {
                TK.And => BinaryPart(Ast.BinaryKind.And),
                TK.Or => BinaryPart(Ast.BinaryKind.Or),
                TK.Xor => BinaryPart(Ast.BinaryKind.Xor),
                TK.Plus => BinaryPart(Ast.BinaryKind.Plus),
                TK.PlusTilde => BinaryPart(Ast.BinaryKind.PlusUnsafe),
                TK.Minus => BinaryPart(Ast.BinaryKind.Minus),
                TK.MinusTilde => BinaryPart(Ast.BinaryKind.MinusUnsafe),
                TK.Multiply => BinaryPart(Ast.BinaryKind.Multiply),
                TK.MultiplyTilde => BinaryPart(Ast.BinaryKind.MultiplyUnsafe),
                TK.Divide => BinaryPart(Ast.BinaryKind.Divide),
                TK.DivideTilde => BinaryPart(Ast.BinaryKind.DivideUnsafe),
                TK.Rem => BinaryPart(Ast.BinaryKind.Rem),
                TK.RemTilde => BinaryPart(Ast.BinaryKind.RemUnsafe),
                TK.Mod => BinaryPart(Ast.BinaryKind.Mod),
                TK.ModTilde => BinaryPart(Ast.BinaryKind.ModUnsafe),
                TK.LShift => BinaryPart(Ast.BinaryKind.LShift),
                TK.LShiftTilde => BinaryPart(Ast.BinaryKind.LShiftUnsafe),
                TK.RShift => BinaryPart(Ast.BinaryKind.RShift),
                TK.RShiftTilde => BinaryPart(Ast.BinaryKind.RShiftUnsafe),
                TK.Eq => BinaryPart(Ast.BinaryKind.Eq),
                TK.EqTilde => BinaryPart(Ast.BinaryKind.EqUnsafe),
                TK.Ne => BinaryPart(Ast.BinaryKind.Ne),
                TK.NeTilde => BinaryPart(Ast.BinaryKind.NeUnsafe),
                TK.Lt => BinaryPart(Ast.BinaryKind.Lt),
                TK.LtTilde => BinaryPart(Ast.BinaryKind.LtUnsafe),
                TK.Le => BinaryPart(Ast.BinaryKind.Le),
                TK.LeTilde => BinaryPart(Ast.BinaryKind.LeUnsafe),
                TK.Gt => BinaryPart(Ast.BinaryKind.Gt),
                TK.GtTilde => BinaryPart(Ast.BinaryKind.GtUnsafe),
                TK.Ge => BinaryPart(Ast.BinaryKind.Ge),
                TK.GeTilde => BinaryPart(Ast.BinaryKind.GeUnsafe),
                TK.Is => IsPart(Ast.BinaryKind.Is),
                TK.Isnt => IsPart(Ast.BinaryKind.Isnt),
                TK.As => AsPart(),
                _ => null,
            };
        }

        private Parts.BinaryPart BinaryPart(Ast.BinaryKind kind)
        {
            var token = Token;

            Begin(TokenKind);
            var partial = MayPartial();
            var term = Term(NL.Both);

            if (partial)
            {
                switch (kind)
                {
                    case Ast.BinaryKind.Plus:
                        kind = Ast.BinaryKind.PlusPartial;
                        break;
                    case Ast.BinaryKind.Minus:
                        kind = Ast.BinaryKind.MinusPartial;
                        break;
                    case Ast.BinaryKind.Multiply:
                        kind = Ast.BinaryKind.MultiplyPartial;
                        break;
                    case Ast.BinaryKind.Divide:
                        kind = Ast.BinaryKind.DividePartial;
                        break;
                    case Ast.BinaryKind.Mod:
                        kind = Ast.BinaryKind.ModPartial;
                        break;
                    case Ast.BinaryKind.Rem:
                        kind = Ast.BinaryKind.RemPartial;
                        break;
                    default:
                        AddError(token, $"{Keywords.String(token.Kind)} can't be made partial");
                        break;
                }
            }

            return new Parts.BinaryPart(End(), kind, term);
        }

        private Parts.IsPart IsPart(Ast.BinaryKind kind)
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

        private Ast.Expression Term(NL nl = NL.Both) => TryTerm(nl) ?? throw NoParse("term");
        private Ast.Expression? TryTerm(NL nl = NL.Both)
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

        private Ast.With With()
        {
            Begin(TK.With);
            var annotations = TryAnnotations();
            var elements = WithElements();
            Match(TK.Do);
            var body = RawSequence();
            var elsePart = TryElse();

            return new Ast.With(End(TK.End), annotations, elements, body, elsePart);
        }

        private Ast.WithElements WithElements()
        {
            Begin();
            var elements = List(WithElement);
            return new Ast.WithElements(End(), elements);
        }

        private Ast.WithElement WithElement()
        {
            Begin();
            var names = Ids();
            Match(TK.Assign);
            var initializer = RawSequence();
            return new Ast.WithElement(End(), names, initializer);
        }

        private Ast.For For()
        {
            Begin(TK.For);
            var annotations = TryAnnotations();
            var ids = Ids();
            Match(TK.In);
            var iterator = RawSequence();
            Match(TK.Do);
            var body = RawSequence();
            var elsePart = TryElse();

            return new Ast.For(End(TK.End), annotations, ids, iterator, body, elsePart);
        }

        private Ast.Ids Ids()
        {
            if (Iss(TK.Identifier))
            {
                return IdsSingle();
            }
            
            return IdsMulti();
        }

        private Ast.IdsSingle IdsSingle()
        {
            Begin();
            var name = Identifier();
            return new Ast.IdsSingle(End(), name);
        }

        private Ast.IdsMulti IdsMulti()
        {
            Begin(TK.LParen, TK.LParenNew);
            var idss = List(Ids);

            return new Ast.IdsMulti(End(TK.RParen), idss);
        }

        private Ast.Repeat Repeat()
        {
            Begin(TK.Repeat);
            var annotations = TryAnnotations();
            var body = RawSequence();
            Match(TK.Until);
            var condition = RawSequence();
            var elsePart = TryElse();

            return new Ast.Repeat(End(TK.End), annotations, body, condition, elsePart);
        }

        private Ast.While While()
        {
            Begin(TK.While);
            var annotations = TryAnnotations();
            var condition = RawSequence();
            Match(TK.Do);
            var body = RawSequence();
            var elsePart = TryElse();

            return new Ast.While(End(TK.End), annotations, condition, body, elsePart);
        }

        private Ast.Match DoMatch()
        {
            Begin(TK.Match);
            var annotations = TryAnnotations();
            var expr = RawSequence();
            var cases = Cases();
            var elsePart = TryElse();


            return new Ast.Match(End(TK.End), annotations, expr, cases, elsePart);
        }

        private Ast.Cases Cases()
        {
            Begin();
            var cases = Collect(TryCase);

            return new Ast.Cases(End(), cases);
        }

        private Ast.Case? TryCase()
        {
            if (MayBegin(TK.Pipe))
            {
                var annotations = TryAnnotations();
                var pattern = TryPattern(NL.Case);
                var guard = TryCaseGuard();
                var body = TryBody();

                return new Ast.Case(End(), annotations, pattern, guard, body);
            }

            return null;
        }

        private Ast.Guard? TryCaseGuard()
        {
            if (MayBegin(TK.If))
            {
                var expression = RawSequence();
                return new Ast.Guard(End(), expression);
            }

            return null;
        }

        private Ast.Consume Consume()
        {
            Begin(TK.Consume);
            var cap = TryCap();
            var term = Term();
            return new Ast.Consume(End(), cap, term);
        }

        private Ast.Recover Recover()
        {
            Begin(TK.Recover);
            var annotations = TryAnnotations();
            var cap = TryCap();
            var body = RawSequence();


            return new Ast.Recover(End(TK.End), annotations, cap, body);
        }

        public Ast.Expression Try()
        {
            Begin(TK.Try);
            var annotations = TryAnnotations();
            var body = RawSequence();
            var elsePart = TryElse();
            var thenPart = TryThenClause();


            return new Ast.Try(End(TK.End), annotations, body, elsePart, thenPart);
        }

        private Ast.Expression Iff(TK iffToken, Func<Ast.Expression> parseCondition, Ast.IffKind iff, Ast.IffKind elseIf)
        {
            Begin(iffToken);
            var annotations = TryAnnotations();
            var condition = parseCondition();
            var thenPart = ThenClause();
            var elsePart = TryElseIf(parseCondition, elseIf) ?? TryElse();


            return new Ast.Iff(End(TK.End), iff, annotations, condition, thenPart, elsePart);
        }

        private Ast.Expression? TryElseIf(Func<Ast.Expression> parseCondition, Ast.IffKind elseIf)
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = parseCondition();
                var thenPart = ThenClause();
                var elsePart = TryElseIf(parseCondition, elseIf) ?? TryElse();

                return new Ast.Iff(End(), elseIf, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Ast.Expression Iff()
        {
            return Iff(TK.If, () => RawSequence(), Ast.IffKind.Iff, Ast.IffKind.ElseIff);
        }

        private Ast.Expression Iffdef()
        {
            return Iff(TK.Ifdef, () => Infix(), Ast.IffKind.IffDef, Ast.IffKind.ElseIffDef);
        }

        private Ast.Expression Ifftype()
        {
            return Iff(TK.Iftype, SubType, Ast.IffKind.IffType, Ast.IffKind.ElseIffType);
        }

        private Ast.Expression SubType()
        {
            Begin();
            var sub = Type();
            Match(TK.Subtype);
            var super = Type();

            return new Ast.SubType(End(), sub, super);
        }


        private Ast.Expression ThenClause() => TryThenClause() ?? throw NoParse("then");
        private Ast.Expression? TryThenClause()
        {
            if (MayBegin(TK.Then))
            {
                var annotations = TryAnnotations();
                var body = RawSequence();

                return new Ast.Then(End(), annotations, body);
            }

            return null;
        }

        private Ast.Else? TryElse()
        {
            if (MayBegin(TK.Else))
            {
                var annotations = TryAnnotations();
                var elsePart = RawSequence();

                return new Ast.Else(End(), annotations, elsePart);
            }

            return null;
        }

        private Ast.Expression RawSequence(NL nl = NL.Both) => TryRawSequence(nl) ?? throw NoParse("raw-sequence");
        private Ast.Expression? TryRawSequence(NL nl = NL.Both)
        {
            var expression = TryExpressionSequence(nl);
            if (expression == null)
            {
                expression = TryJump();
            }

            return expression;
        }

        private Ast.Expression? TryExpressionSequence(NL nl = NL.Both)
        {
            var assignment = TryAssignment(nl);

            if (assignment != null)
            {
                var next = TrySemiExpr() ?? TryNoSemi();

                if (next != null)
                {
                    return new Ast.Sequence(Mark(assignment), assignment, next);
                }
            }

            return assignment;
        }

        private Ast.Expression? TryJump()
        {
            return TokenKind switch
            {
                TK.Return => Jump(Ast.JumpKind.Return),
                TK.Break => Jump(Ast.JumpKind.Break),
                TK.Continue => Jump(Ast.JumpKind.Continue),
                TK.Error => Jump(Ast.JumpKind.Error),
                TK.CompileIntrinsic => Jump(Ast.JumpKind.CompileIntrinsic),
                TK.CompileError => Jump(Ast.JumpKind.CompileError),
                _ => null,
            };
        }

        private Ast.Jump Jump(Ast.JumpKind kind)
        {
            Begin(TokenKind);
            var value = TryRawSequence();
            return new Ast.Jump(End(), kind, value);
        }

        private Ast.Expression? TryNoSemi()
        {
            return TryRawSequence(NL.Next);
        }

        private Ast.Expression? TrySemiExpr()
        {
            if (MayBegin(TK.Semi))
            {
                var expression = RawSequence();
                return new Ast.SemiExpression(End(), expression);
            }

            return null;
        }

        private Ast.Expression Assignment(NL nl = NL.Both) => TryAssignment(nl) ?? throw NoParse("assignment");
        private Ast.Expression? TryAssignment(NL nl = NL.Both)
        {
            var infix = TryInfix(nl);

            if (infix == null)
            {
                return null;
            }

            if (MayMatch(TK.Assign))
            {
                var right = Assignment(NL.Both);
                return new Ast.Assignment(Mark(infix), infix, right);
            }

            return infix;
        }

        private Ast.Expression? TryPattern(NL nl = NL.Both)
        {
            return TryLocal() ?? TryParamPattern(nl);
        }

        private Ast.Expression? TryLocal()
        {
            return TokenKind switch
            {
                TK.Var => Local(Ast.LocalKind.Var),
                TK.Let => Local(Ast.LocalKind.Let),
                TK.Embed => Local(Ast.LocalKind.Embed),
                _ => null,
            };
        }

        private Ast.Local Local(Ast.LocalKind kind)
        {
            Begin(TokenKind);
            var name = Identifier();
            var type = TryColonType();

            return new Ast.Local(End(), kind, name, type);
        }

        private Ast.Expression ParamPattern(NL nl = NL.Both) => TryParamPattern(nl) ?? throw NoParse("param-pattern");
        private Ast.Expression? TryParamPattern(NL nl = NL.Both)
        {
            var expression = TryPrefix(nl);

            if (expression == null)
            {
                expression = TryPostfix(nl);
            }

            return expression;
        }

        private Ast.Expression? TryPrefix(NL nl = NL.Both)
        {
            switch (TokenKind)
            {
                case TK.Addressof:
                    return Prefix(Ast.UnaryKind.Addressof, nl);
                case TK.DigestOf:
                    return Prefix(Ast.UnaryKind.Digestof, nl);
                case TK.Not:
                    return Prefix(Ast.UnaryKind.Not, nl);
                case TK.Minus when nl != NL.Next:
                case TK.MinusNew:
                    return Prefix(Ast.UnaryKind.Minus, nl);
                case TK.MinusTilde when nl != NL.Next:
                case TK.MinusTildeNew:
                    return Prefix(Ast.UnaryKind.MinusUnsafe, nl);
            }

            return null;
        }

        private Ast.Expression Prefix(Ast.UnaryKind kind, NL nl)
        {
            Begin(First.Prefix);
            var expression = ParamPattern(nl != NL.Case ? NL.Both : nl);
            return new Ast.Unary(End(), kind, expression);
        }

        private Ast.Expression? TryPostfix(NL nl = NL.Both)
        {
            var atom = TryAtom(nl);

            if (atom == null)
            {
                return null;
            }

            var parts = Collect(TryPostfixPart);
            if (parts.Count > 0)
            {
                return new Ast.Postfix(Mark(atom), atom, parts);
            }

            return atom;
        }

        private Ast.PostfixPart? TryPostfixPart()
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

        private Ast.Dot Dot()
        {
            Begin(TK.Dot);
            var member = Identifier();
            return new Ast.Dot(End(), member);
        }

        private Ast.Tilde Tilde()
        {
            Begin(TK.Tilde);
            var method = Identifier();
            return new Ast.Tilde(End(), method);
        }

        private Ast.Chain Chain()
        {
            Begin(TK.Chain);
            var method = Identifier();
            return new Ast.Chain(End(), method);
        }

        private Ast.Qualify Qualify()
        {
            Begin();
            var arguments = TypeArguments();
            return new Ast.Qualify(End(), arguments);
        }

        private Ast.Call Call()
        {
            Begin(TK.LParen);
            var arguments = Arguments();
            Match(TK.RParen);
            var partial = MayPartial(); ;

            return new Ast.Call(End(), arguments, partial);
        }

        private bool MayPartial()
        {
            return MayMatch(TK.Question);
        }

        private Ast.Arguments Arguments()
        {
            Begin();

            var arguments = List<Ast.Argument>(Positional, TK.Where, TK.RParen);
            if (MayMatch(TK.Where))
            {
                do
                {
                    arguments.Add(Named());
                }
                while (MayMatch(TK.Comma));
            }

            return new Ast.Arguments(End(), arguments);
        }

        private Ast.PositionalArgument Positional()
        {
            Begin();
            var value = RawSequence();
            return new Ast.PositionalArgument(End(), value);
        }

        private Ast.NamedArgument Named()
        {
            Begin();

            var name = Identifier();
            Match(TK.Assign);
            var value = RawSequence();
            return new Ast.NamedArgument(End(), name, value);
        }

        private Ast.Ref Ref()
        {
            Begin();

            var name = Identifier();
            return new Ast.Ref(End(), name);
        }

        private Ast.ThisLiteral ThisLiteral()
        {
            Begin(TK.This);
            return new Ast.ThisLiteral(End());
        }

        private Ast.Expression? TryAtom(NL nl = NL.Both)
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

        private Ast.Location Location()
        {
            Begin(TK.Location);
            return new Ast.Location(End());
        }

        private Ast.Object Object()
        {
            Begin(TK.Object);

            var annotations = TryAnnotations();
            var cap = TryCap();
            var provides = TryProvides();
            var members = Members();

            return new Ast.Object(End(TK.End), annotations, cap, provides, members);
        }

        private Ast.Lambda Lambda(bool bare)
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

            return new Ast.Lambda(End(), bare, annotations, recCap, name, typeParameters, parameters, captures, returnType, partial, body, refCap);
        }

        private Ast.LambdaParameters LambdaParameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = List(LambdaParameter, TK.RParen);
            return new Ast.LambdaParameters(End(TK.RParen), parameters);
        }

        private Ast.LambdaParameter LambdaParameter()
        {
            Begin();
            var name = Identifier();
            var type = TryColonType();
            var value = TryAssignInfix();
            return new Ast.LambdaParameter(End(), name, type, value);
        }

        private Ast.LambdaCaptures? TryLambdaCaptures()
        {
            if (MayBegin(TK.LParen, TK.LParenNew))
            {
                var captures = List(LambdaCapture);
                return new Ast.LambdaCaptures(End(TK.RParen), captures);
            }
            return null;
        }

        private Ast.LambdaCapture LambdaCapture()
        {
            switch (TokenKind)
            {
                case TK.Identifier:
                    Begin();
                    var name = Identifier();
                    var type = TryColonType();
                    var value = TryAssignInfix();
                    return new Ast.LambdaCaptureName(End(), name, type, value);
                case TK.This:
                    Begin();
                    var thisLiteral = ThisLiteral();
                    return new Ast.LambdaCaptureThis(End(), thisLiteral);
            }

            throw NoParse("lambda-capture");
        }


        private Ast.FfiCall FfiCall()
        {
            Begin(TK.At);
            var name = FfiName();
            var returnType = TryTypeArguments();
            Match(TK.LParen, TK.LParenNew);
            var arguments = Arguments();
            Match(TK.RParen);
            var partial = MayPartial();
            return new Ast.FfiCall(End(), name, returnType, arguments, partial);
        }

        private Ast.FfiName FfiName()
        {
            Begin();
            var name = (Ast.Expression?)TryString() ?? Identifier();
            return new Ast.FfiName(End(), name);
        }

        private Ast.GroupedExpression GroupedExpression()
        {
            Begin(TK.LParen, TK.LParenNew);
            var expressions = List(() => RawSequence(), TK.RParen);
            return new Ast.GroupedExpression(End(TK.RParen), expressions);
        }

        private Ast.Array Array()
        {
            Begin(TK.LSquare, TK.LSquareNew);
            var type = TryArrayType();
            var elements = TryRawSequence();
            return new Ast.Array(End(TK.RSquare), type, elements);
        }

        private Ast.Type? TryArrayType()
        {
            if (MayBegin(TK.As))
            {
                var type = Type();
                return new Ast.ArrayType(End(TK.Colon), type);
            }
            return null;
        }

        public Ast.Identifier Identifier()
        {
            Begin(TK.Identifier);
            return new Ast.Identifier(End());
        }

        public Ast.Identifier? TryIdentifier()
        {
            if (Iss(TK.Identifier))
            {
                return Identifier();
            }
            return null;
        }

        private Ast.Char Char()
        {
            Begin(TK.Char);
            return new Ast.Char(End());
        }

        private Ast.String String()
        {
            Begin(TK.String, TK.DocString);
            return new Ast.String(End());
        }

        private Ast.String? TryString()
        {
            if (Iss(TK.String, TK.DocString))
            {
                return String();
            }
            return null;
        }

        private Ast.Int Int()
        {
            Begin(TK.Int);
            return new Ast.Int(End());
        }

        private Ast.Float Float()
        {
            Begin(TK.Float);
            return new Ast.Float(End());
        }

        private Ast.Bool Bool(bool value)
        {
            Begin(TK.False, TK.True);
            return new Ast.Bool(End(), value);
        }
    }
}
