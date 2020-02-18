using Joke.Front.Pony.Lex;
using System.Collections.Generic;
using System.Linq;

namespace Joke.Front.Pony.Syntax
{
    partial class PonyParser
    {
        private enum NL
        {
            Both,
            Next,
            Case
        }

        private Ast.Expression Infix(NL nl = NL.Both) => TryInfix(nl) ?? throw NoParse("infix");
        private Ast.Expression? TryInfix(NL nl = NL.Both)
        {
            var term = TryTerm(nl);

            if (term == null)
            {
                return null;
            }

            var parts = new List<Parts.InfixPart>();

            var done = false;
            while (!done && next < limit)
            {
                switch (TokenKind)
                {
                    case TK.And:
                        parts.Add(BinaryPart(Ast.BinaryKind.And));
                        break;
                    case TK.Or:
                        parts.Add(BinaryPart(Ast.BinaryKind.Or));
                        break;
                    case TK.Xor:
                        parts.Add(BinaryPart(Ast.BinaryKind.Xor));
                        break;

                    case TK.Plus:
                        parts.Add(BinaryPart(Ast.BinaryKind.Plus));
                        break;
                    case TK.PlusTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.PlusUnsafe));
                        break;
                    case TK.Minus:
                        parts.Add(BinaryPart(Ast.BinaryKind.Minus));
                        break;
                    case TK.MinusTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.MinusUnsafe));
                        break;
                    case TK.Multiply:
                        parts.Add(BinaryPart(Ast.BinaryKind.Multiply));
                        break;
                    case TK.MultiplyTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.MultiplyUnsafe));
                        break;
                    case TK.Divide:
                        parts.Add(BinaryPart(Ast.BinaryKind.Divide));
                        break;
                    case TK.DivideTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.DivideUnsafe));
                        break;
                    case TK.Rem:
                        parts.Add(BinaryPart(Ast.BinaryKind.Rem));
                        break;
                    case TK.RemTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.RemUnsafe));
                        break;
                    case TK.Mod:
                        parts.Add(BinaryPart(Ast.BinaryKind.Mod));
                        break;
                    case TK.ModTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.ModUnsafe));
                        break;

                    case TK.LShift:
                        parts.Add(BinaryPart(Ast.BinaryKind.LShift));
                        break;
                    case TK.LShiftTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.LShiftUnsafe));
                        break;
                    case TK.RShift:
                        parts.Add(BinaryPart(Ast.BinaryKind.RShift));
                        break;
                    case TK.RShiftTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.RShiftUnsafe));
                        break;

                    case TK.Eq:
                        parts.Add(BinaryPart(Ast.BinaryKind.Eq));
                        break;
                    case TK.EqTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.EqUnsafe));
                        break;
                    case TK.Ne:
                        parts.Add(BinaryPart(Ast.BinaryKind.Ne));
                        break;
                    case TK.NeTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.NeUnsafe));
                        break;
                    case TK.Lt:
                        parts.Add(BinaryPart(Ast.BinaryKind.Lt));
                        break;
                    case TK.LtTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.LtUnsafe));
                        break;
                    case TK.Le:
                        parts.Add(BinaryPart(Ast.BinaryKind.Le));
                        break;
                    case TK.LeTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.LeUnsafe));
                        break;
                    case TK.Gt:
                        parts.Add(BinaryPart(Ast.BinaryKind.Gt));
                        break;
                    case TK.GtTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.GtUnsafe));
                        break;
                    case TK.Ge:
                        parts.Add(BinaryPart(Ast.BinaryKind.Ge));
                        break;
                    case TK.GeTilde:
                        parts.Add(BinaryPart(Ast.BinaryKind.GeUnsafe));
                        break;

                    case TK.Is:
                        parts.Add(IsPart(Ast.BinaryKind.Is));
                        break;
                    case TK.Isnt:
                        parts.Add(IsPart(Ast.BinaryKind.Isnt));
                        break;
                    case TK.As:
                        parts.Add(AsPart());
                        break;

                    default:
                        done = true;
                        break;
                }
            }

            if (parts.Count >= 1)
            {
                var operators = parts.Select(p => p.Kind).Distinct().ToList();
                if (operators.Count >= 2)
                {
                    throw NoParse("binary operators have no precedence, use ( ) to group binary expressions");
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
                        throw NoParse($"{token.Kind} can't be partial");
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
            var body = RawSeq();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Ast.With(End(), annotations, elements, body, elsePart);
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
            var initializer = RawSeq();

            return new Ast.WithElement(End(), names, initializer);
        }

        private Ast.For For()
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

            return new Ast.For(End(), annotations, ids, iterator, body, elsePart);
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
            Match(TK.RParen);

            return new Ast.IdsMulti(End(), idss);
        }

        private Ast.Repeat Repeat()
        {
            Begin(TK.Repeat);
            var annotations = TryAnnotations();
            var body = RawSeq();
            Match(TK.Until);
            var condition = RawSeq();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Ast.Repeat(End(), annotations, body, condition, elsePart);
        }

        private Ast.While While()
        {
            Begin(TK.While);
            var annotations = TryAnnotations();
            var condition = RawSeq();
            Match(TK.Do);
            var body = RawSeq();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Ast.While(End(), annotations, condition, body, elsePart);
        }

        private Ast.Match DoMatch()
        {
            Begin(TK.Match);
            var annotations = TryAnnotations();
            var expr = RawSeq();
            var cases = Cases();
            var elsePart = TryElseClause();
            Match(TK.End);

            return new Ast.Match(End(), annotations, expr, cases, elsePart);
        }

        private Ast.Cases Cases()
        {
            Begin();
            var cases = new List<Ast.Case>();
            while (Iss(TK.Pipe))
            {
                cases.Add(TryCase());
            }

            return new Ast.Cases(End(), cases);
        }

        private Ast.Case TryCase()
        {
            Begin(TK.Pipe);
            var annotations = TryAnnotations();
            var pattern = TryPattern(NL.Case);
            var guard = TryGuard();
            var body = TryBody();

            return new Ast.Case(End(), annotations, pattern, guard, body);
        }

        private Ast.Guard? TryGuard()
        {
            if (MayBegin(TK.If))
            {
                var expression = RawSeq();
                return new Ast.Guard(End(), expression);
            }

            return null;
        }

        private Ast.Consume Consume()
        {
            Begin(TK.Consume);
            var cap = TryCap(false);
            var term = Term();
            return new Ast.Consume(End(), cap, term);
        }

        private Ast.Recover Recover()
        {
            Begin(TK.Recover);
            var annotations = TryAnnotations();
            var cap = TryCap(false);
            var body = RawSeq();
            Match(TK.End);

            return new Ast.Recover(End(), annotations, cap, body);
        }

        public Ast.Expression Try()
        {
            Begin(TK.Try);
            var annotations = TryAnnotations();
            var body = RawSeq();
            var elsePart = TryElseClause();
            var thenPart = TryThenClause();
            Match(TK.End);

            return new Ast.Try(End(), annotations, body, elsePart, thenPart);
        }

        private Ast.Expression Iff()
        {
            Begin(TK.If);
            var annotations = TryAnnotations();
            var condition = RawSeq();
            var thenPart = ThenClause();
            var elsePart = TryElseIf() ?? TryElseClause();
            Match(TK.End);

            return new Ast.Iff(End(), Ast.IffKind.Iff, annotations, condition, thenPart, elsePart);
        }

        private Ast.Expression? TryElseIf()
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = RawSeq();
                var thenPart = ThenClause();
                var elsePart = TryElseIfdef() ?? TryElseClause();

                return new Ast.Iff(End(), Ast.IffKind.ElseIff, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Ast.Expression Iffdef()
        {
            Begin(TK.Ifdef);
            var annotations = TryAnnotations();
            var condition = Infix();
            var thenPart = ThenClause();
            var elsePart = TryElseIfdef() ?? TryElseClause();
            Match(TK.End);

            return new Ast.Iff(End(), Ast.IffKind.IffDef, annotations, condition, thenPart, elsePart);
        }

        private Ast.Expression? TryElseIfdef()
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = Infix();
                var thenPart = ThenClause();
                var elsePart = TryElseIfdef() ?? TryElseClause();

                return new Ast.Iff(End(), Ast.IffKind.ElseIffDef, annotations, condition, thenPart, elsePart);
            }

            return null;
        }

        private Ast.Expression Ifftype()
        {
            Begin(TK.Iftype);
            var annotations = TryAnnotations();
            var condition = SubType();
            var thenPart = ThenClause();
            var elsePart = TryElseIftype() ?? TryElseClause();
            Match(TK.End);

            return new Ast.Iff(End(), Ast.IffKind.IffType, annotations, condition, thenPart, elsePart);
        }

        private Ast.Expression? TryElseIftype()
        {
            if (MayBegin(TK.Elseif))
            {
                var annotations = TryAnnotations();
                var condition = SubType();
                var thenPart = ThenClause();
                var elsePart = TryElseIftype() ?? TryElseClause();

                return new Ast.Iff(End(), Ast.IffKind.ElseIffType, annotations, condition, thenPart, elsePart);
            }

            return null;
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
                var body = RawSeq();

                return new Ast.Then(End(), annotations, body);
            }

            return null;
        }

        private Ast.Else? TryElseClause()
        {
            if (MayBegin(TK.Else))
            {
                var annotations = TryAnnotations();
                var elsePart = RawSeq();

                return new Ast.Else(End(), annotations, elsePart);
            }

            return null;
        }

        private Ast.Expression RawSeq(NL nl = NL.Both) => TryRawSeq(nl) ?? throw NoParse("raw-seq");
        private Ast.Expression? TryRawSeq(NL nl = NL.Both)
        {
            var expression = TryExprSeq(nl);
            if (expression == null)
            {
                expression = TryJump();
            }

            return expression;
        }

        private Ast.Expression? TryExprSeq(NL nl = NL.Both)
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
            var value = TryRawSeq();
            return new Ast.Jump(End(), kind, value);
        }

        private Ast.Expression? TryNoSemi()
        {
            return TryRawSeq(NL.Next);
        }

        private Ast.Expression? TrySemiExpr()
        {
            if (MayBegin(TK.Semi))
            {
                var expression = RawSeq();
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
            var expression = TryLocal();

            if (expression == null)
            {
                expression = TryParamPattern(nl);
            }

            return expression;
        }

        private Ast.Expression? TryLocal()
        {
            if (More())
            {
                var kind = Ast.LocalKind.Missing;

                switch (TokenKind)
                {
                    case TK.Var:
                        kind = Ast.LocalKind.Var;
                        break;
                    case TK.Let:
                        kind = Ast.LocalKind.Let;
                        break;
                    case TK.Embed:
                        kind = Ast.LocalKind.Embed;
                        break;
                }

                if (kind != Ast.LocalKind.Missing)
                {
                    Begin(TokenKind);
                    var name = Identifier();
                    var type = TryColonType();

                    return new Ast.Local(End(), kind, name, type);
                }
            }

            return null;
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
                    return Unary(Ast.UnaryKind.Addressof, nl);
                case TK.DigestOf:
                    return Unary(Ast.UnaryKind.Digestof, nl);
                case TK.Not:
                    return Unary(Ast.UnaryKind.Not, nl);
                case TK.Minus when nl != NL.Next:
                case TK.MinusNew:
                    return Unary(Ast.UnaryKind.Minus, nl);
                case TK.MinusTilde when nl != NL.Next:
                case TK.MinusTildeNew:
                    return Unary(Ast.UnaryKind.MinusUnsafe, nl);
            }

            return null;
        }

        private Ast.Expression Unary(Ast.UnaryKind kind, NL nl)
        {
            Begin(TK.Addressof, TK.DigestOf, TK.Not, TK.Minus, TK.MinusNew, TK.MinusTilde, TK.MinusTildeNew);
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

            var parts = new List<Ast.PostfixPart>();

            var done = false;
            while (!done)
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
                return new Ast.Postfix(Mark(atom), atom, parts);
            }

            return atom;
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
            var value = RawSeq();
            return new Ast.PositionalArgument(End(), value);
        }

        private Ast.NamedArgument Named()
        {
            Begin();

            var name = Identifier();
            Match(TK.Assign);
            var value = RawSeq();
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

        private Ast.Location Location()
        {
            Begin(TK.Location);
            return new Ast.Location(End());
        }

        private Ast.Object Object()
        {
            Begin();
            Match(TK.Object);

            var annotations = TryAnnotations();
            var cap = TryCap(false);
            var provides = TryProvides();
            var members = Members();
            Match(TK.End);
            return new Ast.Object(End(), annotations, cap, provides, members);
        }

        private Ast.Lambda Lambda(bool bare)
        {
            Begin(TK.LBrace, TK.AtLBrace);
            var annotations = TryAnnotations();
            var recCap = TryCap(false);
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaParameters();
            var captures = TryLambdaCaptures();
            var returnType = TryColonType();
            var partial = MayPartial();
            var body = TryBody();
            Match(TK.RBrace);
            var refCap = TryCap(false);

            return new Ast.Lambda(End(), bare, annotations, recCap, name, typeParameters, parameters, captures, returnType, partial, body, refCap);
        }

        private Ast.LambdaParameters LambdaParameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = List(LambdaParameter, TK.RParen);
            Match(TK.RParen);

            return new Ast.LambdaParameters(End(), parameters);
        }

        private Ast.LambdaParameter LambdaParameter()
        {
            Begin();

            var name = Identifier();
            var type = TryColonType();
            var value = TryDefaultInfixArg();
            return new Ast.LambdaParameter(End(), name, type, value);
        }

        private Ast.LambdaCaptures? TryLambdaCaptures()
        {
            if (MayBegin(TK.LParen, TK.LParenNew))
            {
                var captures = List(LambdaCapture);
                Match(TK.RParen);
                return new Ast.LambdaCaptures(End(), captures);
            }

            return null;
        }

        private Ast.LambdaCapture LambdaCapture()
        {
            Ensure();

            switch (TokenKind)
            {
                case TK.Identifier:
                    Begin();
                    var name = Identifier();
                    var type = TryColonType();
                    var value = TryDefaultInfixArg();
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
            var expressions = List(() => RawSeq(), TK.RParen);
            Match(TK.RParen);

            return new Ast.GroupedExpression(End(), expressions);
        }

        private Ast.Array Array()
        {
            Begin(TK.LSquare, TK.LSquareNew);
            var type = TryArrayType();
            var elements = TryRawSeq();
            Match(TK.RSquare);

            return new Ast.Array(End(), type, elements);
        }

        private Ast.Type? TryArrayType()
        {
            if (MayBegin(TK.As))
            {
                var type = Type();
                Match(TK.Colon);

                return new Ast.ArrayType(End(), type);
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
