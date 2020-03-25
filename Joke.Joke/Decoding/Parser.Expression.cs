using System;
using System.Collections.Generic;
using System.Linq;

using Joke.Joke.Err;
using Joke.Joke.Tree;

using String = Joke.Joke.Tree.String;
using Char = Joke.Joke.Tree.Char;
using Tuple = Joke.Joke.Tree.Tuple;
using System.Diagnostics;

namespace Joke.Joke.Decoding
{
    partial class Parser
    {
        private IExpression Expression(bool next = false)
        {
            return TryExpression(next) ?? throw Expected("expression");
        }

        private Exception Expected(string what)
        {
            Errors.AtToken(ErrNo.Scan004, Current, $"can't parse ``{what}´´, at token ``{Keywords.String(Current.Kind)}´´");
            return new NotImplementedException();
        }

        private IExpression? TryExpression(bool next = false)
        {
            return TrySequence(next) ?? TryJump();
        }

        private IExpression Sequence(bool next = false)
        {
            return TrySequence(next) ?? throw Expected("sequence");
        }

        private IExpression? TrySequence(bool next = false) // exprseq?
        {
            Begin();
            var assignment = TryAssignment(next);
            if (assignment != null)
            {
                if (IsMatch(TK.Semi))
                {
                    var nextSemi = Expression();
                    return new Sequence(End(), assignment, nextSemi);
                }

                var nextNoSemi = TryExpression(true);

                if (nextNoSemi != null)
                {
                    return new Sequence(End(), assignment, nextNoSemi);
                }
            }

            return Scrap(assignment);
        }

        private IExpression Assignment(bool next = false)
        {
            return TryAssignment(next) ?? throw Expected("assignment");
        }

        private IExpression? TryAssignment(bool next = false)
        {
            Begin();
            var infix = TryInfix(next);

            if (infix != null && IsMatch(TK.Assign))
            {
                var assignment = Assignment();

                return new Assignment(End(), infix, assignment);
            }

            return Scrap(infix);
        }

        private IExpression Infix(bool next = false)
        {
            return TryInfix(next) ?? throw Expected("infix");
        }

        private IExpression? TryInfix(bool next = false)
        {
            Begin();
            var term = TryTerm(next);

            if (term != null)
            {
                var op = BinaryOp();
                if (op == Tree.BinaryOp.As)
                {
                    var types = new List<(BinaryOp op, IType ty)>();

                    do
                    {
                        MatchAny();
                        var nextType = Type();
                        types.Add((op, nextType));
                    }
                    while ((op = BinaryOp()) != Tree.BinaryOp.Missing);

                    var operators = types.Select(t => t.op).Distinct().ToList();

                    if (operators.Count >= 2)
                    {
                        var part = types.Where(p => p.op != operators[0]).Select(p => p.ty).First();
                        var token = Tokens[part.Span.Start];
                        Errors.AtToken(
                            ErrNo.Scan002,
                            token,
                            "binary operators have no precedence, use ( ) to group binary expressions");
                    }

                    var operands = types.Select(t => t.ty).ToList();
                    return new As(End(), term, operands);
                }
                if (op != Tree.BinaryOp.Missing)
                {
                    var terms = new List<(BinaryOp op, IExpression ex)>();

                    do
                    {
                        MatchAny();
                        var nextTerm = Term();
                        terms.Add((op, nextTerm));
                    }
                    while ((op = BinaryOp()) != Tree.BinaryOp.Missing);

                    var operators = terms.Select(t => t.op).Distinct().ToList();

                    if (operators.Count >= 2)
                    {
                        var part = terms.Where(p => p.op != operators[0]).Select(p => p.ex).First();
                        var token = Tokens[part.Span.Start];
                        Errors.AtToken(
                            ErrNo.Scan002,
                            token,
                            "binary operators have no precedence, use ( ) to group binary expressions");
                    }
                    var @operator = operators[0];
                    var operands = Enumerable.Repeat(term, 1).Concat(terms.Select(t => t.ex)).ToList(); 
                    return new Binary(End(), @operator, operands);
                }
            }

            return Scrap(term);
        }

