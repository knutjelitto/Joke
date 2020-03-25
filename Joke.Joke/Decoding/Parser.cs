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
        public bool CurrentIsDoc => Current.Kind == TK.String;

        private readonly Stack<int> markers = new Stack<int>();

        private int next;
        private readonly int limit;

        public Unit ParseUnit()
        {
            next = 0;

            var unit = Unit();

            if (!Is(TK.Eof))
            {
                Errors.AtToken(ErrNo.Scan002, Current, "inconclusive parse, not at ``EOF´´");
            }

            return unit;
        }

        private Unit Unit()
        {
            Begin();
            String? packageDoc = null;
            if (Current.Kind == TK.String && Next.Kind == TK.String)
            {
                packageDoc = String();
            }
            var members = UnitMembers();
            return new Unit(End(), packageDoc, members);
        }

        private MemberList UnitMembers()
        {
            Begin();
            var items = Collect(TryUnitMember);
            return new MemberList(End(), items);
        }

        private INamedMember? TryUnitMember()
        {
            return TryClassType();
        }

        private INamedMember? TryClassType()
        {
            switch (Current.Kind)
            {
                case TK.Type:
                    return ClassType(Current.Kind, ClassKind.Alias);
                case TK.Primitive:
                    return ClassType(Current.Kind, ClassKind.Primitive);
                case TK.Interface:
                    return ClassType(Current.Kind, ClassKind.Interface);
                case TK.Struct:
                    return ClassType(Current.Kind, ClassKind.Struct);
                case TK.Class:
                    return ClassType(Current.Kind, ClassKind.Class);
                case TK.Trait:
                    return ClassType(Current.Kind, ClassKind.Trait);
                case TK.Actor:
                    return ClassType(Current.Kind, ClassKind.Actor);
                case TK.Extern:
                    return ClassType(Current.Kind, ClassKind.Extern);
                default:
                    return null;
            }
        }

        private Class ClassType(TK token, ClassKind kind)
        {
            Begin();
            Match(token);
            var name = Identifier();
            var typeparameters = TryTypeParameters();
            var provides = TryProvides();
            var doc = TryString();
            var members = ClassMembers();


            return new Class(End(), kind, name, typeparameters, provides, doc, members);
        }

        private MemberList ClassMembers()
        {
            Begin();
            var members = Collect(TryClassMember);
            return new MemberList(End(), members);
        }

        private INamedMember? TryClassMember()
        {
            return TryField() ?? TryMethod();
        }

        private INamedMember? TryField()
        {
            return Current.Kind switch
            {
                TK.Let => Field(Current.Kind, FieldKind.Let),
                TK.Var => Field(Current.Kind, FieldKind.Var),
                TK.Embed => Field(Current.Kind, FieldKind.Embed),
                _ => null,
            };
        }
        private INamedMember Field(TK token, FieldKind kind)
        {
            Begin();
            Match(token);
            var name = Identifier();
            var type = TypeAnnotation();
            var init = TryInitInfix();
            var doc = TryString();

            return new Field(End(), kind, name, type, init, doc);
        }

        private INamedMember? TryMethod()
        {
            return Current.Kind switch
            {
                TK.Fun => Method(Current.Kind, MethodKind.Fun),
                TK.New => Method(Current.Kind, MethodKind.New),
                TK.Be => Method(Current.Kind, MethodKind.Be),
                _ => null,
            };
        }

        private INamedMember Method(TK token, MethodKind kind)
        {
            Begin();
            Match(token);
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = ValueParameters();
            var @return = TryTypeAnnotation();
            var throws = TryThrows();
            var doc = TryString();
            var body = TryBody();

            return new Method(End(), kind, name, typeParameters, parameters, @return, throws, doc, body);
        }

        private Throws? TryThrows()
        {
            if (IsBeginMatch(TK.Exclamation))
            {
                return new Throws(End());
            }
            return null;
        }

        private IExpression? TryInitInfix()
        {
            if (IsMatch(TK.Assign))
            {
                return Infix();
            }

            return null;
        }

        private Body? TryBody()
        {
            if (IsBeginMatch(TK.DblArrow))
            {
                var expression = Expression();
                return new Body(End(), expression);
            }
            return null;
        }

        private Body Body()
        {
            BeginMatch(TK.DblArrow);
            var expression = Expression();
            return new Body(End(), expression);
        }

        private ParameterList ValueParameters()
        {
            BeginMatch(TK.LParen);
            var items = CollectOptional(TryParameter, TK.Comma);
            Match(TK.RParen);
            return new ParameterList(End(), items);
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
            if (IsMatch(TK.Assign))
            {
                return Expression();
            }
            return null;
        }

        private TypeParameterList? TryTypeParameters()
        {
            if (IsBeginMatch(TK.Lt))
            {
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
            if (IsMatch(TK.Colon))
            {
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
            if (IsMatch(TK.Assign))
            {
                return Type();
            }
            return null;
        }

        private IType Provides()
        {
            return TryProvides() ?? throw Expected("provides");
        }

        private IType? TryProvides()
        {
            if (IsMatch(TK.Is))
            {
                return Type();
            }
            return null;
        }

        private IType Type()
        {
            return TryType() ?? throw Expected("type");
        }

        private IType? TryType()
        {
            return TryAtomType();
        }

        private IType? TryAtomType()
        {
            return Current.Kind switch
            {
                TK.This => ThisType(),
                TK.LParen => TryTupleType(),
                TK.Identifier => NominalType(),
                TK.LBrace => LambdaType(),
                _ => null,
            };
        }

        private LambdaType LambdaType()
        {
            BeginMatch(TK.LBrace);
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaTypeParameters();
            var result = TryTypeAnnotation();
            var throws = TryThrows();
            Match(TK.RBrace);

            return new LambdaType(End(), name, typeParameters, parameters, result, throws);
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
            return TryTypeArguments() ?? throw Expected("type-arguments");
        }

        private TypeList? TryTypeArguments()
        {
            if (IsBeginMatch(TK.Lt))
            {
                var types = CollectOptional(TryType, TK.Comma);
                if (types.Count > 0 && IsMatch(TK.Gt))
                {
                    return new TypeList(End(), types);
                }
                // recover to starting '<'
                Inconclusive();
            }
            return null;
        }

        private TypeList LambdaTypeParameters()
        {
            BeginMatch(TK.LParen);
            var types = CollectOptional(TryType, TK.Comma);
            Match(TK.RParen);
            return new TypeList(End(), types);
        }

        private ThisType ThisType()
        {
            BeginMatch(TK.This);
            return new ThisType(End());
        }

        private IType? TryTupleType()
        {
            if (IsBeginMatch(TK.LParen))
            {
                var types = Collect(InfixType, TK.Comma);
                if (IsMatch(TK.RParen))
                {
                    return Singularize(types) ?? new TupleType(End(), types);
                }
                Inconclusive();
            }

            return null;
        }

        private IType TupleType()
        {
            BeginMatch(TK.LParen);
            var types = Collect(InfixType, TK.Comma);
            Match(TK.RParen);
            return Singularize(types) ?? new TupleType(End(), types);
        }

        private IType InfixType()
        {
            return UnionType();
        }

        private IType UnionType()
        {
            Begin();
            var types = Collect(IntersectionType, TK.Pipe);
            return Singularize(types) ?? new UnionType(End(), types);
        }

        private IType IntersectionType()
        {
            Begin();
            var types = Collect(Type, TK.Amper);
            return Singularize(types) ?? new IntersectionType(End(), types);
        }

        private String? TryDoc()
        {
            if (IsBeginMatch(TK.String))
            {
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
            BeginMatch(TK.Identifier);
            return new Identifier(End());
        }

        private Identifier? TryIdentifier()
        {
            if (IsBeginMatch(TK.Identifier))
            {
                return new Identifier(End());
            }
            return null;
        }
    }
}
