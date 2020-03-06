using System;
using System.Collections.Generic;
using System.Diagnostics;

using Joke.Joke.Err;
using Joke.Joke.Tree;
using String = Joke.Joke.Tree.String;

namespace Joke.Joke.Decoding
{
    public sealed partial class Parser
    {
        public Parser(Errors errors, Tokens tokens)
        {
            Errors = errors;
            Tokens = tokens;

            Debug.Assert(Tokens.Count > 0 && Tokens[^1].Kind == TK.Eof);

            next = 0;
            limit = Tokens.Count;
        }

        public Errors Errors { get; }
        public Tokens Tokens { get; }
        public Token Current => next < limit ? Tokens[next] : Tokens[limit - 1];
        public Token Next => next + 1 < limit ? Tokens[next + 1] : Tokens[limit - 1];
        public bool CurrentIsDoc => Current.Kind == TK.String || Current.Kind == TK.DocString;

        private readonly Stack<int> markers = new Stack<int>();

        private int next;
        private readonly int limit;

        public Namespace ParseUnit()
        {
            next = 0;

            Begin();
            Match(TK.Namespace);
            var name = QualifiedIdentifier();
            var members = NamespaceMembers();
            Match(TK.End);

            if (!Is(TK.Eof))
            {
                Errors.AtToken(ErrNo.Scan002, Current, "inconclusive parse, not at ``EOF´´");
            }

            var @namespace = new Namespace(End(), name, members);

            return @namespace;
        }

        private MemberList NamespaceMembers()
        {
            Begin();
            var items = Collect(TryNamespaceMember);
            return new MemberList(End(), items);
        }

        private IMember? TryNamespaceMember()
        {
            return TryClassType() ?? TryExtern();
        }

        private IMember? TryExtern()
        {
            if (Is(TK.Extern))
            {
                Begin();
                Match(TK.Extern);
                var name = Identifier();
                var funs = Collect(TryMethod);
                Match(TK.End);
                return new Extern(End(), funs);
            }

            return null;
        }

        private IMember? TryClassType()
        {
            switch (CurrentIsDoc ? Next.Kind : Current.Kind)
            {
                case TK.Type:
                    return ClassType(ClassKind.Alias);
                case TK.Primitive:
                    return ClassType(ClassKind.Primitive);
                case TK.Interface:
                    return ClassType(ClassKind.Interface);
                case TK.Struct:
                    return ClassType(ClassKind.Struct);
                case TK.Class:
                    return ClassType(ClassKind.Class);
                case TK.Trait:
                    return ClassType(ClassKind.Trait);
                case TK.Actor:
                    return ClassType(ClassKind.Actor);
                default:
                    return null;
            }
        }

        private ClassType ClassType(ClassKind kind)
        {
            Begin();
            var doc = TryAnyString();
            Match(Current.Kind);
            var name = Identifier();
            var typeparameters = TryTypeParameters();
            var provides = TryProvides();
            var members = ClassMembers();

            return new ClassType(End(), kind, doc, name, typeparameters, provides, members);
        }

        private MemberList ClassMembers()
        {
            Begin();
            var members = Collect(TryClassMember);
            return new MemberList(End(), members);
        }

        private IMember? TryClassMember()
        {
            return TryField() ?? TryMethod();
        }

        private IMember? TryField()
        {
            switch (CurrentIsDoc ? Next.Kind : Current.Kind)
            {
                case TK.Let:
                    return Field(FieldKind.Let);
                case TK.Var:
                    return Field(FieldKind.Var);
                case TK.Embed:
                    return Field(FieldKind.Embed);
                default:
                    return null;
            }
        }
        private IMember Field(FieldKind kind)
        {
            Begin();
            var doc = TryAnyString();
            Match(Current.Kind);
            var name = Identifier();
            var type = TypeAnnotation();
            var init = TryInitInfix();

            return new Field(End(), kind, doc, name, type, init);
        }

        private IMember? TryMethod()
        {
            switch (CurrentIsDoc ? Next.Kind : Current.Kind)
            {
                case TK.Fun:
                    return Method(MethodKind.Fun);
                case TK.New:
                    return Method(MethodKind.New);
                case TK.Be:
                    return Method(MethodKind.Be);
                default:
                    return null;
            }
        }

        private IMember Method(MethodKind kind)
        {
            Begin();
            var doc = TryAnyString();
            Match(Current.Kind);
            var name = Identifier();
            Console.WriteLine($"{kind} - {name}");
            var typeParameters = TryTypeParameters();
            var parameters = ValueParameters();
            var @return = TryTypeAnnotation();
            var body = TryBody();

            return new Method(End(), kind, doc, name, typeParameters, parameters, @return, body);
        }

        private IExpression? TryInitInfix()
        {
            if (Is(TK.Assign))
            {
                Match(TK.Assign);
                return Infix();
            }

            return null;
        }

        private IExpression? TryBody()
        {
            if (MayMatch(TK.DblArrow))
            {
                return Expression();
            }
            return null;
        }

        private ValueParameterList ValueParameters()
        {
            Begin(TK.LParen);
            var items = CollectOptional(TryParameter, TK.Comma);
            Match(TK.RParen);
            return new ValueParameterList(End(), items);
        }

        private ValueParameter? TryParameter()
        {
            if (Is(TK.Identifier))
            {
                Begin();
                var name = Identifier();
                var type = TypeAnnotation();
                var @default = TryValueDefault();
                return new ValueParameter(End(), name, type, @default);
            }
            return null;
        }

        private IExpression? TryValueDefault()
        {
            if (Is(TK.Assign))
            {
                Match(TK.Assign);
                return Expression();
            }
            return null;
        }