        private BinaryOp BinaryOp()
        {
            switch (Current.Kind)
            {
                case TK.And:
                    return Tree.BinaryOp.And;
                case TK.Or:
                    return Tree.BinaryOp.Or;
                case TK.Xor:
                    return Tree.BinaryOp.Xor;
                case TK.Plus:
                    return Tree.BinaryOp.Plus;
                case TK.Minus:
                    return Tree.BinaryOp.Minus;
                case TK.Multiply:
                    return Tree.BinaryOp.Multiply;
                case TK.Divide:
                    return Tree.BinaryOp.Divide;
                case TK.Mod:
                    return Tree.BinaryOp.Mod;
                case TK.Rem:
                    return Tree.BinaryOp.Rem;
                case TK.LShift:
                    return Tree.BinaryOp.LShift;
                case TK.RShift:
                    return Tree.BinaryOp.RShift;
                case TK.Eq:
                    return Tree.BinaryOp.Eq;
                case TK.Ne:
                    return Tree.BinaryOp.Ne;
                case TK.Lt:
                    return Tree.BinaryOp.Lt;
                case TK.Le:
                    return Tree.BinaryOp.Le;
                case TK.Gt:
                    if (Next.Kind == TK.Gt && Current.NextOffset == Next.ClutterOffset)
                    {
                        MatchAny();
                        return Tree.BinaryOp.RShift;
                    }
                    return Tree.BinaryOp.Gt;
                case TK.Ge:
                    return Tree.BinaryOp.Ge;
                case TK.Is:
                    return Tree.BinaryOp.Is;
                case TK.Isnt:
                    return Tree.BinaryOp.Isnt;
                case TK.As:
                    return Tree.BinaryOp.As;
                default:
                    return Tree.BinaryOp.Missing;
            }
        }

        private IExpression Term(bool next = false)
        {
            return TryTerm(next) ?? throw Expected("term");
        }

        private IExpression? TryTerm(bool next = false)
        {
            switch (Current.Kind)
            {
                case TK.If:
                    return If();
                case TK.IfDef:
                    return IfDef();
                case TK.Match:
                    return DoMatch();
                case TK.While:
                    return While();
                case TK.Repeat:
                    return Repeat();
                case TK.For:
                    return For();
                case TK.With:
                    break;
                case TK.Try:
                    return Try();
                default:
                    return TryPattern(next);
            }

            throw Expected("not-implemented");
        }

        private For For()
        {
            BeginMatch(TK.For);
            var names = Names();
            Match(TK.In);
            var values = Expression();
            Match(TK.Do);
            var body = Expression();
            var @else = TryElse();
            Match(TK.End);

            return new For(End(), names, values, body, @else);
        }

        private INamePattern Names()
        {
            Begin();

            if (Is(TK.Identifier))
            {
                var name = Identifier();
                return new OneName(End(), name);
            }

            Match(TK.LParen);
            var names = Collect(Names, TK.Comma);
            Match(TK.RParen);

            return new MoreNames(End(), names);
        }

        private Match DoMatch()
        {
            BeginMatch(TK.Match);
            var value = Expression();
            var cases = Collect(TryCase);
            var @else = TryElse();
            Match(TK.End);

            return new Match(End(), value, cases, @else);
        }

        private Case? TryCase()
        {
            if (IsBeginMatch(TK.Pipe))
            {
                var pattern = TryPattern();
                var guard = TryWhenGuard();
                var body = TryBody();

                return new Case(End(), pattern, guard, body);
            }

            return null;
        }

        private When? TryWhenGuard()
        {
            if (IsBeginMatch(TK.When))
            {
                var condition = Expression();
                return new When(End(), condition);
            }

            return null;
        }

        private Try Try()
        {
            BeginMatch(TK.Try);
            var body = Expression();
            var @else = TryElse();
            var then = TryThen();
            Match(TK.End);

            return new Try(End(), body, @else, then);
        }

        private While While()
        {
            BeginMatch(TK.While);
            var condition = Expression();
            Match(TK.Do);
            var body = Expression();
            var @else = TryElse();
            Match(TK.End);

            return new While(End(), condition, body, @else);
        }

        private Repeat Repeat()
        {
            BeginMatch(TK.Repeat);
            var body = Expression();
            Match(TK.Until);
            var condition = Expression();
            var @else = TryElse();
            Match(TK.End);

            return new Repeat(End(), body, condition, @else);
        }

        private IExpression If()
        {
            return If(TK.If, IfKind.If);
        }

        private IExpression IfDef()
        {
            return If(TK.IfDef, IfKind.IfDef);
        }

        private IExpression If(TK whichIf, IfKind kind)
        {
            Begin();
            var conditional = Conditional(whichIf);
            var conditionals = Collect(conditional, TryElseIf);
            var @else = TryElse();
            Match(TK.End);

            return new If(End(), kind, conditionals, @else);
        }

