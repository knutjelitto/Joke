using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony.Syntax
{
    public partial class PonyParser
    {
        public PonyParser(ISource source, IReadOnlyList<Token> tokens)
        {
            Source = source;
            Tokens = tokens;
            next = 0;
            limit = Tokens.Count;
        }


        private static readonly TK[] FirstClass = new TK[]
        {
            TK.Type, TK.Interface, TK.Trait, TK.Primitive, TK.Struct, TK.Class, TK.Actor
        };

        private static readonly TK[] FirstParameter = new TK[]
        {
            TK.Identifier, TK.Ellipsis
        };

        public Ast.Module Module()
        {
            Begin();

            var doc = TryString();

            var uses = new List<Ast.Use>();

            while (Iss(TK.Use))
            {
                uses.Add(Use());
            }

            var classes = new List<Ast.Class>();

            var done = false;
            while (!done)
            {
                switch (TokenKind)
                {
                    case TK.Type:
                        classes.Add(Class(Ast.ClassKind.Type));
                        break;
                    case TK.Interface:
                        classes.Add(Class(Ast.ClassKind.Interface));
                        break;
                    case TK.Trait:
                        classes.Add(Class(Ast.ClassKind.Trait));
                        break;
                    case TK.Primitive:
                        classes.Add(Class(Ast.ClassKind.Primitive));
                        break;
                    case TK.Struct:
                        classes.Add(Class(Ast.ClassKind.Struct));
                        break;
                    case TK.Actor:
                        classes.Add(Class(Ast.ClassKind.Actor));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return new Ast.Module(End(), doc, uses, classes);
        }

        public Ast.Use Use()
        {
            Begin(TK.Use);

            var name = TryUseName();

            if (Iss(TK.At))
            {
                Match(TK.At);
                var ffiName = FfiName();
                var returnType = TryTypeArguments() ?? throw NoParse("ffi return type");
                var parameters = Parameters();
                var partial = MayPartial();

                var use = new Ast.UseFfi(End(), name, ffiName, returnType, parameters, partial);

                return use;
            }
            else if (Iss(TK.String))
            {
                var uri = String();
                return new Ast.UseUri(End(), name, uri);
            }

            throw NoParse("use");
        }

        public Ast.Identifier? TryUseName()
        {
            Begin();

            var name = TryIdentifier();
            if (name != null)
            {
                Match(TK.Assign);
                return new Ast.UseName(End(), name);
            }

            Discard();
            return null;
        }

        public Ast.Class Class(Ast.ClassKind kind)
        {
            Begin(FirstClass);

            var annotations = TryAnnotations();
            var bare = MayMatch(TK.At);
            var cap = TryCap(false);
            var name = Identifier();
            var typeParams = TryTypeParameters();
            var provides = TryProvides();
            var doc = TryString();
            var members = Members();

            return new Ast.Class(End(), kind, annotations, bare, cap, name, typeParams, provides, doc, members);
        }

        private Ast.Members Members()
        {
            Begin();

            var fields = Fields();
            var methods = Methods();

            return new Ast.Members(End(), fields, methods);
        }

        private Ast.Fields Fields()
        {
            Begin();

            var fields = new List<Ast.Field>();
            var done = false;
            while (!done)
            {
                switch (TokenKind)
                {
                    case TK.Var:
                        fields.Add(Field(Ast.FieldKind.Var));
                        break;
                    case TK.Let:
                        fields.Add(Field(Ast.FieldKind.Let));
                        break;
                    case TK.Embed:
                        fields.Add(Field(Ast.FieldKind.Embed));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return new Ast.Fields(End(), fields);
        }

        private Ast.Field Field(Ast.FieldKind kind)
        {
            Begin(TK.Var, TK.Let, TK.Embed);

            var name = Identifier();
            var type = ColonType();
            var value = TryDefaultInfixArg();
            var doc = TryString();

            return new Ast.Field(End(), kind, name, type, value, doc);
        }

        private Ast.Methods Methods()
        {
            Begin();

            var methods = new List<Ast.Method>();
            var done = false;
            while (!done)
            {
                switch(TokenKind)
                {
                    case TK.Fun:
                        methods.Add(Method(Ast.MethodKind.Fun));
                        break;
                    case TK.Be:
                        methods.Add(Method(Ast.MethodKind.Be));
                        break;
                    case TK.New:
                        methods.Add(Method(Ast.MethodKind.New));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return new Ast.Methods(End(), methods);
        }

        private Ast.Method Method(Ast.MethodKind kind)
        {
            Begin(TK.Fun, TK.Be, TK.New);

            var annotations = TryAnnotations();
            var bare = MayMatch(TK.At);
            var cap = TryCap(false);
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = Parameters();
            var returnType = TryColonType();
            var partial = MayPartial();
            var doc = TryString();
            var body = TryBody();

            return new Ast.Method(End(), kind, annotations, bare, cap, name, typeParameters, parameters, returnType, partial, doc, body);
        }

        private Ast.Body? TryBody()
        {
            if (Iss(TK.DblArrow))
            {
                Begin(TK.DblArrow);
                var body = RawSeq();
                return new Ast.Body(End(), body);
            }

            return null;
        }

        private Ast.Parameters Parameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = PlusList(Parameter, TK.RParen);
            Match(TK.RParen);

            return new Ast.Parameters(End(), parameters);
        }

        private Ast.Parameter Parameter()
        {
            Begin();

            if (MayMatch(TK.Ellipsis))
            {
                return new Ast.EllipsisParameter(End());
            }

            var name = Identifier();
            var type = ColonType();
            var value = TryDefaultInfixArg();

            return new Ast.RegularParameter(End(), name, type, value);
        }

        private Ast.DefaultArg? TryDefaultInfixArg()
        {
            if (MayBegin(TK.Assign))
            {
                var expression = Infix();
                return new Ast.DefaultArg(End(), expression);
            }

            return null;
        }

        private Ast.DefaultType? TryDefaultType()
        {
            if (MayBegin(TK.Assign))
            {
                var type = TypeArgument();
                return new Ast.DefaultType(End(), type);
            }
            return null;
        }

        private Ast.ColonType ColonType()
        {
            Begin(TK.Colon);
            var type = Type();
            return new Ast.ColonType(End(), type);
        }

        private Ast.ColonType? TryColonType()
        {
            if (Iss(TK.Colon))
            {
                return ColonType();
            }
            return null;
        }

        private Ast.Type? TryProvides()
        {
            if (MayBegin(TK.Is))
            {
                var type = Type();
                return new Ast.Provides(End(), type);
            }

            return null;
        }

        private Ast.TypeParameter TypeParameter()
        {
            Begin();

            var name = Identifier();
            var type = TryColonType();
            var defaultType = TryDefaultType();

            return new Ast.TypeParameter(End(), name, type, defaultType);
        }

        private Ast.TypeParameters? TryTypeParameters()
        {
            if (MayBegin(TK.LSquare, TK.LSquareNew))
            {
                var parameters = PlusList(TypeParameter);
                Match(TK.RSquare);
                return new Ast.TypeParameters(End(), parameters);
            }

            return null;
        }

        private Ast.Cap? TryCap(bool extended)
        {
            return TokenKind switch
            {
                TK.Iso => Cap(Ast.CapKind.Iso),
                TK.Trn => Cap(Ast.CapKind.Trn),
                TK.Ref => Cap(Ast.CapKind.Ref),
                TK.Val => Cap(Ast.CapKind.Val),
                TK.Box => Cap(Ast.CapKind.Box),
                TK.Tag => Cap(Ast.CapKind.Tag),
                TK.CapRead when extended => Cap(Ast.CapKind.HashRead),
                TK.CapSend when extended => Cap(Ast.CapKind.HashSend),
                TK.CapShare when extended => Cap(Ast.CapKind.HashShare),
                TK.CapAlias when extended => Cap(Ast.CapKind.HashAlias),
                TK.CapAny when extended => Cap(Ast.CapKind.HashAny),
                _ => null,
            };
        }

        private Ast.Cap Cap(Ast.CapKind kind)
        {
            Begin(TokenKind);
            return new Ast.Cap(End(), kind);
        }

        public Ast.Annotations? TryAnnotations()
        {
            if (MayBegin(TK.Backslash))
            {
                var names = PlusList(Identifier);
                Match(TK.Backslash);
                return new Ast.Annotations(End(), names);
            }

            return null;
        }
    }
}
