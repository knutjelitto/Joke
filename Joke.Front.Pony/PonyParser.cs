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
            this.source = source;
            toks = tokens;
            next = 0;
            limit = toks.Count;
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
            while (!done && More())
            {
                switch (Kind)
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
            Debug.Assert(Iss(TK.Use));

            Begin(); Match();

            var name = TryUseName();

            if (Iss(TK.At))
            {
                Match();
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
                Match("'='", TK.Assign);
                return new Tree.UseName(End(), name);
            }

            Discard();
            return null;
        }

        public Tree.Class Class(Tree.ClassKind kind)
        {
            Debug.Assert(Iss(FirstClass));

            Begin(); Match();

            var annotations = Annotations();
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
            while (!done && More())
            {
                switch (Kind)
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
            Debug.Assert(Iss(TK.Var, TK.Let, TK.Embed));

            Begin(); Match();

            var name = Identifier();
            var type = ColonType();
            var value = TryDefaultArg();
            var doc = TryString();

            return new Tree.Field(End(), kind, name, type, value, doc);
        }

        private Tree.Methods Methods()
        {
            Begin();

            var methods = new List<Tree.Method>();
            while (More())
            {
                var kind = Tree.MethodKind.Missing;

                switch(Kind)
                {
                    case TK.Fun:
                        kind = Tree.MethodKind.Fun;
                        break;
                    case TK.Be:
                        kind = Tree.MethodKind.Be;
                        break;
                    case TK.New:
                        kind = Tree.MethodKind.New;
                        break;
                }

                if (kind != Tree.MethodKind.Missing)
                {
                    methods.Add(Method(kind));
                }
                else
                {
                    break;
                }
            }

            return new Tree.Methods(End(), methods);
        }

        private Tree.Method Method(Tree.MethodKind kind)
        {
            Debug.Assert(Iss(TK.Fun, TK.Be, TK.New));

            Begin();

            Match();
            var annotations = Annotations();
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
                Begin();

                Match();

                var expression = TryRawSeq() ?? throw NoParse("body -- raw-seq");

                return new Tree.Body(End(), expression);
            }

            return null;
        }

        private Tree.Parameters Parameters()
        {
            Begin();

            var parameters = new List<Tree.Parameter>();

            Match("'('", TK.LParen, TK.LParenNew);
            if (Iss(FirstParameter))
            {
                do
                {
                    parameters.Add(Parameter());
                }
                while (MayMatch(TK.Comma));
            }
            Match("')'", TK.RParen);

            return new Tree.Parameters(End(), parameters);
        }

        private Tree.Parameter Parameter()
        {
            Begin();

            if (Iss(TK.Ellipsis))
            {
                Match();
                return new Tree.EllipsisParameter(End());
            }

            var name = Identifier();
            var type = ColonType();
            var value = TryDefaultArg();

            return new Tree.RegularParameter(End(), name, type, value);
        }

        private Tree.DefaultArg? TryDefaultArg()
        {
            if (Iss(TK.Assign))
            {
                Begin();Match();

                var expression = TryInfix() ?? throw NoParse("default - value");

                return new Tree.DefaultArg(End(), expression);
            }

            return null;
        }

        private Tree.DefaultType? TryDefaultType()
        {
            if (Iss(TK.Assign))
            {
                Begin(); Match();

                var type = TypeArgument();

                return new Tree.DefaultType(End(), type);
            }

            return null;
        }

        private Tree.ColonType ColonType()
        {
            Begin();

            Match("':'", TK.Colon);
            var type = Type();

            return new Tree.ColonType(End(), type);
        }

        private Tree.ColonType? TryColonType()
        {
            if (Iss(TK.Colon))
            {
                Begin();

                Match("':'", TK.Colon);
                var type = Type();

                return new Tree.ColonType(End(), type);
            }

            return null;
        }

        private Tree.Bare? TryBare()
        {
            if (Iss(TK.At))
            {
                Begin(); Match();
                return new Tree.Bare(End());
            }

            return null;
        }

        private Tree.Type? TryProvides()
        {
            if (Iss(TK.Is))
            {
                Match();
                return Type();
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

        private Tree.TypeParameters TryTypeParameters()
        {
            Begin();

            var parameters = new List<Tree.TypeParameter>();

            if (Iss(TK.LSquare, TK.LSquareNew))
            {
                do
                {
                    Match();
                    parameters.Add(TypeParameter());
                }
                while (Iss(TK.Comma));

                Match("']'", TK.RSquare);
            }

            return new Tree.TypeParameters(End(), parameters);
        }

        private Tree.Cap? TryCap(bool extended)
        {
            if (More())
            {
                Begin();

                var cap = Tree.CapKind.Missing;

                switch (Kind)
                {
                    case TK.Iso:
                        cap = Tree.CapKind.Iso;
                        break;
                    case TK.Trn:
                        cap = Tree.CapKind.Trn;
                        break;
                    case TK.Ref:
                        cap = Tree.CapKind.Ref;
                        break;
                    case TK.Val:
                        cap = Tree.CapKind.Val;
                        break;
                    case TK.Box:
                        cap = Tree.CapKind.Box;
                        break;
                    case TK.Tag:
                        cap = Tree.CapKind.Tag;
                        break;
                    case TK.CapRead when extended:
                        cap = Tree.CapKind.HashRead;
                        break;
                    case TK.CapSend when extended:
                        cap = Tree.CapKind.HashSend;
                        break;
                    case TK.CapShare when extended:
                        cap = Tree.CapKind.HashShare;
                        break;
                    case TK.CapAlias when extended:
                        cap = Tree.CapKind.HashAlias;
                        break;
                    case TK.CapAny when extended:
                        cap = Tree.CapKind.HashAny;
                        break;
                }

                if (cap != Tree.CapKind.Missing)
                {
                    Match();
                    return new Tree.Cap(End(), cap);
                }
            }

            return null;
        }

        public Tree.Annotations Annotations()
        {
            Begin();

            var names = new List<Tree.Identifier>();

            if (Iss(TK.Backslash))
            {
                do
                {
                    Match();
                    names.Add(Identifier());
                }
                while (Iss(TK.Comma));
                Match("backslash", TK.Backslash);
            }

            return new Tree.Annotations(End(), names);
        }

        /*
         * Errros
         */
        public int Offset => toks[next].Payload;

        protected NotYetException NotYet(string message)
        {
            return new NotYetException(message);
        }

        protected NoParseException NoParse(string message)
        {
            return new NoParseException(message);
        }

    }
}
