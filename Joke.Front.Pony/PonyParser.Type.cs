using System.Collections.Generic;

using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony
{
    partial class PonyParser
    {
        private Tree.Type Type() => TryType() ?? throw NoParse("type");
        private Tree.Type? TryType()
        {
            Begin();

            var atom = TryAtomType();

            if (atom != null && Iss(TK.Arrow))
            {
                var arrow = ArrowType();

                return new Tree.ViewpointType(End(), atom, arrow);
            }

            Discard();

            return atom;
        }

        private Tree.ArrowType ArrowType()
        {
            Begin(TK.Arrow);
            var type = Type();
            return new Tree.ArrowType(End(), type);
        }

        private Tree.Type? TryAtomType()
        {
            if (More())
            {
                switch (TokenKind)
                {
                    case TK.This:
                        return ThisType();
                    case TK.LParen:
                    case TK.LParenNew:
                        return GroupedType();
                    case TK.Identifier:
                        return Nominal();
                    case TK.LBrace:
                        return LambdaType(false);
                    case TK.AtLBrace:
                        return LambdaType(true);
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

        private Tree.ThisType ThisType()
        {
            Begin(TK.This);
            return new Tree.ThisType(End());
        }

        private Tree.LambdaType LambdaType(bool bare)
        {
            Begin(TK.LBrace, TK.AtLBrace);
            var receiverCap = TryCap(false);
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaTypeParameters();
            var returnType = TryColonType();
            var partial = TryPartial();
            Match(TK.RBrace);
            var referenceCap = TryCap(false);
            var ea = EphemAlias();

            return new Tree.LambdaType(End(), bare, receiverCap, name, typeParameters, parameters, returnType, partial, referenceCap, ea);
        }

        private Tree.LambdaTypeParameters LambdaTypeParameters()
        {
            Begin(TK.LParen, TK.LParenNew);

            var types = new List<Tree.Type>();

            if (Issnt(TK.RParen))
            {
                do
                {
                    types.Add(Type());
                }
                while (MayMatch(TK.Comma));
            }
            Match(TK.RParen);

            return new Tree.LambdaTypeParameters(End(), types);
        }

        private Tree.GroupedType GroupedType()
        {
            Begin(TK.LParen, TK.LParenNew);
            var types = new List<Tree.Type>();
            do
            {
                var type = InfixType();
                types.Add(type);
            }
            while (MayMatch(TK.Comma));
            Match(TK.RParen);

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
                switch (TokenKind)
                {
                    case TK.Pipe:
                        Begin();
                        Match(TK.Pipe);
                        var ptype = Type();
                        parts.Add(new Tree.UnionPart(End(), ptype));
                        break;
                    case TK.ISectType:
                        Begin();
                        Match(TK.ISectType);
                        var itype = Type();
                        parts.Add(new Tree.IntersectionPart(End(), itype));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            if (parts.Count > 0)
            {
                return new Tree.InfixType(End(), type, parts);
            }

            Discard();
            return type;
        }

        private Tree.Type Nominal()
        {
            Begin();
            var name = DotIdentifier();
            var typeArguments = TryTypeArguments();
            var cap = TryCap(true);
            var ea = EphemAlias();

            return new Tree.NominalType(End(), name, typeArguments, cap, ea);
        }

        private Tree.Identifier DotIdentifier()
        {
            Begin();
            var name = Identifier();
            if (Iss(TK.Dot))
            {
                Match(TK.Dot);

                var name2 = Identifier();

                return new Tree.DotIdentifier(End(), name, name2);
            }

            Discard();
            return name;
        }

        private Tree.EphemAlias EphemAlias()
        {
            Begin();

            if (MayMatch(TK.Ephemeral))
            {
                return new Tree.EphemAlias(End(), Tree.EAKind.Epemeral);
            }
            if (MayMatch(TK.Aliased))
            {
                return new Tree.EphemAlias(End(), Tree.EAKind.Aliased);
            }

            return new Tree.EphemAlias(End(), Tree.EAKind.None);
        }

        private Tree.TypeArguments TypeArguments() => TryTypeArguments() ?? throw NoParse("type-arguments");
        private Tree.TypeArguments? TryTypeArguments()
        {
            if (MayBegin(TK.LSquare))
            {
                var arguments = new List<Tree.TypeArgument>();

                arguments.Add(TypeArgument());

                while (Iss(TK.Comma))
                {
                    Match(TK.Comma);
                    arguments.Add(TypeArgument());
                }

                Match(TK.RSquare);

                return new Tree.TypeArguments(End(), arguments);
            }

            return null;
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
