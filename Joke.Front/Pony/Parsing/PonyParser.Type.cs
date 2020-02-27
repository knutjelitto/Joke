using System.Collections.Generic;
using System.Linq;

using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.ParseTree;

namespace Joke.Front.Pony.Parsing
{
    partial class PonyParser
    {
        private PtType Type() => TryType() ?? throw NoParse("type");
        private PtType? TryType()
        {
            var atom = TryAtomType();

            if (atom != null && Iss(TK.Arrow))
            {
                var arrow = ArrowType();

                return new PtViewpointType(Mark(atom), atom, arrow);
            }

            return atom;
        }

        private PtType ArrowType()
        {
            Match(TK.Arrow);
            return Type();
        }

        private PtType? TryAtomType()
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
                    return TryCap();
            }
        }

        private PtThisType ThisType()
        {
            Begin(TK.This);
            return new PtThisType(End());
        }

        private PtLambdaType LambdaType(bool bare)
        {
            Begin(First.Lambda);
            var receiverCap = TryCap();
            var name = TryIdentifier();
            var typeParameters = TryTypeParameters();
            var parameters = LambdaTypeParameters();
            var returnType = TryColonType();
            var partial = MayPartial();
            Match(TK.RBrace);
            var referenceCap = TryCap();
            var ea = TryEphemAlias();
            return new PtLambdaType(End(), bare, receiverCap, name, typeParameters, parameters, returnType, partial, referenceCap, ea);
        }

        private PtLambdaTypeParameters LambdaTypeParameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var types = List(Type, TK.RParen);
            return new PtLambdaTypeParameters(End(TK.RParen), types);
        }

        private PtGroupedType GroupedType()
        {
            Begin(TK.LParen, TK.LParenNew);
            var types = List(InfixType);
            return new PtGroupedType(End(TK.RParen), types);
        }

        private PtType InfixType()
        {
            var type = Type();
            var parts = new List<Parts.InfixTypePart>();
            var done = false;
            while (!done)
            {
                switch (TokenKind)
                {
                    case TK.Pipe:
                        Begin(TK.Pipe);
                        var ptype = Type();
                        parts.Add(new Parts.InfixTypePart(End(), PtInfixTypeKind.Union, ptype));
                        break;
                    case TK.ISectType:
                        Begin(TK.ISectType);
                        var itype = Type();
                        parts.Add(new Parts.InfixTypePart(End(), PtInfixTypeKind.Intersection, itype));
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            if (parts.Count >= 1)
            {
                var operators = parts.Select(p => p.Kind).Distinct().ToList();
                if (operators.Count >= 2)
                {
                    throw NoParse("binary type operators have no precedence, use ( ) to group type expressions");
                }
                var @operator = operators[0];
                var operands = new List<PtType> { type };
                operands.AddRange(parts.Cast<Parts.InfixTypePart>().Select(b => b.Right));
                return new PtInfixType(Mark(type), @operator, operands);
            }

            return type;
        }

        private PtType Nominal()
        {
            Begin();
            var name = DotIdentifier();
            var typeArguments = TryTypeArguments();
            var cap = TryCapEx();
            var ea = TryEphemAlias();

            return new PtNominalType(End(), name, typeArguments, cap, ea);
        }

        private PtIdentifier DotIdentifier()
        {
            var name = Identifier();
            if (Iss(TK.Dot))
            {
                Match(TK.Dot);

                var name2 = Identifier();

                return new PtDotIdentifier(Mark(name), name, name2);
            }

            return name;
        }

        private PtEphemAlias? TryEphemAlias()
        {
            if (MayBegin(TK.Ephemeral))
            {
                return new PtEphemAlias(End(), PtEAKind.Epemeral);
            }
            if (MayBegin(TK.Aliased))
            {
                return new PtEphemAlias(End(), PtEAKind.Aliased);
            }

            return null;
        }

        private PtTypeArguments TypeArguments() => TryTypeArguments() ?? throw NoParse("type-arguments");
        private PtTypeArguments? TryTypeArguments()
        {
            if (MayBegin(TK.LSquare))
            {
                var arguments = List(TypeArgument);


                return new PtTypeArguments(End(TK.RSquare), arguments);
            }

            return null;
        }

        private PtType TypeArgument()
        {
            return Type();
        }
    }
}
