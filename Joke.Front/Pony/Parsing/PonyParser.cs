using System.Collections.Generic;

using Joke.Front.Err;
using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.ParseTree;

namespace Joke.Front.Pony.Parsing
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

        public Errors Errors { get; }
        public ISource Source { get; }
        public PonyTokens Tokens { get; }

        public PonyParser(Errors errors, ISource source, PonyTokens tokens)
        {
            Errors = errors;
            Source = source;
            Tokens = tokens;

            next = 0;
            limit = Tokens.Count;
        }

        public PtUnit Unit()
        {
            Begin();
            var doc = TryString();
            var uses = CollectRecover(First.RecoverInModule, TryUse);
            var classes = CollectRecover(First.RecoverInModule, TryClass);
            var module = new PtUnit(End(), doc, uses, classes);
            return module;
        }

        private PtUse? TryUse()
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

                    var use = new PtUseFfi(End(), name, ffiName, returnType, parameters, partial, guard);

                    return use;
                }
                else if (Iss(TK.String))
                {
                    var uri = String();
                    var guard = TryUseGuard();
                    return new PtUseUri(End(), name, uri, guard);
                }
            }

            return null;
        }

        private PtGuard? TryUseGuard()
        {
            if (MayBegin(TK.If))
            {
                var expression = Infix();
                return new PtGuard(End(), expression);
            }

            return null;
        }

        private PtIdentifier? TryUseName()
        {
            var name = TryIdentifier();
            if (name != null)
            {
                Match(TK.Assign);
            }
            return name;
        }

        private PtClass? TryClass()
        {
            return TokenKind switch
            {
                TK.Class => Class(PtClassKind.Class),
                TK.Type => Class(PtClassKind.Type),
                TK.Interface => Class(PtClassKind.Interface),
                TK.Trait => Class(PtClassKind.Trait),
                TK.Primitive => Class(PtClassKind.Primitive),
                TK.Struct => Class(PtClassKind.Struct),
                TK.Actor => Class(PtClassKind.Actor),
                _ => null,
            };
        }

        private PtClass Class(PtClassKind kind)
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
            var result = new PtClass(End(), kind, annotations, bare, cap, name, typeParams, provides, doc, members);
            return result;
        }

        private PtMembers Members()
        {
            Begin();
            var fields = Fields();
            var methods = Methods();
            return new PtMembers(End(), fields, methods);
        }

        private PtFields Fields()
        {
            Begin();
            var fields = CollectRecover(First.RecoverInClass, TryField);
            return new PtFields(End(), fields);
        }

        private PtField? TryField()
        {
            return TokenKind switch
            {
                TK.Var => Field(PtFieldKind.Var),
                TK.Let => Field(PtFieldKind.Let),
                TK.Embed => Field(PtFieldKind.Embed),
                _ => null,
            };
        }

        private PtField Field(PtFieldKind kind)
        {
            Begin(First.Field);
            var name = Identifier();
            var type = ColonType();
            var value = TryAssignInfix();
            var doc = TryString();
            return new PtField(End(), kind, name, type, value, doc);
        }

        private PtMethods Methods()
        {
            Begin();
            var methods = CollectRecover(First.RecoverInClass, TryMethod);
            return new PtMethods(End(), methods);
        }

        private PtMethod? TryMethod()
        {
            return TokenKind switch
            {
                TK.Fun => Method(PtMethodKind.Fun),
                TK.Be => Method(PtMethodKind.Be),
                TK.New => Method(PtMethodKind.New),
                _ => null,
            };
        }

        private PtMethod Method(PtMethodKind kind)
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
            return new PtMethod(End(), kind, annotations, bare, cap, name, typeParameters, parameters, returnType, partial, doc, body);
        }

        private PtExpression? TryBody()
        {
            if (MayMatch(TK.DblArrow))
            {
                return RawSequence();
            }

            return null;
        }

        private PtParameters Parameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var parameters = List(Parameter, TK.RParen);
            return new PtParameters(End(TK.RParen), parameters);
        }

        private PtParameter Parameter()
        {
            Begin();

            if (MayMatch(TK.Ellipsis))
            {
                return new PtEllipsisParameter(End());
            }

            var name = Identifier();
            var type = ColonType();
            var value = TryAssignInfix();

            return new PtRegularParameter(End(), name, type, value);
        }

        private PtExpression? TryAssignInfix()
        {
            return MayMatch(TK.Assign) ? Infix() : null;
        }

        private PtType? TryDefaultType()
        {
            if (MayMatch(TK.Assign))
            {
                return TypeArgument();
            }
            return null;
        }

        private PtType ColonType()
        {
            Match(TK.Colon);
            return Type();
        }

        private PtType? TryColonType()
        {
            if (MayMatch(TK.Colon))
            {
                return Type();
            }
            return null;
        }

        private PtType? TryProvides()
        {
            if (MayMatch(TK.Is))
            {
                return Type();
            }

            return null;
        }

        private PtTypeParameter TypeParameter()
        {
            Begin();

            var name = Identifier();
            var type = TryColonType();
            var defaultType = TryDefaultType();

            return new PtTypeParameter(End(), name, type, defaultType);
        }

        private PtTypeParameters? TryTypeParameters()
        {
            if (MayBegin(TK.LSquare, TK.LSquareNew))
            {
                var parameters = List(TypeParameter);

                return new PtTypeParameters(End(TK.RSquare), parameters);
            }

            return null;
        }

        private PtCap? TryCap()
        {
            return TokenKind switch
            {
                TK.Iso => Cap(PtCapKind.Iso),
                TK.Trn => Cap(PtCapKind.Trn),
                TK.Ref => Cap(PtCapKind.Ref),
                TK.Val => Cap(PtCapKind.Val),
                TK.Box => Cap(PtCapKind.Box),
                TK.Tag => Cap(PtCapKind.Tag),
                _ => null,
            };
        }

        private PtCap? TryCapEx()
        {
            return TokenKind switch
            {
                TK.Iso => Cap(PtCapKind.Iso),
                TK.Trn => Cap(PtCapKind.Trn),
                TK.Ref => Cap(PtCapKind.Ref),
                TK.Val => Cap(PtCapKind.Val),
                TK.Box => Cap(PtCapKind.Box),
                TK.Tag => Cap(PtCapKind.Tag),
                TK.CapRead => Cap(PtCapKind.HashRead),
                TK.CapSend => Cap(PtCapKind.HashSend),
                TK.CapShare => Cap(PtCapKind.HashShare),
                TK.CapAlias => Cap(PtCapKind.HashAlias),
                TK.CapAny => Cap(PtCapKind.HashAny),
                _ => null,
            };
        }

        private PtCap Cap(PtCapKind kind)
        {
            Begin(TokenKind);
            return new PtCap(End(), kind);
        }

        private PtAnnotations? TryAnnotations()
        {
            if (MayBegin(TK.Backslash))
            {
                var names = List(Identifier);

                return new PtAnnotations(End(TK.Backslash), names);
            }

            return null;
        }
    }
}
