using System.Collections.Generic;
using System.Linq;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing
{
    partial class PonyParser
    {
        private Ast.Type Type() => TryType() ?? throw NoParse("type");
        private Ast.Type? TryType()
        {
            var atom = TryAtomType();

            if (atom != null && Iss(TK.Arrow))
            {
                var arrow = ArrowType();

                return new Ast.ViewpointType(Mark(atom), atom, arrow);
            }

            return atom;
        }

        private Ast.Type ArrowType()
        {
            Match(TK.Arrow);
            return Type();
        }

        private Ast.Type? TryAtomType()
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

        private Ast.ThisType ThisType()
        {
            Begin(TK.This);
            return new Ast.ThisType(End());
        }

        private Ast.LambdaType LambdaType(bool bare)
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
            return new Ast.LambdaType(End(), bare, receiverCap, name, typeParameters, parameters, returnType, partial, referenceCap, ea);
        }

        private Ast.LambdaTypeParameters LambdaTypeParameters()
        {
            Begin(TK.LParen, TK.LParenNew);
            var types = List(Type, TK.RParen);
            return new Ast.LambdaTypeParameters(End(TK.RParen), types);
        }

        private Ast.GroupedType GroupedType()
        {
            Begin(TK.LParen, TK.LParenNew);
            var types = List(InfixType);
            return new Ast.GroupedType(End(TK.RParen), types);
        }

        private Ast.Type InfixType()
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
                        parts.Add(new Parts.InfixTypePart(End(), Ast.InfixTypeKind.Union, ptype));
                        break;
                    case TK.ISectType:
                        Begin(TK.ISectType);
                        var itype = Type();
                        parts.Add(new Parts.InfixTypePart(End(), Ast.InfixTypeKind.Intersection, itype));
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
                var operands = new List<Ast.Type> { type };
                operands.AddRange(parts.Cast<Parts.InfixTypePart>().Select(b => b.Right));
                return new Ast.InfixType(Mark(type), @operator, operands);
            }

            return type;
        }

        private Ast.Type Nominal()
        {
            Begin();
            var name = DotIdentifier();
            var typeArguments = TryTypeArguments();
            var cap = TryCapEx();
            var ea = TryEphemAlias();

            return new Ast.NominalType(End(), name, typeArguments, cap, ea);
        }

        private Ast.Identifier DotIdentifier()
        {
            var name = Identifier();
            if (Iss(TK.Dot))
            {
                Match(TK.Dot);

                var name2 = Identifier();

                return new Ast.DotIdentifier(Mark(name), name, name2);
            }

            return name;
        }

        private Ast.EphemAlias? TryEphemAlias()
        {
            if (MayBegin(TK.Ephemeral))
            {
                return new Ast.EphemAlias(End(), Ast.EAKind.Epemeral);
            }
            if (MayBegin(TK.Aliased))
            {
                return new Ast.EphemAlias(End(), Ast.EAKind.Aliased);
            }

            return null;
        }

        private Ast.TypeArguments TypeArguments() => TryTypeArguments() ?? throw NoParse("type-arguments");
        private Ast.TypeArguments? TryTypeArguments()
        {
            if (MayBegin(TK.LSquare))
            {
                var arguments = List(TypeArgument);


                return new Ast.TypeArguments(End(TK.RSquare), arguments);
            }

            return null;
        }

        private Ast.Type TypeArgument()
        {
            return Type();
        }
    }
}