        private Conditional Conditional(TK token)
        {
            BeginMatch(token);
            var condition = Expression();
            Match(TK.Then);
            var thenPart = Expression();
            return new Conditional(End(), condition, thenPart);
        }

        private Conditional? TryElseIf()
        {
            if (Is(TK.Elseif))
            {
                return Conditional(TK.Elseif);
            }

            return null;
        }

        private Else? TryElse()
        {
            if (IsBeginMatch(TK.Else))
            {
                var body = Expression();
                return new Else(End(), body);
            }

            return null;
        }

        private Then? TryThen()
        {
            if (IsBeginMatch(TK.Then))
            {
                var body = Expression();
                return new Then(End(), body);
            }

            return null;
        }

        private IExpression? TryPattern(bool next = false)
        {
            return TryLocal() ?? TryParamPattern(next);
        }

        private IExpression? TryLocal()
        {
            return Current.Kind switch
            {
                TK.Var => Local(LocalKind.Var),
                TK.Let => Local(LocalKind.Let),
                TK.Embed => Local(LocalKind.Embed),
                _ => null,
            };
        }

        private IExpression Local(LocalKind kind)
        {
            BeginMatch(Current.Kind);
            var name = Identifier();
            var type = TryTypeAnnotation();
            var doc = TryString();
            return new Local(End(), kind, name, type, doc);
        }

        private IExpression ParamPattern(bool next = false)
        {
            return TryParamPattern(next) ?? throw Expected("param-pattern");
        }

        private IExpression? TryParamPattern(bool next = false)
        {
            var op = UnaryOp(next);
            switch (op)
            {
                case Tree.UnaryOp.Missing:
                    return TryPostfix(next);
                default:
                    BeginMatch(Current.Kind);
                    var operand = ParamPattern();
                    return new Unary(End(), op, operand);
            }
        }

        private UnaryOp UnaryOp(bool next)
        {
            return Current.Kind switch
            {
                TK.Not => Tree.UnaryOp.Not,
                TK.Addressof => Tree.UnaryOp.Addressof,
                TK.Digestof => Tree.UnaryOp.Digestof,
                TK.Minus when !next || Current.Nl => Tree.UnaryOp.Minus,
                _ => Tree.UnaryOp.Missing,
            };
        }

        private IExpression? TryPostfix(bool next = false)
        {
            Begin();
            var atom = TryAtom(next);

            var post = atom;
            while (post != null)
            {
                atom = post;
                post = TryPostfixMore(atom);
            }

            return Scrap(atom);
        }

        private IExpression? TryPostfixMore(IExpression expression)
        {
            switch (Current.Kind)
            {
                case TK.LParen:
                    {
                        var arguments = Arguments();
                        var throws = TryThrows();
                        return new Call(Mark(expression), expression, arguments, throws);
                    }
                case TK.Lt:
                    {
                        var arguments = TryTypeArguments(); // maybe a binary operator
                        if (arguments != null)
                        {
                            return new Generic(Mark(expression), expression, arguments);
                        }
                        return null;
                    }
                case TK.Dot:
                    {
                        Match(TK.Dot);
                        var member = Identifier();
                        return new Dot(Mark(expression), expression, member);
                    }
                case TK.Tilde:
                    {
                        Match(TK.Tilde);
                        var member = Identifier();
                        return new Tilde(Mark(expression), expression, member);
                    }
                case TK.Chain:
                    {
                        Match(TK.Chain);
                        var member = Identifier();
                        return new Chain(Mark(expression), expression, member);
                    }
                default:
                    return null;
            }
        }

        private ArgumentList Arguments()
        {
            BeginMatch(TK.LParen);
            var arguments = CollectOptional(() => TryExpression(), TK.Comma);
            if (IsMatch(TK.Where))
            {
                var named = Collect(NamedArgument, TK.Comma);
                arguments = arguments.Concat(named).ToList();
            }
            Match(TK.RParen);
            return new ArgumentList(End(), arguments);
        }

        private IExpression NamedArgument()
        {
            Begin();
            var name = Identifier();
            Match(TK.Assign);
            var value = Expression();

            return new NamedArgument(End(), name, value);
        }