        private TypeParameterList? TryTypeParameters()
        {
            if (Is(TK.Lt))
            {
                Begin(TK.Lt);
                var items = Collect(TypeParameter, TK.Comma);
                Match(TK.Gt);

                return new TypeParameterList(End(), items);
            }
            return null;
        }

        private TypeParameter TypeParameter()
        {
            Begin();
            var name = Identifier();
            var type = TryTypeAnnotation();
            var @default = TryTypeDefault();
            return new TypeParameter(End(), name, type, @default);
        }

        private IType? TryTypeAnnotation()
        {
            if (Is(TK.Colon))
            {
                Match(TK.Colon);
                return Type();
            }
            return null;
        }

        private IType TypeAnnotation()
        {
            Match(TK.Colon);
            return Type();
        }

        private IType? TryTypeDefault()
        {
            if (Is(TK.Assign))
            {
                Match(TK.Assign);
                return Type();
            }
            return null;
        }

        private IType Provides()
        {
            return TryProvides() ?? throw new NotImplementedException();
        }

        private IType? TryProvides()
        {
            if (MayMatch(TK.Is))
            {
                return Type();
            }
            return null;
        }

        private IType Type()
        {
            switch (Current.Kind)
            {
                case TK.This:
                    return ThisType();
                case TK.LParen:
                    return TupleType();
                default:
                    return NominalType();

            }
        }

        private IType NominalType()
        {
            Begin();
            var name = QualifiedIdentifier();
            var arguments = TryTypeArguments();
            return new NominalType(End(), name, arguments);
        }

        private TypeList TypeArguments()
        {
            return TryTypeArguments() ?? throw new NotImplementedException();
        }
        private TypeList? TryTypeArguments()
        {
            if (Is(TK.Lt))
            {
                Begin(TK.Lt);
                var types = Collect(Type, TK.Comma);
                if (Is(TK.Gt))
                {
                    Match(TK.Gt);

                    return new TypeList(End(), types);
                }
                End();
            }
            return null;
        }

        private ThisType ThisType()
        {
            Begin(TK.This);
            return new ThisType(End());
        }

        private IType TupleType()
        {
            Begin();
            Match(TK.LParen);
            var types = Collect(InfixType, TK.Comma);
            Match(TK.RParen);
            if (types.Count == 1)
            {
                End();
                return types[0];
            }
            return new TupleType(End(), types);
        }

        private IType InfixType()
        {
            return UnionType();
        }

        private IType UnionType()
        {
            Begin();
            var types = Collect(IntersectionType, TK.Pipe);
            if (types.Count == 1)
            {
                End();
                return types[0];
            }
            return new UnionType(End(), types);
        }

        private IType IntersectionType()
        {
            Begin();
            var types = Collect(Type, TK.Amper);
            if (types.Count == 1)
            {
                End();
                return types[0];
            }
            return new IntersectionType(End(), types);
        }

        private String? TryDoc()
        {
            if (Is(TK.DocString))
            {
                Begin(TK.DocString);
                return new String(End());
            }
            return null;
        }

        private QualifiedIdentifier QualifiedIdentifier()
        {
            Begin();
            var names = Collect(Identifier, TK.Dot);
            return new QualifiedIdentifier(End(), names);
        }

        private Identifier Identifier()
        {
            Begin(TK.Identifier);
            return new Identifier(End());
        }

        private void Begin()
        {
            markers.Push(next);
        }

        private void Begin(TK kind)
        {
            markers.Push(next);
            Match(kind);
        }

        private TokenSpan End()
        {
            Debug.Assert(next <= limit);
            Debug.Assert(markers.Count > 0);
            return new TokenSpan(Tokens, markers.Pop(), next);
        }

        private TokenSpan End(TK token)
        {
            Match(token);
            return End();
        }

        private TokenSpan Mark(IAny node)
        {
            Debug.Assert(next <= limit);
            Debug.Assert(markers.Count > 0);
            return new TokenSpan(Tokens, node.Span.Start, next);
        }

        private bool Is(TK token)
        {
            return next < limit && Tokens[next].Kind == token;
        }

        private void Match(TK kind)
        {
            if (next < limit && kind == TK.Gt && Current.Kind == TK.Do)
            {
                Debug.Assert(true);
            }
            if (next < limit && Tokens[next].Kind == kind)
            {
                next += 1;
                return;
            }

            Errors.AtToken(ErrNo.Scan001, Current, $"unknown token in token stream, expected ``{Keywords.String(kind)}´´ but got ``{Keywords.String(Current.Kind)}´´");

            // simply try to skip
            while (next < limit && Tokens[next].Kind != kind)
            {
                next += 1;
            }
            if (next < limit)
            {
                next += 1;
            }
        }

        private bool MayMatch(TK kind)
        {
            if (next < limit && Tokens[next].Kind == kind)
            {
                next += 1;
                return true;
            }

            return false;
        }

        private IReadOnlyList<T> Collect<T>(Func<T> collect, TK token)
        {
            var list = new List<T>();

            do
            {
                list.Add(collect());
            }
            while (MayMatch(token));

            return list;
        }

        private IReadOnlyList<T> Collect<T>(T first, Func<T?> collect) where T : class
        {
            var list = new List<T>() { first };

            while (true)
            {
                var item = collect();
                if (item != null)
                {
                    list.Add(item);
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        private IReadOnlyList<T> Collect<T>(Func<T?> collect) where T : class
        {
            var list = new List<T>();

            while (true)
            {
                var item = collect();
                if (item != null)
                {
                    list.Add(item);
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        private IReadOnlyList<T> CollectOptional<T>(Func<T?> collect, TK token) where T : class
        {
            var list = new List<T>();

            do
            {
                var item = collect();
                if (item != null)
                {
                    list.Add(item);
                }
                else
                {
                    break;
                }
            }
            while (MayMatch(token));

            return list;
        }
    }
}
