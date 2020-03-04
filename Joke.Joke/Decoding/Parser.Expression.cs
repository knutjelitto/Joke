using System;
using System.Collections.Generic;
using System.Linq;

using Joke.Joke.Err;
using Joke.Joke.Tree;

namespace Joke.Joke.Decoding
{
    partial class Parser
    {
        private IExpression Expression() // rawseq
        {
            return TrySequence() ?? TryJump() ?? throw new NotImplementedException();
        }

        private IExpression Sequence(bool next = false) // exprseq
        {
            throw new NotImplementedException();
        }

        private IExpression? TrySequence(bool next = false) // exprseq
        {
            var assignment = TryAssignment(next);
            throw new NotImplementedException();
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
                            ErrNo.Scan001,
                            token,
                            "binary operators have no precedence, use ( ) to group binary expressions");
                    }
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
                            ErrNo.Scan001,
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
            var term = TryTerm(next);
            throw new NotImplementedException();
        }
        private IExpression? TryTerm(bool next = false)
        {
            throw new NotImplementedException();
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
            return new Error(End(), sequence);
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
