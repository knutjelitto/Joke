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
            var members = UnitMembers();
            return new Unit(End(), members);
        }

        private MemberList UnitMembers()
        {
            Begin();
            var items = Collect(TryUnitMember);
            return new MemberList(End(), items);
        }

        private IMember? TryUnitMember()
        {
            return TryClassType() ?? TryExtern();
        }

        private IMember? TryExtern()
        {
            if (IsBeginMatch(TK.Extern))
            {
                var name = Identifier();
                var funs = Collect(TryMethod);
                Match(TK.End);
                return new Extern(End(), funs);
            }

            return null;
        }

        private IMember? TryClassType()
        {
            var token = CurrentIsDoc ? Next.Kind : Current.Kind;
            switch (token)
            {
                case TK.Type:
                    return ClassType(token, ClassKind.Alias);
                case TK.Primitive:
                    return ClassType(token, ClassKind.Primitive);
                case TK.Interface:
                    return ClassType(token, ClassKind.Interface);
                case TK.Struct:
                    return ClassType(token, ClassKind.Struct);
                case TK.Class:
                    return ClassType(token, ClassKind.Class);
                case TK.Trait:
                    return ClassType(token, ClassKind.Trait);
                case TK.Actor:
                    return ClassType(token, ClassKind.Actor);
                default:
                    return null;
            }
        }

        private ClassType ClassType(TK token, ClassKind kind)
        {
            Begin();
            var doc = TryAnyString();
            Match(token);
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
            var token = CurrentIsDoc ? Next.Kind : Current.Kind;
            switch (token)
            {
                case TK.Let:
                    return Field(token, FieldKind.Let);
                case TK.Var:
                    return Field(token, FieldKind.Var);
                case TK.Embed:
                    return Field(token, FieldKind.Embed);
                default:
                    return null;
            }
        }
        private IMember Field(TK token, FieldKind kind)
        {
            Begin();
            var doc = TryAnyString();
            Match(token);
            var name = Identifier();
            var type = TypeAnnotation();
            var init = TryInitInfix();

            return new Field(End(), kind, doc, name, type, init);
        }

        private IMember? TryMethod()
        {
            var token = CurrentIsDoc ? Next.Kind : Current.Kind;
            switch (token)
            {
                case TK.Fun:
                    return Method(token, MethodKind.Fun);
                case TK.New:
                    return Method(token, MethodKind.New);
                case TK.Be:
                    return Method(token, MethodKind.Be);
                default:
                    return null;
            }
        }

        private IMember Method(TK token, MethodKind kind)
        {
            Begin();
            var doc = TryAnyString();
            Match(token);
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = ValueParameters();
            var @return = TryTypeAnnotation();
            var throws = TryThrows();
            var body = TryBody();

            return new Method(End(), kind, doc, name, typeParameters, parameters, @return, throws, body);
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
            if (IsBeginMatch(TK.DocString))
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