        private IExpression? TryAtom(bool next = false)
        {
            switch (Current.Kind)
            {
                case TK.String:
                    return String();
                case TK.Char:
                    return Character();
                case TK.This:
                    return ThisValue();
                case TK.Identifier:
                    return Reference();
                case TK.Wildcard:
                    return Wildcard();
                case TK.Integer:
                    return Integer();
                case TK.True:
                case TK.False:
                    return Bool();
                case TK.LParen when !next || Current.Nl:
                    return MaybeTuple();
                case TK.LBrace:
                    return Lambda();
                case TK.LSquare when !next || Current.Nl:
                case TK.Float:
                case TK.Object:
                case TK.Loc:
                case TK.If:
                case TK.While:
                case TK.For:
                    throw Expected("not-implemented");
            }

            return null;
        }

        private Lambda Lambda()
        {
            BeginMatch(TK.LBrace);
            var x = markers.Count;
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaParameters();
            var captures = TryCaptures();
            var result = TryTypeAnnotation();
            var throws = TryThrows();
            var body = Body();
            Match(TK.RBrace);
            Debug.Assert(x == markers.Count);

            return new Lambda(End(), name, typeParameters, parameters, captures, result, throws, body);
        }

        private ParameterList LambdaParameters()
        {
            BeginMatch(TK.LParen);
            var parameters = CollectOptional(TryLambdaParameter, TK.Comma);
            Match(TK.RParen);
            return new ParameterList(End(), parameters);
        }

        private LambdaParameter? TryLambdaParameter()
        {
            if (Is(TK.Identifier))
            {
                Begin();
                var name = Identifier();
                var type = TryTypeAnnotation();
                var value = TryInitInfix();
                return new LambdaParameter(End(), name, type, value);
            }
            return null;
        }

        private CaptureList? TryCaptures()
        {
            if (IsBeginMatch(TK.LParen))
            {
                var captures = Collect(Capture, TK.Comma);
                Match(TK.RParen);
                return new CaptureList(End(), captures);
            }
            return null;
        }

        private ICapture Capture()
        {
            if (Is(TK.This))
            {
                BeginMatch(TK.This);
                return new ThisCapture(End());
            }

            Begin();
            var name = Identifier();
            var type = TryTypeAnnotation();
            var value = TryInitInfix();
            return new NameCapture(End(), name, type, value);
        }

        private Bool Bool()
        {
            BeginMatch(Current.Kind);
            return new Bool(End());
        }

        private IExpression MaybeTuple()
        {
            BeginMatch(TK.LParen);
            var expressions = Collect(() => Expression(), TK.Comma);
            Match(TK.RParen);
            return Singularize(expressions) ?? new Tuple(End(), expressions);
        }

        private Char Character()
        {
            BeginMatch(TK.Char);
            return new Char(End());
        }

        private String String()
        {
            BeginMatch(TK.String);
            return new String(End());
        }

        private String? TryString()
        {
            if (Is(TK.String))
            {
                return String();
            }
            return null;
        }

        private ThisValue ThisValue()
        {
            BeginMatch(TK.This);
            return new ThisValue(End());
        }

        private Reference Reference()
        {
            Begin();
            var name = Identifier();
            return new Reference(End(), name);
        }

        private Wildcard Wildcard()
        {
            BeginMatch(TK.Wildcard);
            return new Wildcard(End());
        }

        private Integer Integer()
        {
            BeginMatch(TK.Integer);
            return new Integer(End());
        }

        private IExpression? TryJump()
        {
            return Current.Kind switch
            {
                TK.Return => Return(),
                TK.Break => Break(),
                TK.Continue => Continue(),
                TK.Error => Error(),
                TK.CompileIntrinsic => CompileIntrinsic(),
                TK.CompileError => CompileError(),
                _ => null,
            };
        }

        private IExpression Return()
        {
            BeginMatch(TK.Return);
            var sequence = TrySequence();
            return new Return(End(), sequence);
        }

        private IExpression Break()
        {
            BeginMatch(TK.Break);
            var sequence = TrySequence();
            return new Break(End(), sequence);
        }

        private IExpression Continue()
        {
            BeginMatch(TK.Continue);
            var sequence = TrySequence();
            return new Continue(End(), sequence);
        }

        private IExpression Error()
        {
            BeginMatch(TK.Error);
            var sequence = TrySequence();
            return new Tree.Error(End(), sequence);
        }

        private IExpression CompileIntrinsic()
        {
            BeginMatch(TK.CompileIntrinsic);
            var sequence = TrySequence();
            return new CompileIntrinsic(End(), sequence);
        }

        private IExpression CompileError()
        {
            BeginMatch(TK.CompileError);
            var sequence = TrySequence();
            return new CompileError(End(), sequence);
        }
    }
}
