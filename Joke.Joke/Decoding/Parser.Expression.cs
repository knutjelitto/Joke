using System;
using System.Collections.Generic;
using System.Linq;
using Joke.Joke.Err;
using Joke.Joke.Tree;
using String = Joke.Joke.Tree.String;
using Tuple = Joke.Joke.Tree.Tuple;

namespace Joke.Joke.Decoding
{
    partial class Parser
    {
        private IExpression Expression(bool next = false) // rawseq
        {
            return TryExpression(next) ?? throw new NotImplementedException();
        }

        private IExpression? TryExpression(bool next = false) // rawseq?
        {
            return TrySequence(next) ?? TryJump();
        }

        private IExpression Sequence(bool next = false) // exprseq
        {
            return TrySequence(next) ?? throw new NotImplementedException();
        }

        private IExpression? TrySequence(bool next = false) // exprseq?
        {
            Begin();

            var assignment = TryAssignment(next);

            if (assignment != null)
            {
                if (Is(TK.Semi))
                {
                    Match(TK.Semi);
                    var nextSemi = Expression();
                    return new Sequence(End(), assignment, nextSemi);
                }

                var nextNoSemi = TryExpression(true);

                if (nextNoSemi != null)
                {
                    return new Sequence(End(), assignment, nextNoSemi);
                }
            }

            End();

            return assignment;
        }

        private IExpression Assignment(bool next = false)
        {
            return TryAssignment(next) ?? throw new NotImplementedException();
        }

        private IExpression? TryAssignment(bool next = false)
        {
            Begin();

            var infix = TryInfix(next);

            if (infix != null && Is(TK.Assign))
            {
                Match(TK.Assign);
                var assignment = Assignment();

                return new Assignment(End(), infix, assignment);
            }

            End();
            return infix;
        }

