using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony
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

        public Tree.Module Module()
        {
            Begin();

            var doc = TryString();

            var uses = new List<Tree.Use>();

            while (Iss(TK.Use))
            {
                uses.Add(Use());
            }

            var classes = new List<Tree.Class>();

            var done = false;
            while (!done)
            {
                switch (TokenKind)
                {
                    case TK.Type:
                        classes.Add(Class(Tree.ClassKind.Type));
                        break;
                    case TK.Interface:
                        classes.Add(Class(Tree.ClassKind.Interface));
                        break;
                    case TK.Trait:
                        classes.Add(Class(Tree.ClassKind.Trait));
                        break;
                    case TK.Primitive:
                        classes.Add(Class(Tree.ClassKind.Primitive));
                        break;
                    case TK.Struct:
                        classes.Add(Class(Tree.ClassKind.Struct));
                        break;
                    case TK.Actor:
                        classes.Add(Class(Tree.ClassKind.Actor));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return new Tree.Module(End(), doc, uses, classes);
        }

        public Tree.Use Use()
        {
            Begin(TK.Use);

            var name = TryUseName();

            if (Iss(TK.At))
            {
                Match(TK.At);
                var ffiName = FfiName();
                var returnType = TryTypeArguments() ?? throw NoParse("ffi return type");
                var parameters = Parameters();
                var partial = TryPartial();

                return new Tree.UseFfi(End(), name, ffiName, returnType, parameters, partial);
            }
            else if (Iss(TK.String))
            {
                var uri = String();
                return new Tree.UseUri(End(), name, uri);
            }

            throw NoParse("use");
        }

        public Tree.Identifier? TryUseName()
        {
            Begin();

            var name = TryIdentifier();
            if (name != null)
            {
                Match(TK.Assign);
                return new Tree.UseName(End(), name);
            }

            Discard();
            return null;
        }

        public Tree.Class Class(Tree.ClassKind kind)
        {
            Begin(FirstClass);

            var annotations = TryAnnotations();
            var bare = TryBare();
            var cap = TryCap(false);
            var name = Identifier();
            var typeParams = TryTypeParameters();
            var provides = TryProvides();
            var doc = TryString();
            var members = Members();

            return new Tree.Class(End(), kind, annotations, bare, cap, name, typeParams, provides, doc, members);
        }

        private Tree.Members Members()
        {
            Begin();

            var fields = Fields();
            var methods = Methods();

            return new Tree.Members(End(), fields, methods);
        }

        private Tree.Fields Fields()
        {
            Begin();

            var fields = new List<Tree.Field>();
            var done = false;
            while (!done)
            {
                switch (TokenKind)
                {
                    case TK.Var:
                        fields.Add(Field(Tree.FieldKind.Var));
                        break;
                    case TK.Let:
                        fields.Add(Field(Tree.FieldKind.Let));
                        break;
                    case TK.Embed:
                        fields.Add(Field(Tree.FieldKind.Embed));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return new Tree.Fields(End(), fields);
        }

        private Tree.Field Field(Tree.FieldKind kind)
        {
            Begin(TK.Var, TK.Let, TK.Embed);

            var name = Identifier();
            var type = ColonType();
            var value = TryDefaultInfixArg();
            var doc = TryString();

            return new Tree.Field(End(), kind, name, type, value, doc);
        }

        private Tree.Methods Methods()
        {
            Begin();

            var methods = new List<Tree.Method>();
            var done = false;
            while (!done)
            {
                switch(TokenKind)
                {
                    case TK.Fun:
                        methods.Add(Method(Tree.MethodKind.Fun));
                        break;
                    case TK.Be:
                        methods.Add(Method(Tree.MethodKind.Be));
                        break;
                    case TK.New:
                        methods.Add(Method(Tree.MethodKind.New));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            return new Tree.Methods(End(), methods);
        }

        private Tree.Method Method(Tree.MethodKind kind)
        {
            Begin(TK.Fun, TK.Be, TK.New);

            var annotations = TryAnnotations();
            var bare = TryBare();
            var cap = TryCap(false);
            var name = Identifier();
            var typeParameters = TryTypeParameters();
            var parameters = Parameters();
            var returnType = TryColonType();
            var partial = TryPartial();
            var doc = TryString();
            var body = TryBody();

            return new Tree.Method(End(), kind, annotations, bare, cap, name, typeParameters, parameters, returnType, partial, doc, body);
        }

        private Tree.Body? TryBody()
        {
            if (Iss(TK.DblArrow))
            {
                Begin(TK.DblArrow);
                var body = RawSeq();
                return new Tree.Body(End(), body);
            }

            return null;
        }

        private Tree.Parameters Parameters()
        {
            Begin(TK.LParen, TK.LParenNew);

            var parameters = new List<Tree.Parameter>();

            if (Iss(FirstParameter))
            {
                do
                {
                    parameters.Add(Parameter());
                }
                while (MayMatch(TK.Comma));
            }
            Match(TK.RParen);

            return new Tree.Parameters(End(), parameters);
        }

        private Tree.Parameter Parameter()
        {
            Begin();

            if (Iss(TK.Ellipsis))
            {
                Match(TK.Ellipsis);
                return new Tree.EllipsisParameter(End());
            }

            var name = Identifier();
            var type = ColonType();
            var value = TryDefaultInfixArg();

            return new Tree.RegularParameter(End(), name, type, value);
        }

        private Tree.DefaultArg? TryDefaultInfixArg()
        {
            if (MayBegin(TK.Assign))
            {
                var expression = Infix();
                return new Tree.DefaultArg(End(), expression);
            }

            return null;
        }

        private Tree.DefaultType? TryDefaultType()
        {
            if (MayBegin(TK.Assign))
            {
                var type = TypeArgument();
                return new Tree.DefaultType(End(), type);
            }
            return null;
        }

        private Tree.ColonType ColonType()
        {
            Begin(TK.Colon);
            var type = Type();
            return new Tree.ColonType(End(), type);
        }

        private Tree.ColonType? TryColonType()
        {
            if (Iss(TK.Colon))
            {
                return ColonType();
            }
            return null;
        }

        private Tree.Bare? TryBare()
        {
            if (MayBegin(TK.At))
            {
                return new Tree.Bare(End());
            }

            return null;
        }

        private Tree.Type? TryProvides()
        {
            if (MayBegin(TK.Is))
            {
                var type = Type();
                return new Tree.Provides(End(), type);
            }

            return null;
        }

        private Tree.TypeParameter TypeParameter()
        {
            Begin();

            var name = Identifier();
            var type = TryColonType();
            var defaultType = TryDefaultType();

            return new Tree.TypeParameter(End(), name, type, defaultType);
        }

        private Tree.TypeParameters? TryTypeParameters()
        {
            if (MayBegin(TK.LSquare, TK.LSquareNew))
            {
                var parameters = new List<Tree.TypeParameter>();
                do
                {
                    parameters.Add(TypeParameter());
                }
                while (MayMatch(TK.Comma));

                Match(TK.RSquare);
                return new Tree.TypeParameters(End(), parameters);
            }

            return null;
        }

        private Tree.Cap? TryCap(bool extended)
        {
            switch (TokenKind)
            {
                case TK.Iso:
                    return Cap(Tree.CapKind.Iso);
                case TK.Trn:
                    return Cap(Tree.CapKind.Trn);
                case TK.Ref:
                    return Cap(Tree.CapKind.Ref);
                case TK.Val:
                    return Cap(Tree.CapKind.Val);
                case TK.Box:
                    return Cap(Tree.CapKind.Box);
                case TK.Tag:
                    return Cap(Tree.CapKind.Tag);
                case TK.CapRead when extended:
                    return Cap(Tree.CapKind.HashRead);
                case TK.CapSend when extended:
                    return Cap(Tree.CapKind.HashSend);
                case TK.CapShare when extended:
                    return Cap(Tree.CapKind.HashShare);
                case TK.CapAlias when extended:
                    return Cap(Tree.CapKind.HashAlias);
                case TK.CapAny when extended:
                    return Cap(Tree.CapKind.HashAny);
                default:
                    return null;
            }
        }

        private Tree.Cap Cap(Tree.CapKind kind)
        {
            Begin(TokenKind);
            return new Tree.Cap(End(), kind);
        }

        public Tree.Annotations? TryAnnotations()
        {
            if (MayBegin(TK.Backslash))
            {
                var names = new List<Tree.Identifier>();
                do
                {
                    names.Add(Identifier());
                }
                while (MayMatch(TK.Comma));
                Match(TK.Backslash);
                return new Tree.Annotations(End(), names);
            }

            return null;
        }
    }
}
