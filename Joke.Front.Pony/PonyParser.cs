using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony
{
    public class PonyParser
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

        public Tree.Module Module()
        {
            var start = next;

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

            var start = next;
            var kind = toks[next].Kind;
            Match();

            var at = MayMatch(TK.At);
            var cap = OptCap(false);
            var name = Identifier();
            var typeParams = OptTypeParameters();
            var provides = OptProvides();
            var doc = OptString();
            var members = Members();


            throw NotYet("class-def");
        }

        private Tree.Members Members()
        {
            var start = next;

            var fields = Fields();
            var methods = Methods();

            return new Tree.Members(Span(start), fields, methods);
        }

        private Tree.Fields Fields()
        {
            var start = next;

            var fields = new List<Tree.Field>();
            while (Iss(TK.Var, TK.Let, TK.Embed))
            {
                fields.Add(Field());
            }

            return new Tree.Fields(Span(start), fields);
        }

        private Tree.Field Field()
        {
            Debug.Assert(Iss(TK.Var, TK.Let, TK.Embed));

            throw NotYet("methods");
        }

        private Tree.Methods Methods()
        {
            var start = next;

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
                        kind = Tree.MethodKind.Fun;
                        break;
                    case TK.New:
                        kind = Tree.MethodKind.Fun;
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

            return new Tree.Methods(Span(start), methods);
        }

        private Tree.Method Method(Tree.MethodKind kind)
        {
            Debug.Assert(Iss(TK.Fun, TK.Be, TK.New));
            
            var start = next;

            Match();
            var annotations = Annotations();
            
            throw NotYet("method");
        }

        private Tree.Type? OptProvides()
        {
            if (Iss(TK.Is))
            {
                return Type();
            }

            return null;
        }

        private Tree.Type Type()
        {
            throw NotYet("type");
        }

        private Tree.TypeParameter TypeParameter()
        {
            throw NotYet("type-parameter");
        }

        private Tree.TypeParameters OptTypeParameters()
        {
            if (Iss(TK.LSquare, TK.LSquareNew))
            {
                var start = next;

                var parameters = new List<Tree.TypeParameter>();
                do
                {
                    next += 1;
                    parameters.Add(TypeParameter());
                }
                while (Iss(TK.Comma));

                Match("']'", TK.RSquare);

                return new Tree.TypeParameters(Span(start), parameters);
            }

            return new Tree.TypeParameters(Span(), new Tree.TypeParameter[] { });
        }

        private Tree.Cap OptCap(bool extended)
        {
            if (next < limit)
            {
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
                    next += 1;
                    return new Tree.Cap(Span(next - 1), cap);
                }
            }

            return new Tree.Cap(Span(next), Tree.CapKind.Missing);
        }

        public Tree.Annotations Annotations()
        {
            if (Iss(TK.Backslash))
            {
                var start = next;
                var names = new List<Tree.Identifier>();
                do
                {
                    Match();
                    names.Add(Identifier());
                }
                while (Iss(TK.Comma));
                Match("backslash", TK.Backslash);

                return new Tree.Annotations(Span(start), names);
            }

            return new Tree.Annotations(Span(next), new Tree.Identifier[] { });
        }

        //================= Literals

        public Tree.Identifier Identifier()
        {
            if (Iss(TK.Identifier))
            {
                return new Tree.Identifier(EatSpan());
            }

            throw NoParse("identifier expected");
        }

        public Tree.String? OptString()
        {
            if (Iss(TK.String, TK.DocString))
            {
                return new Tree.String(EatSpan());
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

        private TSpan Span(int start)
        {
            Debug.Assert(next <= limit);

            return new TSpan(toks, start, next);
        }

        private TSpan Span()
        {
            return new TSpan(toks, next, next);
        }

        private TSpan EatSpan()
        {
            Debug.Assert(next < limit);

            int start = next;

            next += 1;

            return new TSpan(toks, start, next);
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

        private int Eat()
        {
            Debug.Assert(next < limit);

            var start = next;

            next += 1;

            return start;
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
