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

        private Tree.Type Type()
        {
            Begin();

            var atom = AtomType();

            if (Iss(FirstViewpoint))
            {
                var arrow = ArrowType();

                return new Tree.ViewpointType(End(), atom, arrow);
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

        private Tree.Type AtomType()
        {
            Begin();

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
            }

            throw NotYet("atom-type");
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
            throw NotYet("infix-type");
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
            throw NotYet("nominal");
        }
    }
}
