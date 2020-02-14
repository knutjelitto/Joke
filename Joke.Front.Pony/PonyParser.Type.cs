using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joke.Front.Pony
{
    partial class PonyParser
    {
        private static TK[] FirstViewpoint = new TK[] { TK.Arrow };

        private Tree.Type Type() => TryType() ?? throw NoParse("type");
        private Tree.Type? TryType()
        {
            Begin();

            var atom = TryAtomType();

            if (atom != null)
            {
                if (Iss(FirstViewpoint))
                {
                    var arrow = ArrowType();

                    return new Tree.ViewpointType(End(), atom, arrow);
                }
            }
            else
            {
                Discard();
            }

            return atom;
        }

        private Tree.ArrowType ArrowType()
        {
            Begin();

            Match("->", TK.Arrow);
            var type = Type();

            return new Tree.ArrowType(End(), type);
        }

        private Tree.Type? TryAtomType()
        {
            if (More())
            {
                switch (Kind)
                {
                    case TK.This:
                        Match();
                        return new Tree.ThisType(End());
                    case TK.LParen:
                    case TK.LParenNew:
                        return GroupedType();
                    case TK.Identifier:
                        return Nominal();
                    case TK.LBrace:
                        throw NotYet("atom-type -- lambdatype");
                    case TK.AtLBrace:
                        throw NotYet("atom-type -- barelambdatype");
                    default:
                        var cap = TryCap(false);
                        if (cap != null)
                        {
                            return cap;
                        }
                        break;
                }
            }

            return null;
        }

        private Tree.GroupedType GroupedType()
        {
            Debug.Assert(Iss(TK.LParen, TK.LParenNew));

            Begin();

            var types = new List<Tree.Type>();

            do
            {
                Match();
                var type = InfixType();
                types.Add(type);
            }
            while (Iss(TK.Comma));
            Match("')'", TK.RParen);

            return new Tree.GroupedType(End(), types);
        }

        private Tree.Type InfixType()
        {
            Begin();

            var type = Type();
            var parts = new List<Tree.InfixTypePart>();
            var done = false;
            while (!done && More())
            {
                switch (Kind)
                {
                    case TK.Pipe:
                        Begin();
                        Match();
                        var ptype = Type();
                        parts.Add(new Tree.UnionPart(End(), ptype));
                        break;
                    case TK.ISectType:
                        Begin();
                        Match();
                        var itype = Type();
                        parts.Add(new Tree.IntersectionPart(End(), itype));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            if (parts.Count > 1)
            {
                return new Tree.InfixType(End(), type, parts);
            }

            Discard();
            return type;
        }

        private Tree.Type Nominal()
        {
            Debug.Assert(Iss(TK.Identifier));

            Begin();

            var name = Identifier();
            if (Iss(TK.Dot))
            {
                Match();
                var name2 = Identifier();

                name = new Tree.DotIdentifier(End(), name, name2);
            }
            var typeArguments = TryTypeArguments();
            var cap = TryCap(true);
            var ea = EphemAlias();

            return new Tree.NominalType(End(), name, typeArguments, cap, ea);
        }

        private Tree.EphemAlias EphemAlias()
        {
            if (MayMatch(TK.Ephemeral))
            {
                return Tree.EphemAlias.Epemeral;
            }
            if (MayMatch(TK.Aliased))
            {
                return Tree.EphemAlias.Aliased;
            }
            return Tree.EphemAlias.None;
        }

        private Tree.TypeArguments TryTypeArguments()
        {
            Begin();

            var arguments = new List<Tree.TypeArgument>();
            if (Iss(TK.LSquare))
            {
                do
                {
                    Match();
                    var argument = TypeArgument();
                    arguments.Add(argument);
                }
                while (Iss(TK.Comma));
                Match("']'", TK.RSquare);
            }

            return new Tree.TypeArguments(End(), arguments);
        }

        private Tree.TypeArgument TypeArgument()
        {
            var type = TryType();

            if (type != null)
            {
                return new Tree.TypeArgumentType(type.Span, type);
            }

            throw NotYet("type-argument");
        }
    }
}
