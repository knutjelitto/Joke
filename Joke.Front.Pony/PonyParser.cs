using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony
{
    public partial class PonyParser
    {
        public PonyParser(IReadOnlyList<Token> tokens)
        {
            toks = tokens;
            next = 0;
            limit = toks.Count;
        }

        private int next;
        private readonly int limit;
        private readonly IReadOnlyList<Token> toks;


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

            var doc = OptString();

            var uses = new List<Tree.Use>();

            while (Iss(TK.Use))
            {
                uses.Add(Use());
            }

            var classes = new List<Tree.Class>();

            while (Iss(FirstClass))
            {
                classes.Add(Class());
            }

            throw NotYet("module");
        }

        public Tree.Use Use()
        {
            throw NotYet("use");
        }

        public Tree.Class Class()
        {
            Debug.Assert(Iss(FirstClass));

            Begin();

            var kind = toks[next].Kind;
            Match();

            var bare = TryBare();
            var cap = TryCap(false);
            var name = Identifier();
            var typeParams = TryTypeParameters();
            var provides = OptProvides();
            var doc = OptString();
            var members = Members();


            throw NotYet("class-def");
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
            while (Iss(TK.Var, TK.Let, TK.Embed))
            {
                fields.Add(Field());
            }

            return new Tree.Fields(End(), fields);
        }

        private Tree.Field Field()
        {
            Debug.Assert(Iss(TK.Var, TK.Let, TK.Embed));

            throw NotYet("methods");
        }

        private Tree.Methods Methods()
        {
            Begin();

            var methods = new List<Tree.Method>();
            while (next < limit)
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
            var partial = MayMatch(TK.Question);
            var doc = TryString();
            var body = TryBody();
            
            throw NotYet("method");
        }

        private Tree.Body? TryBody()
        {
            if (Iss(TK.DblArrow))
            {
                Begin();

                Match();

                var expression = RawSeq();

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
                Begin();

                Match();
                var expression = Infix();

                return new Tree.DefaultArg(End(), expression);
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

        private Tree.Bare TryBare()
        {
            Begin();

            if (Iss(TK.At))
            {
                Match();
            }

            return new Tree.Bare(End());
        }

        private Tree.Type? OptProvides()
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
            throw NotYet("type-parameter");
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

        private Tree.Cap TryCap(bool extended)
        {
            Begin();

            var cap = Tree.CapKind.Missing;

            if (next < limit)
            {
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
                    next += 1;
                }
            }

            return new Tree.Cap(End(), cap);
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

        //================= Literals

        public Tree.Identifier Identifier()
        {
            if (Iss(TK.Identifier))
            {
                Begin();

                Match();

                return new Tree.Identifier(End());
            }

            throw NoParse("identifier expected");
        }

        public Tree.String? OptString()
        {
            if (Iss(TK.String, TK.DocString))
            {
                Begin();

                Match();

                return new Tree.String(End());
            }
            return null;
        }

        //================= Helpers

        private TK Kind => toks[next].Kind;

        private bool MayMatch(TK kind)
        {
            if (next < limit && toks[next].Kind == kind)
            {
                next += 1;
                return true;
            }

            return false;
        }

        private void Match(string fail, TK kind)
        {
            if (next < limit && toks[next].Kind == kind)
            {
                next += 1;
                return;
            }

            throw NoParse($"{fail} expected");
        }

        private void Match(string fail, params TK[] kinds)
        {
            if (next < limit)
            {
                for (var i = 0; i < kinds.Length; ++i)
                {
                    if (kinds[i] == toks[next].Kind)
                    {
                        next += 1;
                        return;
                    }
                }
            }

            throw NoParse($"{fail} expected");
        }

        private bool Iss(TK kind)
        {
            return next < limit && toks[next].Kind == kind;
        }

        private bool Iss(params TK[] kinds)
        {
            if (next < limit)
            {
                for (var i = 0; i < kinds.Length; ++i)
                {
                    if (kinds[i] == toks[next].Kind)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Next()
        {
            if (next < limit)
            {
                next += 1;
            }
        }

        private void Match()
        {
            if (next < limit)
            {
                next += 1;
                return;
            }

            throw NoParse("expected something (not EOF)");
        }

        private void Ensure()
        {
            if (next >= limit)
            {
                throw NoParse("expected something (not EOF)");
            }
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