        private IExpression Infix(bool next = false)
        {
            return TryInfix(next) ?? throw new NotImplementedException();
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
                    var types = new List<(Tree.BinaryOp op, IType ty)>();

                    do
                    {
                        Match(Current.Kind);
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
                    var terms = new List<(Tree.BinaryOp op, IExpression ex)>();

                    do
                    {
                        Match(Current.Kind);
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

            End();
            return term;
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
                    if (Next.Kind == TK.Gt && Current.Next == Next.Clutter)
                    {
                        Match(Current.Kind);
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
            return TryTerm(next) ?? throw new NotImplementedException();
        }

        private IExpression? TryTerm(bool next = false)
        {
            switch (Current.Kind)
            {
                case TK.If:
                    return If();
                case TK.Match:
                    break;
                case TK.While:
                    return While();
                case TK.Repeat:
                    break;
                case TK.For:
                    break;
                case TK.With:
                    break;
                case TK.Try:
                    break;
                default:
                    return TryPattern(next);

            }

            throw new NotImplementedException();
        }

        private IExpression While()
        {
            Begin();
            Match(TK.While);
            var condition = Expression();
            Match(TK.Do);
            var body = Expression();
            var @else = TryElse();
            Match(TK.End);

            return new While(End(), condition, body, @else);
        }

        private IExpression If()
        {
            Begin();
            var conditional = Conditional(TK.If);
            var conditionals = Collect(conditional, TryElseIf);
            var @else = TryElse();
            Match(TK.End);

            return new If(End(), conditionals, @else);
        }

        private Conditional Conditional(TK token)
        {
            Begin(token);
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
            if (Is(TK.Else))
            {
                Begin(TK.Else);
                var body = Expression();
                return new Else(End(), body);
            }

            return null;
        }

        private IExpression? TryPattern(bool next = false)
        {
            return TryLocal() ?? TryParamPattern();
        }

        private IExpression? TryLocal()
        {
            switch (Current.Kind)
            {
                case TK.Var:
                    return Local(LocalKind.Var);
                case TK.Let:
                    return Local(LocalKind.Let);
                case TK.Embed:
                    return Local(LocalKind.Embed);
                default:
                    return null;
            }
        }

        private IExpression Local(LocalKind kind)
        {
            Begin(Current.Kind);
            var name = Identifier();
            var type = TryTypeAnnotation();
            return new Local(End(), kind, name, type);
        }

        private IExpression ParamPattern(bool next = false)
        {
            return TryParamPattern(next) ?? throw new NotImplementedException();
        }

        private IExpression? TryParamPattern(bool next = false)
        {
            var op = UnaryOp(next);
            switch (op)
            {
                case Tree.UnaryOp.Missing:
                    return TryPostfix(next);
                default:
                    Begin(Current.Kind);
                    var operand = ParamPattern();
                    return new Unary(End(), op, operand);
            }
        }

        private UnaryOp UnaryOp(bool next)
        {
            switch (Current.Kind)
            {
                case TK.Not:
                    return Tree.UnaryOp.Not;
                case TK.Addressof:
                    return Tree.UnaryOp.Addressof;
                case TK.Digestof:
                    return Tree.UnaryOp.Digestof;
                case TK.Minus when !next || Current.Nl:
                    return Tree.UnaryOp.Minus;
                default:
                    return Tree.UnaryOp.Missing;
            }
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

            End();
            return atom;
        }

        private IExpression? TryPostfixMore(IExpression expression)
        {
            switch (Current.Kind)
            {
                case TK.LParen:
                    {
                        var arguments = Arguments();
                        return new Call(Mark(expression), expression, arguments);
                    }
                case TK.Lt:
                    {
                        var arguments = TypeArguments();
                        return new Generic(Mark(expression), expression, arguments);
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
            Begin(TK.LParen);
            var arguments = CollectOptional(() => TryExpression(), TK.Comma);
            Match(TK.RParen);
            return new ArgumentList(End(), arguments);
        }

        private IExpression? TryAtom(bool next = false)
        {
            switch (Current.Kind)
            {
                case TK.String:
                    return String();
                case TK.DocString:
                    return DocString();
                case TK.This:
                    return ThisValue();
                case TK.Identifier:
                    return Reference();
                case TK.Integer:
                    return Integer();
                case TK.LParen when !next || Current.Nl:
                    return MaybeTuple();
                case TK.LSquare when !next || Current.Nl:
                case TK.Float:
                case TK.True:
                case TK.False:
                case TK.Object:
                case TK.Loc:
                case TK.If:
                case TK.While:
                case TK.For:
                    throw new NotImplementedException();
            }

            return null;
        }

        private IExpression MaybeTuple()
        {
            Begin(TK.LParen);
            var expressions = Collect(() => Expression(), TK.Comma);
            Match(TK.RParen);

            if (expressions.Count == 1)
            {
                End();
                return expressions[0];
            }

            return new Tuple(End(), expressions);
        }

        private String String()
        {
            Begin(TK.String);
            return new String(End());
        }

        private String DocString()
        {
            Begin(TK.DocString);
            return new String(End());
        }

        private String? TryAnyString()
        {
            switch (Current.Kind)
            {
                case TK.String:
                    return String();
                case TK.DocString:
                    return DocString();
            }

            return null;
        }

        private ThisValue ThisValue()
        {
            Begin(TK.This);
            return new ThisValue(End());
        }

        private Reference Reference()
        {
            Begin();
            var name = Identifier();
            return new Reference(End(), name);
        }

        private Integer Integer()
        {
            Begin(TK.Integer);
            return new Integer(End());
        }

        private IExpression? TryJump()
        {
            switch (Current.Kind)
            {
                case TK.Return:
                    return Return();
                case TK.Break:
                    return Break();
                case TK.Continue:
                    return Continue();
                case TK.Error:
                    return Error();
                case TK.CompileIntrinsic:
                    return CompileIntrinsic();
                case TK.CompileError:
                    return CompileError();
            }

            return null;
        }

        private IExpression Return()
        {
            Begin(TK.Return);
            var sequence = TrySequence();
            return new Return(End(), sequence);
        }

        private IExpression Break()
        {
            Begin(TK.Return);
            var sequence = TrySequence();
            return new Break(End(), sequence);
        }

        private IExpression Continue()
        {
            Begin(TK.Return);
            var sequence = TrySequence();
            return new Continue(End(), sequence);
        }

        private IExpression Error()
        {
            Begin(TK.Error);
            var sequence = TrySequence();
            return new Tree.Error(End(), sequence);
        }

        private IExpression CompileIntrinsic()
        {
            Begin(TK.CompileIntrinsic);
            var sequence = TrySequence();
            return new CompileIntrinsic(End(), sequence);
        }

        private IExpression CompileError()
        {
            Begin(TK.CompileError);
            var sequence = TrySequence();
            return new CompileError(End(), sequence);
        }
    }
}
