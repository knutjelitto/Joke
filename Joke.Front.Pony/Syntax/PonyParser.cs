using Joke.Front.Pony.Lex;
using System.Collections.Generic;
using System.Diagnostics;

namespace Joke.Front.Pony.Syntax
{
    public partial class PonyParser
    {
        public PonyParser(Source source, IReadOnlyList<Token> tokens)
        {
            Source = source;
            Tokens = tokens;
            next = 0;
            limit = Tokens.Count;
        }

        public Ast.Module Module()
        {
            Debug.Assert(marks.Count == 0);

            Begin();

            Debug.Assert(marks.Count == 1);
            var doc = TryString();
            Debug.Assert(marks.Count == 1);
            var uses = Collect(TryUse);
            Debug.Assert(marks.Count == 1);
            var classes = Collect(TryClass);
            Debug.Assert(marks.Count == 1);

            var module = new Ast.Module(End(), doc, uses, classes);

            Debug.Assert(marks.Count == 0);

            return module;
        }

        public Ast.Use? TryUse()
        {
            if (MayBegin(TK.Use))
            {
                var name = TryUseName();

                if (Iss(TK.At))
                {
                    Match(TK.At);
                    var ffiName = FfiName();
                    var returnType = TypeArguments();
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
            }

            return null;
        }

        public Ast.Identifier? TryUseName()
        {
            var name = TryIdentifier();
            if (name != null)
            {
                Match(TK.Assign);
            }
            return name;
        }

        public Ast.Class? TryClass()
        {
            Ast.Class? result = null;

            Debug.Assert(marks.Count == 1);

            switch (TokenKind)
            {
                case TK.Class:
                    result = Class(Ast.ClassKind.Class);
                    Debug.Assert(marks.Count == 1);
                    break;
                case TK.Type:
                    result = Class(Ast.ClassKind.Type);
                    Debug.Assert(marks.Count == 1);
                    break;
                case TK.Interface:
                    result = Class(Ast.ClassKind.Interface);
                    Debug.Assert(marks.Count == 1);
                    break;
                case TK.Trait:
                    result = Class(Ast.ClassKind.Trait);
                    Debug.Assert(marks.Count == 1);
                    break;
                case TK.Primitive:
                    result = Class(Ast.ClassKind.Primitive);
                    Debug.Assert(marks.Count == 1);
                    break;
                case TK.Struct:
                    result = Class(Ast.ClassKind.Struct);
                    Debug.Assert(marks.Count == 1);
                    break;
                case TK.Actor:
                    result = Class(Ast.ClassKind.Actor);
                    Debug.Assert(marks.Count == 1);
                    break;
            }

            return result;
        }

        public Ast.Class Class(Ast.ClassKind kind)
        {
            Debug.Assert(marks.Count == 1);
            Begin(First.Class);
            Debug.Assert(marks.Count == 2);
            var annotations = TryAnnotations();
            Debug.Assert(marks.Count == 2);
            var bare = MayMatch(TK.At);
            Debug.Assert(marks.Count == 2);
            var cap = TryCap(false);
            Debug.Assert(marks.Count == 2);
            var name = Identifier();
            Debug.Assert(marks.Count == 2);
            var typeParams = TryTypeParameters();
            Debug.Assert(marks.Count == 2);
            var provides = TryProvides();
            Debug.Assert(marks.Count == 2);
            var doc = TryString();
            Debug.Assert(marks.Count == 2);
            var members = Members();
            Debug.Assert(marks.Count == 2);
            var result = new Ast.Class(End(), kind, annotations, bare, cap, name, typeParams, provides, doc, members);
            Debug.Assert(marks.Count == 1);
            return result;
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

        private Ast.Expression? TryBody()
        {
            if (MayMatch(TK.DblArrow))
            {
                return RawSeq();
            }

            return null;
        }

        private Ast.Parameters Parameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = List(Parameter, TK.RParen);
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

        private Ast.Type? TryDefaultType()
        {
            if (MayMatch(TK.Assign))
            {
                return TypeArgument();
            }
            return null;
        }

        private Ast.Type ColonType()
        {
            Match(TK.Colon);
            return Type();
        }

        private Ast.Type? TryColonType()
        {
            if (MayMatch(TK.Colon))
            {
                return Type();
            }
            return null;
        }

        private Ast.Type? TryProvides()
        {
            if (MayMatch(TK.Is))
            {
                return Type();
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
                var parameters = List(TypeParameter);
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
                var names = List(Identifier);
                Match(TK.Backslash);
                return new Ast.Annotations(End(), names);
            }

            return null;
        }
    }
}
