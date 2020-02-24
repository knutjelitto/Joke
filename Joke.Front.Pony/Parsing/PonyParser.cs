using System.Collections.Generic;

using Joke.Front.Err;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Syntax
{
    public partial class PonyParser
    {
        private enum NL
        {
            Both,
            Next,
            Case
        }

        private readonly Stack<int> marks = new Stack<int>();

        private int next;
        private readonly int limit;

        public ErrorAccu Errors { get; }
        public ISource Source { get; }
        public PonyTokens Tokens { get; }

        public PonyParser(ErrorAccu errors, ISource source, PonyTokens tokens)
        {
            Errors = errors;
            Source = source;
            Tokens = tokens;

            next = 0;
            limit = Tokens.Count;
        }

        public Ast.File Module()
        {
            Begin();
            var doc = TryString();
            var uses = CollectRecover(First.RecoverInModule, TryUse);
            var classes = CollectRecover(First.RecoverInModule, TryClass);
            var module = new Ast.File(End(), doc, uses, classes);
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
                    var guard = TryUseGuard();

                    var use = new Ast.UseFfi(End(), name, ffiName, returnType, parameters, partial, guard);

                    return use;
                }
                else if (Iss(TK.String))
                {
                    var uri = String();
                    var guard = TryUseGuard();
                    return new Ast.UseUri(End(), name, uri, guard);
                }
            }

            return null;
        }

        private Ast.Guard? TryUseGuard()
        {
            if (MayBegin(TK.If))
            {
                var expression = Infix();
                return new Ast.Guard(End(), expression);
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
            return TokenKind switch
            {
                TK.Class => Class(Ast.ClassKind.Class),
                TK.Type => Class(Ast.ClassKind.Type),
                TK.Interface => Class(Ast.ClassKind.Interface),
                TK.Trait => Class(Ast.ClassKind.Trait),
                TK.Primitive => Class(Ast.ClassKind.Primitive),
                TK.Struct => Class(Ast.ClassKind.Struct),
                TK.Actor => Class(Ast.ClassKind.Actor),
                _ => null,
            };
        }

        public Ast.Class Class(Ast.ClassKind kind)
        {
            Begin(First.Class);
            var annotations = TryAnnotations();
            var bare = MayMatch(TK.At);
            var cap = TryCap();
            var name = Identifier();
            var typeParams = TryTypeParameters();
            var provides = TryProvides();
            var doc = TryString();
            var members = Members();
            var result = new Ast.Class(End(), kind, annotations, bare, cap, name, typeParams, provides, doc, members);
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
            var fields = CollectRecover(First.RecoverInClass, TryField);
            return new Ast.Fields(End(), fields);
        }

        private Ast.Field? TryField()
        {
            return TokenKind switch
            {
                TK.Var => Field(Ast.FieldKind.Var),
                TK.Let => Field(Ast.FieldKind.Let),
                TK.Embed => Field(Ast.FieldKind.Embed),
                _ => null,
            };
        }

        private Ast.Field Field(Ast.FieldKind kind)
        {
            Begin(First.Field);
            var name = Identifier();
            var type = ColonType();
            var value = TryAssignInfix();
            var doc = TryString();
            return new Ast.Field(End(), kind, name, type, value, doc);
        }

        private Ast.Methods Methods()
        {
            Begin();
            var methods = CollectRecover(First.RecoverInClass, TryMethod);
            return new Ast.Methods(End(), methods);
        }

        private Ast.Method? TryMethod()
        {
            return TokenKind switch
            {
                TK.Fun => Method(Ast.MethodKind.Fun),
                TK.Be => Method(Ast.MethodKind.Be),
                TK.New => Method(Ast.MethodKind.New),
                _ => null,
            };
        }

        private Ast.Method Method(Ast.MethodKind kind)
        {
            Begin(First.Method);
            var annotations = TryAnnotations();
            var bare = MayMatch(TK.At);
            var cap = TryCap();
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
                return RawSequence();
            }

            return null;
        }

        private Ast.Parameters Parameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = List(Parameter, TK.RParen);
            return new Ast.Parameters(End(TK.RParen), parameters);
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
            var value = TryAssignInfix();

            return new Ast.RegularParameter(End(), name, type, value);
        }

        private Ast.Expression? TryAssignInfix()
        {
            return MayMatch(TK.Assign) ? Infix() : null;
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

                return new Ast.TypeParameters(End(TK.RSquare), parameters);
            }

            return null;
        }

        private Ast.Cap? TryCap()
        {
            return TokenKind switch
            {
                TK.Iso => Cap(Ast.CapKind.Iso),
                TK.Trn => Cap(Ast.CapKind.Trn),
                TK.Ref => Cap(Ast.CapKind.Ref),
                TK.Val => Cap(Ast.CapKind.Val),
                TK.Box => Cap(Ast.CapKind.Box),
                TK.Tag => Cap(Ast.CapKind.Tag),
                _ => null,
            };
        }

        private Ast.Cap? TryCapEx()
        {
            return TokenKind switch
            {
                TK.Iso => Cap(Ast.CapKind.Iso),
                TK.Trn => Cap(Ast.CapKind.Trn),
                TK.Ref => Cap(Ast.CapKind.Ref),
                TK.Val => Cap(Ast.CapKind.Val),
                TK.Box => Cap(Ast.CapKind.Box),
                TK.Tag => Cap(Ast.CapKind.Tag),
                TK.CapRead => Cap(Ast.CapKind.HashRead),
                TK.CapSend => Cap(Ast.CapKind.HashSend),
                TK.CapShare => Cap(Ast.CapKind.HashShare),
                TK.CapAlias => Cap(Ast.CapKind.HashAlias),
                TK.CapAny => Cap(Ast.CapKind.HashAny),
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

                return new Ast.Annotations(End(TK.Backslash), names);
            }

            return null;
        }
    }
}
